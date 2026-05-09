using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Notifiers
{
    public interface ISessionNotifierService
    {
        Task SessionWaitingActivated(string sessionId);
    }
}
