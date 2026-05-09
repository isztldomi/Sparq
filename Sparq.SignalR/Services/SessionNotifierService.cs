using Microsoft.AspNetCore.SignalR;
using Sparq.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Notifiers
{
    public class SessionNotifierService : ISessionNotifierService
    {
        private readonly IHubContext<SessionHub> _hub;

        public SessionNotifierService(IHubContext<SessionHub> hub)
        {
            _hub = hub;
        }

        public async Task SessionWaitingActivated(string sessionId)
        {
            await _hub.Clients.Group(sessionId)
                .SendAsync("SessionWaitingActivated");
        }
    }
}
