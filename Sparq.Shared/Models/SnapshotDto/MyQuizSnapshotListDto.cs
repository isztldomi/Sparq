using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SnapshotDto
{
    public class MyQuizSnapshotListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
