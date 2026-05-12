using Microsoft.AspNetCore.SignalR;
using Sparq.Shared.SignalR.HubInterfaces;
using Sparq.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Services
{
    internal class SessionsNotificationService : ISessionsNotificationService
    {
        private readonly IHubContext<SessionsHub, ISessionsClient> _hubContext;

        public SessionsNotificationService(IHubContext<SessionsHub, ISessionsClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifySessionParticipantsUpdatedAsync(string sessionId)
        {
            await _hubContext.Clients
                .Group($"session:{sessionId}")
                .SessionParticipantsUpdated(sessionId);
        }

        public async Task NotifySessionDeactivatedAsync(string sessionId)
        {
            await _hubContext.Clients
                .Group($"session:{sessionId}")
                .SessionDeactivated(sessionId);
        }

        public Task NotifyOnClientDisconnectionAsync(string connectionId)
        {
            return Task.CompletedTask;
        }
    }
}
