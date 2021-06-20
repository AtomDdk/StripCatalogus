using Domeinlaag.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Model
{
    public class Uitgeverij
    {
        public Uitgeverij(string naam)
        {
            Naam = naam;
        }

        public Uitgeverij(int id, string naam) : this(naam)
        {
            Id = id;
        }
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (value < 0)
                    throw new UitgeverijException("Het Id van de Strip moet groter dan 0 zijn.");
                _Id = value;
            }
        }
        private string _Naam;
        public string Naam
        {
            get { return _Naam; }
            set
            {
                if (value == null)
                    throw new UitgeverijException("Naam mag niet null zijn");
                else if (value == "")
                    throw new UitgeverijException("Naam mag niet leeg zijn.");
                _Naam = value;
            }
        }
        public override string ToString()
        {
            return $"{Id}, {Naam}";
        }
        public override bool Equals(object obj)
        {
            if (obj is Uitgeverij)
            {
                var x = obj as Uitgeverij;
                return Naam.Equals(x.Naam)&&Id.Equals(x.Id);
            }
            else return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(_Id, Id, _Naam, Naam);
        }
    }
}
