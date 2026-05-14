using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Services
{
    public interface ISessionsNotificationService
    {
        Task NotifySessionParticipantsUpdatedAsync(string sessionId);
        Task NotifySessionDeactivatedAsync(string sessionId);
        Task NotifyOnClientDisconnectionAsync(string connectionId);
        Task NotifySessionStart(string sessionId);
        Task NotifySessionEnd(string sessionId);
        Task NotifySessionNextQuestion(string sessionId);
    }
}
