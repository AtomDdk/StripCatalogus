using System;
using System.Collections.Generic;
using System.Text;

namespace ImportEnExport.Models
{
    public class JSONAuteur
    {
        public int ID { get; set; }
        public string Naam { get; set; }

        public override bool Equals(object obj)
        {
            return obj is JSONAuteur json &&
                   ID == json.ID &&
                   Naam == json.Naam;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Naam);
        }
        public override string ToString()
        {
            return $"Auteur: [{ID}, {Naam}] ";
        }
    }
}
