using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class UserRole : IdentityRole
    {
        public UserRole() { }
        public UserRole(string role) : base(role) { }
    }
}
