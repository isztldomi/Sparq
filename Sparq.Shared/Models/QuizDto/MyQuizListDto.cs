using Sparq.Shared.Models.SnapshotDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.QuizDto
{
    public class MyQuizListDto
    {
        public string Id { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public MyQuizSnapshotListDto? LastSnapshot { get; set; }
    }
}
