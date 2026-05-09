using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Notifiers
{
    public interface ISessionNotifier
    {
        Task SessionWaitingActivated(string sessionId);
    }
}
