using Sparq.Shared.Models.SnapshotDto;
using System.ComponentModel.DataAnnotations;

namespace Sparq.Shared.Models.QuizDto
{
    public class QuizCreateRequestDto
    {
        [Required(ErrorMessage = "The visibility (IsPublic) field is required.")]
        public bool IsPublic { get; set; }

        [Required(ErrorMessage = "At least one snapshot is required.")]
        [MinLength(1, ErrorMessage = "At least one snapshot must be provided.")]
        public List<SnapshotCreateRequestDto> Snapshots { get; set; } = new();
    }
}
