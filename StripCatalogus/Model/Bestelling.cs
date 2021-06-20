using Domeinlaag.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Domeinlaag.Model
{
    public class Bestelling
    {
        public Bestelling(int id, Dictionary<Strip, int> stripsEnAantallen, DateTime datum) : this(stripsEnAantallen,datum)
        {
            Id = id;
        }

        public Bestelling(Dictionary<Strip, int> stripsEnAantallen,DateTime datum)
        {
            StripsEnAantallen = stripsEnAantallen;
            Datum = datum;
        }

        public int Id { get; set; }

        private DateTime _Datum;
        public DateTime Datum {
            get { return _Datum; }
            set {
                if (value > DateTime.Now)
                    throw new BestellingException("Een bestelling kan niet in de toekomst plaatsvinden.");
                _Datum = value;
            } 
        }
        private Dictionary<Strip,int> StripsEnAantallen { get; set; }
        public IReadOnlyDictionary<Strip, int> GeefStripsEnAantallen()
        {
            return (IReadOnlyDictionary<Strip, int>)StripsEnAantallen;
        }
        public void StelStripsIn(Dictionary<Strip,int> stripsEnAantallen)
        {
            StripsEnAantallen = stripsEnAantallen;
        }

        public List<Strip> GeefStrips()
        {
            return StripsEnAantallen.Keys.ToList();
        }

        public override bool Equals(object obj)
        {
            if (obj is Bestelling)
            {
                var x = obj as Bestelling;
                return x.Datum == Datum && GeefStripsEnAantallen().SequenceEqual(x.GeefStripsEnAantallen());
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Datum, StripsEnAantallen);
        }

    }
}
