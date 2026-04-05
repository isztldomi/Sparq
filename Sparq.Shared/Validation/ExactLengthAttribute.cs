using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Sparq.Shared.Validation
{
    public class ExactLengthAttribute : ValidationAttribute
    {
        private readonly int _length;

        public ExactLengthAttribute(int length)
        {
            _length = length;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var count = (value as ICollection)?.Count ?? 0;

            if (count != _length)
            {
                return new ValidationResult(ErrorMessage ?? $"Must contain exactly {_length} items.");
            }

            return ValidationResult.Success;
        }
    }
}