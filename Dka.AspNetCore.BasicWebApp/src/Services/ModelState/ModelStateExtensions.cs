using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dka.AspNetCore.BasicWebApp.Services.ModelState
{
    public static class ModelStateExtensions
    {
        public static object[] GetModelStateErrorMessages(this ModelStateDictionary modelState)
        {
            var errorMessages = new List<object>();
            
            foreach (var modelStateEntry in modelState.Values)
            {
                errorMessages.AddRange(modelStateEntry.Errors.Select(error => error.ErrorMessage));
            }

            return errorMessages.ToArray();
        }
        
        public static string ToSummary(this ModelStateDictionary modelState)
        {
            var errorMessage = string.Empty;
                
            foreach (var (key, modelStateValue) in modelState)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    errorMessage += $"{key}: {error.ErrorMessage} {Environment.NewLine}";
                }
            }

            return errorMessage;
        }
        
        public static void ValidateModel(this ControllerBase controller, object viewModel, string propertyPrefix)
        {
            if (viewModel == null)
            {
                controller.ModelState.AddModelError(nameof(viewModel), "Model is null");
                return;
            }

            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(viewModel, null, null);
            Validator.TryValidateObject(viewModel, ctx, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    controller.ModelState.AddModelError($"{propertyPrefix}{memberName}", validationResult.ErrorMessage);
                }
            }
        }        
    }
}