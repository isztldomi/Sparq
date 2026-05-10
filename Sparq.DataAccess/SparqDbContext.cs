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
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Snapshot> Snapshots { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
        public DbSet<ParticipantAnswer> ParticipantAnswers { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Media> Media { get; set; } = null!;


        public SparqDbContext(DbContextOptions<SparqDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Snapshots)
                .WithOne(s => s.Quiz)
                .HasForeignKey(s => s.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Snapshot>()
                .HasMany(s => s.Questions)
                .WithOne(q => q.Snapshot)
                .HasForeignKey(q => q.SnapshotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
                .Property(s => s.Status)
                .HasConversion<int>()
                .HasDefaultValue(SessionStatus.Created);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Media)
                .WithMany()
                .HasForeignKey(q => q.MediaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
