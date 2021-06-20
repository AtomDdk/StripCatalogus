using System;
using System.Collections.Generic;
using System.Text;

namespace StripCatalogus___Data_Layer
{
    public class DataLayerException : Exception
    {
        public DataLayerException(string message) : base(message)
        {
        }
    }
}
