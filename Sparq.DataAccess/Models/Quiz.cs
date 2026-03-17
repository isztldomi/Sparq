using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public bool IsPublic { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Version> Versions { get; set; }
    }
}
