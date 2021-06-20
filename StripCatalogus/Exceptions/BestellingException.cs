using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Exceptions
{
    public class BestellingException : DomeinException
    {
        public BestellingException(string message) : base(message)
        {

        }
    }
}
