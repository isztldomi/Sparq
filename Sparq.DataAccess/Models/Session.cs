using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Session
    {
        public int Id { get; set; }

        public int VersionId { get; set; }
        public Version Version { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public int? CurrentQuestionId { get; set; }

        public string PinCode { get; set; }

        public bool IsWaiting { get; set; }
        public bool IsRunning { get; set; }

        public ICollection<Participant> Participants { get; set; }
    }
}
