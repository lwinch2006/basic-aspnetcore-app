using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}