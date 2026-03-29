using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public required string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public required User Owner { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
    }
}
