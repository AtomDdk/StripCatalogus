using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Exceptions
{
    public class DomeinException : Exception
    {
        public DomeinException(string message) : base(message)
        {

        }
    }
}
