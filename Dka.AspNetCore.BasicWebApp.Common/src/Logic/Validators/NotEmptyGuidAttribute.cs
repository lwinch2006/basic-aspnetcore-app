using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Dka.AspNetCore.BasicWebApp.Common.Logic.Validators
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public NotEmptyGuidAttribute()
            : base("Value cannot be empty.")
        {
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            SetLocalizedErrorMessage(validationContext);
            
            var targetGuidAsObject = value;

            if (targetGuidAsObject == null || validationContext == null)
            {
                return ValidationResult.Success;
            }

            var targetGuid = (Guid)targetGuidAsObject;

            return targetGuid.Equals(Guid.Empty) 
                ? new ValidationResult(ErrorMessage, new[] {validationContext.MemberName}) 
                : ValidationResult.Success;
        }
        
        private void SetLocalizedErrorMessage(ValidationContext validationContext)
        {
            if (validationContext.GetService(typeof(IHtmlLocalizer)) is not IHtmlLocalizer htmlLocalizer)
            {
                return;
            }

            ErrorMessage = htmlLocalizer["profilepicture.modelvalidation.useridempty"].Value;
        }        
    }
}