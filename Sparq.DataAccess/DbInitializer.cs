using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(SparqDbContext context, string imagePath)
        {
            if (!Path.Exists(imagePath))
            {
                throw new IOException("Image path does not exists");
            }

            context.Database.Migrate();

            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            var user = new User
            {
                Id = "teszt_user_id",
                UserName = "Teszt_1",
                Email = "teszt_1@gmail.com",
                FirstName = "Teszt_1",
                LastName = "Teszt_1",
                NickName = "Teszt_1",
                EmailConfirmed = true
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
