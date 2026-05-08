using Sparq.Shared.Models.QuestionDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SnapshotDto
{
    public class SnapshotResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TimeLimit { get; set; }
        public string PinCode { get; set; } = string.Empty;
        public List<QuestionResponseDto> Questions { get; set; } = new();
    }
}
