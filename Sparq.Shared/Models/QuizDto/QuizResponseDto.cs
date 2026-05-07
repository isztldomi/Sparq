using Sparq.Shared.Models.SnapshotDto;

namespace Sparq.Shared.Models.QuizDto
{
    public class QuizResponseDto
    {
        public int Id { get; set; }
        public bool IsPublic { get; set; }
        public SnapshotResponseDto? LastSnapshot { get; set; }
    }
}
