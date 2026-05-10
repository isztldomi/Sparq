using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public enum SessionStatus
    {
        Created = 0,
        Waiting = 1,
        Running = 2,
        Finished = 3
    }
}
