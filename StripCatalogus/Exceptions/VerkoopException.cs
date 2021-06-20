using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Exceptions
{
    public class VerkoopException : DomeinException
    {
        public VerkoopException(string message) : base(message)
        {

        }
    }
}
