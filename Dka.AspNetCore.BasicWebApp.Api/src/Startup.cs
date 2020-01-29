using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Services.ServiceCollection;
using Dka.AspNetCore.BasicWebApp.Common.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using DbUp;
using Dka.AspNetCore.BasicWebApp.Api.Models.AutoMapper;
using Dka.AspNetCore.BasicWebApp.Api.Models.Configurations;
using Dka.AspNetCore.BasicWebApp.Api.Models.ExceptionProcessing;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Logic.Authorization;
using Dka.AspNetCore.BasicWebApp.Common.Models.Authentication;
using Dka.AspNetCore.BasicWebApp.Common.Models.Constants;
using Dka.AspNetCore.BasicWebApp.Common.Models.ExceptionProcessing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Dka.AspNetCore.BasicWebApp.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly string _appName;
        private readonly IConfiguration _configuration;
        private readonly DatabaseConfiguration _databaseConfiguration;
        private IServiceCollection _services;
        
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _appName = environment.ApplicationName;
            _configuration = configuration;
            _databaseConfiguration = new DatabaseConfiguration();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            _configuration.GetSection($"{_appName}:{AppSettingsJsonFileSections.BaseWebAppContext}").Bind(_databaseConfiguration);

            services.AddSingleton(_databaseConfiguration);
            services.AddDatabaseClasses(_databaseConfiguration);
            services
                .AddHealthChecks()
                .AddSqlServer(_databaseConfiguration.ConnectionString, null, "Database", null, new[] {"db-status-check"}, null)
                .AddCheck("Memory", () => HealthCheckResult.Healthy(), new [] { "memory-status-check" });
            services.AddAutoMapper(typeof(BasicWebAppApiProfile));

            var jwtConfiguration = _configuration.GetSection($"{_appName}:{AppSettingsJsonFileSections.Jwt}").Get<JwtConfiguration>();
            
            services.AddOptions();
            services.Configure<JwtConfiguration>(_configuration.GetSection($"{_appName}:{AppSettingsJsonFileSections.Jwt}"));
            
            // Defining authentication.
            services
                .AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        _configuration.GetSection($"{_appName}:{AppSettingsJsonFileSections.Jwt}:{nameof(JwtBearerOptions)}").Bind(options);
                        
                        options.TokenValidationParameters.IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration.Secret));
                    });
            
            services.AddAuthorization();

            var authorizationOptions = Options.Create(new AuthorizationOptions());
            
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForAdministrator>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForSupport>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerForPowerUser>();
            services.AddSingleton<IAuthorizationHandler, DataOperationAuthorizationHandlerBasedOnRight>();
            services.AddSingleton<IAuthorizationPolicyProvider>(sp => new DataOperationAuthorizationPolicyProvider(JwtBearerDefaults.AuthenticationScheme, authorizationOptions));            
            
            services.AddControllers(config =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(authorizationPolicy));                
            });
            
            _services = services;
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, ILoggerFactory loggerFactory, IHostEnvironment environment)
        {
            loggerFactory.AddSerilog();
            
            RunDbMigrations(logger);

            if (environment.IsDevelopment())
            {
                RunDbMigrationsInDevelopmentEnvironment(logger);

                SeedDummyUsersWithRoles();
                
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(configure =>
            {
                configure.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                configure.MapHealthChecks("/health");
                configure.MapHealthChecks("/health/database", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("db-status-check")
                });
                configure.MapHealthChecks("/health/memory", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("memory-status-check")
                });
                configure.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("db-status-check") && check.Tags.Contains("memory-status-check") 
                });
                configure.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = _ => false
                });
            });
            
            logger.LogInformation("WebAPI initialised.");
        }

        private void RunDbMigrations(ILogger<Startup> logger)
        {
            EnsureDatabase.For.SqlDatabase(_databaseConfiguration.ConnectionString);
            
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_databaseConfiguration.ConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), scriptName => !scriptName.Contains("seed-dev-data", StringComparison.OrdinalIgnoreCase))
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                logger.LogError(result.Error.Message);
                
                throw new ApiDbRunMigrationsException("Database migrations run failure", result.Error);
            }

            logger.LogInformation("Database migrations run success.");
        }

        private void RunDbMigrationsInDevelopmentEnvironment(ILogger<Startup> logger)
        {
            EnsureDatabase.For.SqlDatabase(_databaseConfiguration.ConnectionString);
            
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_databaseConfiguration.ConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), scriptName => scriptName.Contains("seed-dev-data", StringComparison.OrdinalIgnoreCase))
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                logger.LogError(result.Error.Message);
                
                throw new ApiDbRunMigrationsException("Database migrations (Development environment) run failure", result.Error);
            }

            logger.LogInformation("Database migrations (Development environment) run success.");            
            
            
            
            
        }

        private async void SeedDummyUsersWithRoles()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var dummyPassword = "Test@123";
            var dummyUsers = (await ApplicationUser.GetDummyUserSet()).ToList();
            var dummyRoles = (await ApplicationRole.GetDummyRoleSet()).ToList();

            foreach (var dummyRole in dummyRoles)
            {
                await EnsureRole(serviceProvider, dummyRole);

                var dummyUsersPerRole = dummyUsers
                    .Where(record => record.Email.StartsWith(dummyRole.Name, StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var dummyUserPerRole in dummyUsersPerRole)
                {
                    await EnsureUser(serviceProvider, dummyUserPerRole, dummyRole, dummyPassword);
                }
            }
        }
        
        private static async Task EnsureUser(IServiceProvider serviceProvider, ApplicationUser user, ApplicationRole role, string password)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            if (await userManager.FindByNameAsync(user.UserName) is { } _)
            {
                return;
            }
            
            if (!(await userManager.CreateAsync(user, password) is { } _))
            {
                throw new BasicWebAppException("The password is probably not strong enough!");
            }

            await userManager.AddToRoleAsync(user, role.Name);
        }

        private static async Task EnsureRole(IServiceProvider serviceProvider, ApplicationRole role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();

            if (!(await roleManager.FindByNameAsync(role.Name) is { } _))
            {
                await roleManager.CreateAsync(role);
            }
        }

    }
}