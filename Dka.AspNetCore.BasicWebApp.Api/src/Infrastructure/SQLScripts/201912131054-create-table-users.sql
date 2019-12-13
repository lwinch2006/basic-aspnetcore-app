IF NOT EXISTS(SELECT * FROM [Information_Schema].[Tables] WHERE Table_Name='Users')
BEGIN
    CREATE TABLE [Users] (
        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED,

        [UserName] NVARCHAR(100) NOT NULL,
        [NormalizedUserName] NVARCHAR(100) NOT NULL CONSTRAINT [IX_Users_NormalizedUserName] UNIQUE NONCLUSTERED,

        [Email] NVARCHAR(100) NOT NULL,
        [NormalizedEmail] NVARCHAR(100) NOT NULL CONSTRAINT [IX_Users_NormalizedEmail] UNIQUE NONCLUSTERED,
        [EmailConfirmed] BIT NOT NULL,

        [PhoneNumber] NVARCHAR(50) NULL,
        [PhoneNumberConfirmed] BIT NOT NULL,

        [PasswordHash] NVARCHAR(max) NOT NULL,
        [SecurityStamp] NVARCHAR(max) NULL,
        [ConcurrencyStamp] NVARCHAR(max) NULL,

        [AccessFailedCount] INT NOT NULL,

        [LockoutEnabled] BIT NOT NULL,
        [LockoutEnd] DATETIME NULL,

        [TwoFactorEnabled] BIT NOT NULL,

        [Guid] UNIQUEIDENTIFIER NOT NULL
    );            
END;