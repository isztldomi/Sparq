using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.SignalR.HubInterfaces
{
    public interface ISessionsClient
    {
        Task SessionParticipantsUpdated(string sessionId);
        Task SessionDeactivated(string sessionId);
        Task SessionStart(string sessionId);
        Task SessionEnd(string sessionId);
        Task SessionNextQuestion(string sessionId);
    }
}
