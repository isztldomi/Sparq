using Microsoft.AspNetCore.SignalR;
using Sparq.Shared.SignalR.HubInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Hubs
{
    public class SessionsHub : Hub<ISessionsClient>
    {
        public async Task JoinSessionGroup(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"session:{sessionId}");
        }
        public async Task LeaveSessionGroup(string sessionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"session:{sessionId}");
        }
    }
}
