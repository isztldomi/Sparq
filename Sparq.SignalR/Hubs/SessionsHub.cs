using Microsoft.AspNetCore.SignalR;
using Sparq.Shared.SignalR.HubInterfaces;
using Sparq.SignalR.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Hubs
{
    public class SessionsHub : Hub<ISessionsClient>
    {
        private readonly ISessionsNotificationService _sessionsNotificationService;

        public SessionsHub(ISessionsNotificationService sessionsNotificationService)
        {
            _sessionsNotificationService = sessionsNotificationService;
        }

        public async Task JoinSessionGroup(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"session:{sessionId}");

            await _sessionsNotificationService.NotifySessionParticipantsUpdatedAsync(sessionId);
        }

        public async Task LeaveSessionGroup(string sessionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"session:{sessionId}");

            await _sessionsNotificationService.NotifySessionParticipantsUpdatedAsync(sessionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
