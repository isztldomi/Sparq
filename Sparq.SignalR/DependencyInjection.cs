using Microsoft.Extensions.DependencyInjection;
using Sparq.SignalR.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
        {
            services.AddSingleton<ISessionsNotificationService, SessionsNotificationService>();
            return services;
        }
    }
}
