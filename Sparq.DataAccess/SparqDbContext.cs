using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    public class SparqDbContext : IdentityDbContext<User, UserRole, string>
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantAnswer> ParticipantAnswers { get; set; }
        public DbSet<Message> Messages { get; set; }


        public SparqDbContext(DbContextOptions<SparqDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Snapshot → Quiz (normál)
            modelBuilder.Entity<Snapshot>()
                .HasOne(s => s.Quiz)
                .WithMany(q => q.Snapshots)
                .HasForeignKey(s => s.QuizId);

            // Quiz → LastSnapshot (speciális)
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.LastSnapshot)
                .WithMany() // nincs visszanavigáció
                .HasForeignKey(q => q.LastSnapshotId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
