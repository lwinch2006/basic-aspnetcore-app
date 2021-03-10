using System;
using System.ComponentModel.DataAnnotations;

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
    }
}