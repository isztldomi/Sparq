using System.ComponentModel.DataAnnotations;

namespace Sparq.Shared.Models.AnswerDto
{
    public class AnswerCreateRequestDto
    {
        [Required(ErrorMessage = "Answer text is required.")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Answer text must be between 1 and 1000 characters.")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "The IsCorrect field is required.")]
        public bool IsCorrect { get; set; }
        [Required(ErrorMessage = "The Order field is required.")]
        public int Order { get; set; }
    }
}
