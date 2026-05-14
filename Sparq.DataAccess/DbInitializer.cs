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
        public static void Initialize(SparqDbContext context, string imagePath, UserManager<User>? userManager = null)
        {
            if (!Path.Exists(imagePath))
            {
                throw new IOException("Image path does not exists");
            }

            context.Database.Migrate();

            if (context.Users.Any())
            {
                return; // db adatot tartalmaz
            }

            User? adminUser = null;

            if (userManager != null)
            {
                adminUser = SeedUsersAsync(userManager).Result;
            }

            // 1. QUIZ
            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                OwnerId = adminUser?.Id,
                IsPublic = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Set<Quiz>().Add(quiz);
            context.SaveChanges();

            // 2. SNAPSHOT
            var snapshot = new Snapshot
            {
                Id = Guid.NewGuid().ToString(),
                Quiz = quiz,
                QuizId = quiz.Id,
                SnapshotNumber = 1,
                Title = "Alap teszt kvíz",
                Description = "Demo kérdéssor mintaadatokkal",
                TimeLimit = 30,
                PinCode = "1234",
                CreatedAt = DateTime.UtcNow
            };

            context.Set<Snapshot>().Add(snapshot);
            context.SaveChanges();

            // kapcsolás a quizhez
            quiz.LastSnapshot = snapshot;
            quiz.LastSnapshotId = snapshot.Id;
            context.SaveChanges();

            // 4. QUESTIONS
            var question1 = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Snapshot = snapshot,
                SnapshotId = snapshot.Id,
                Order = 1,
                Title = "Alap matematika",
                Text = "Mennyi 2 + 2?",
                Point = 10,
                TimeLimit = 10
            };

            var question2 = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Snapshot = snapshot,
                SnapshotId = snapshot.Id,
                Order = 2,
                Title = "Földrajz",
                Text = "Mi Magyarország fővárosa?",
                Point = 10,
                TimeLimit = 10
            };

            var question3 = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Snapshot = snapshot,
                SnapshotId = snapshot.Id,
                Order = 3,
                Title = "Programozás",
                Text = "Melyik nyelv .NET alapú?",
                Point = 10,
                TimeLimit = 10
            };

            context.Set<Question>().AddRange(question1, question2, question3);

            // 5. ANSWERS

            // Q1
            context.Set<Answer>().AddRange(
                new Answer { Id = Guid.NewGuid().ToString(), Question = question1, QuestionId = question1.Id, Order = 1, Text = "3", IsCorrect = false },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question1, QuestionId = question1.Id, Order = 2, Text = "4", IsCorrect = true },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question1, QuestionId = question1.Id, Order = 3, Text = "5", IsCorrect = false },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question1, QuestionId = question1.Id, Order = 4, Text = "6", IsCorrect = false }
            );

            // Q2
            context.Set<Answer>().AddRange(
                new Answer { Id = Guid.NewGuid().ToString(), Question = question2, QuestionId = question2.Id, Order = 1, Text = "Debrecen", IsCorrect = false },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question2, QuestionId = question2.Id, Order = 2, Text = "Budapest", IsCorrect = true },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question2, QuestionId = question2.Id, Order = 3, Text = "Szeged", IsCorrect = false },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question2, QuestionId = question2.Id, Order = 4, Text = "Pécs", IsCorrect = false }
            );

            // Q3
            context.Set<Answer>().AddRange(
                new Answer { Id = Guid.NewGuid().ToString(), Question = question3, QuestionId = question3.Id, Order = 1, Text = "Python", IsCorrect = false },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question3, QuestionId = question3.Id, Order = 2, Text = "C#", IsCorrect = true },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question3, QuestionId = question3.Id, Order = 3, Text = "HTML", IsCorrect = false },
                new Answer { Id = Guid.NewGuid().ToString(), Question = question3, QuestionId = question3.Id, Order = 4, Text = "CSS", IsCorrect = false }
            );

            // -------------------- SAVE --------------------
            context.SaveChanges();
        }

        private static async Task<User?> SeedUsersAsync(UserManager<User> userManager)
        {
            User? adminUser = null;

            var users = new List<(string Email, string Password, string FirstName, string LastName, string NickName)>
                {
                    ("admin@example.com", "Admin@123", "Test", "Admin", "Test Admin"),
                    ("istvan.kiss@example.com", "Password@123", "István", "Kiss", "István"),
                    ("anna.nagy@example.com", "Password@123", "Anna", "Nagy", "Anna"),
                    ("laszlo.toth@example.com", "Password@123", "László", "Tóth", "László"),
                    ("katalin.szabo@example.com", "Password@123", "Katalin", "Szabó", "Katalin"),
                    ("gergo.horvath@example.com", "Password@123", "Gergő", "Horváth", "Gergő"),
                    ("eszter.farkas@example.com", "Password@123", "Eszter", "Farkas", "Eszter")
                };

            foreach (var (email, password, firstName, lastName, nickName) in users)
            {
                var existingUser = await userManager.FindByEmailAsync(email);

                if (existingUser == null)
                {
                    var user = new User
                    {
                        UserName = email,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        NickName = nickName,
                        RefreshToken = Guid.NewGuid()
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                        );
                    }

                    if (email == "admin@example.com")
                    {
                        adminUser = user;
                    }
                }
                else
                {
                    if (email == "admin@example.com")
                    {
                        adminUser = existingUser;
                    }
                }
            }

            return adminUser;
        }
    }
}
