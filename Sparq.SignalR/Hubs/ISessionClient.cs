using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.SignalR.Hubs
{
    public interface ISessionClient
    {
        Task UserJoined(string connectionId);
        Task UserLeft(string connectionId);
    }
}
