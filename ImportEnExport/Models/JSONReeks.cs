using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ImportEnExport.Models
{
    public class JSONReeks : IComparable,IComparable<JSONReeks>
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        //public List<object> Strips { get; set; }

        public int CompareTo([AllowNull] JSONReeks other)
        {
            return ID.CompareTo(other.ID);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as JSONReeks);
        }

        public override bool Equals(object obj)
        {
            return obj is JSONReeks json &&
                   ID == json.ID &&
                   Naam == json.Naam;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Naam);
        }
        public override string ToString()
        {
            return $"Reeks: [{ID}, {Naam}] ";
        }
    }
}
