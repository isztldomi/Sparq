using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string name) : base($"{name} entity not found.") { }
    }
}
