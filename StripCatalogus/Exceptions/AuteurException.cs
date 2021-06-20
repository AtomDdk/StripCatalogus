using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Exceptions
{
    public class AuteurException: DomeinException
    {
        public AuteurException(string message) : base(message){

        }
    }
}
