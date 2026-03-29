using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public Guid? RefreshToken { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
    }
}
