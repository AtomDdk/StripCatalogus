using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Exceptions
{
    public class LeveringException : DomeinException
    {
        public LeveringException(string message) : base(message)
        {

        }
    }
}
