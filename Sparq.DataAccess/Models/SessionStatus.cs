using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public enum SessionStatus
    {
        Created = 0,
        Waiting = 1,
        Running = 2,
        Finished = 3
    }
}
