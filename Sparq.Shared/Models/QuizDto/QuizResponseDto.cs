using Sparq.Shared.Models.SnapshotDto;

namespace Sparq.Shared.Models.QuizDto
{
    public class QuizResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public SnapshotResponseDto? LastSnapshot { get; set; }
    }
}
