using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sparq.DataAccess.Config;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
        {
            // Config
            // services.Configure<ReservationSettings>(config.GetSection("ReservationSettings"));
            // services.Configure<EmailSettings>(config.GetSection("EmailSettings"));


            // Database
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<SparqDbContext>(options => options
                .UseNpgsql(connectionString)
                .UseLazyLoadingProxies()
                );

            //Identity
            services.AddIdentity<User, UserRole>(options =>
            {
                // Password settings.
                options.Password.RequiredLength = 6;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<SparqDbContext>()
                .AddDefaultTokenProviders();

            // Services
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IParticipantAnswerService, ParticipantAnswerService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISnapshotService, SnapshotService>();
            services.AddScoped<IUsersService, UsersService>();

            // Add email sending service

            // Add email sending service
            // services.AddSingleton<IEmailsService, SmtpEmailsService>();

            return services;
        }
    }
}
