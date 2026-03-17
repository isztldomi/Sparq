using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    public class SparqDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Models.Version> QuizVersions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> AnswerOptions { get; set; }
        public DbSet<Session> QuizSessions { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantAnswer> ParticipantAnswers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public SparqDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
