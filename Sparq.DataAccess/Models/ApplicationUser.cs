using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
    }
}
