using Domeinlaag.Exceptions;
using System;

namespace Domeinlaag.Model
{
    public class Reeks
    {
        public Reeks(string naam)
        {
            Naam = naam;
        }

        public Reeks(int id, string naam) : this(naam)
        {
            Id = id;
        }
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (value < 0) throw new ReeksException("De id van de Reeks moet groter dan 0 zijn.");
                else _Id = value;
            }
        }
        private string _Naam;
        public string Naam
        {
            get { return _Naam; }
            set
            {
                if (value == null) throw new ReeksException("De Naam van de reeks mag niet null zijn.");
                else if (value == "") throw new ReeksException("De Naam van de reeks mag niet leeg zijn.");
                else _Naam = value;
            }
        }
        public override bool Equals(object obj)
        {
            return obj is Reeks reeks &&
                   Id == reeks.Id &&
                   Naam == reeks.Naam;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Naam);
        }

        public override string ToString()
        {
            return $"Reeks: [ {Id}, {Naam}] ";
        }
    }
}
