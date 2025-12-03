using System.ComponentModel.DataAnnotations;

namespace DirtBikePark.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PositiveIdAttribute : ValidationAttribute
    {
        public PositiveIdAttribute() : base("The {0} field must be a positive integer (greater than 0).") { }

        // The validation logic overriding this base method
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if the value is null
            if (value == null)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            // Attempt to cast the value to an integer
            if (value is int id)
            {
                // Validation check
                if (id > 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName ?? validationContext.DisplayName });
                }
            }

            // If the type is not an integer
            return new ValidationResult("Invalid data type for ID.");
        }
    }
}