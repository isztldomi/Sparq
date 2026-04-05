using Sparq.Shared.Models.SnapshotDto;
using Sparq.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sparq.Shared.Models.QuizDto
{
    public class QuizCreateRequestDto
    {
        [Required(ErrorMessage = "The visibility (IsPublic) field is required.")]
        public bool IsPublic { get; set; }

        [Required(ErrorMessage = "At least one snapshot is required.")]
        [ExactLength(1, ErrorMessage = "Exactly one snapshot is required.")]
        public List<SnapshotCreateFromQuizRequestDto> Snapshots { get; set; } = new();
    }
}
