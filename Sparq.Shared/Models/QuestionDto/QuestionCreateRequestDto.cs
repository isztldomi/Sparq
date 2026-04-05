using Sparq.Shared.Models.AnswerDto;
using System.ComponentModel.DataAnnotations;

namespace Sparq.Shared.Models.QuestionDto
{
    public class QuestionCreateRequestDto : IValidatableObject
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Text is required.")]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Text must be between 3 and 2000 characters.")]
        public string Text { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Media URL cannot exceed 500 characters.")]
        public string MediaUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Time limit is required.")]
        [Range(10, 7200, ErrorMessage = "Time limit must be between 10 seconds and 2 hours.")]
        public int TimeLimit { get; set; }

        [Required(ErrorMessage = "Point value is required.")]
        [Range(1, 10, ErrorMessage = "Point value must be between 1 and 10.")]
        public int Point { get; set; }

        [Required(ErrorMessage = "At least one answer is required.")]
        [MinLength(1, ErrorMessage = "At least one answer must be provided.")]
        [MaxLength(10, ErrorMessage = "A maximum of 10 answers is allowed.")]
        public List<AnswerCreateRequestDto> Answers { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Answers == null || !Answers.Any())
            {
                yield return new ValidationResult(
                    "At least one answer is required.",
                    new[] { nameof(Answers) });
                yield break;
            }

            var correctCount = Answers.Count(a => a.IsCorrect);

            if (correctCount != 1)
            {
                yield return new ValidationResult(
                    "Exactly one correct answer is required.",
                    new[] { nameof(Answers) });
            }
        }
    }
}
