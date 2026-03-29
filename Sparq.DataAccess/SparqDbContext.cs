using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    public class SparqDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
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
    }
}
