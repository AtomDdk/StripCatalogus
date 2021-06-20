using Domeinlaag.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domeinlaag.Model
{
    public class Levering
    {
        public Levering(int id,Dictionary<Strip,int> stripsEnAantallen,DateTime bestelDatum,DateTime leverDatum) : this(stripsEnAantallen,bestelDatum,leverDatum)
        {
            Id = id;
        }
        public Levering(Dictionary<Strip, int> stripsEnAantallen, DateTime bestelDatum, DateTime leverDatum)
        {
            StelStripsEnAantallenIn(stripsEnAantallen);
            StelLeverEnBestelDatumIn(bestelDatum, leverDatum);
        }
        public void StelStripsEnAantallenIn(Dictionary<Strip, int> stripsEnAantallen)
        {
            StripsEnAantallen = stripsEnAantallen;
        }
        public void StelLeverEnBestelDatumIn(DateTime bestelDatum,DateTime leverDatum)
        {
            if (leverDatum > DateTime.Now)
                throw new LeveringException("Een levering kan niet plaatsvinden in de toekomst.");
            if (bestelDatum > leverDatum)
                throw new LeveringException("De bestelDatum kan niet vroeger zijn dan de leverDatum.");
            BestelDatum = bestelDatum;
            LeverDatum = leverDatum;
        }
        public List<Strip> GeefStrips()
        {
            return StripsEnAantallen.Keys.ToList();

        }
        public IReadOnlyDictionary<Strip, int> GeefStripsEnAantallen()
        {
            return (IReadOnlyDictionary<Strip,int>)StripsEnAantallen;
        }
        public int Id { get; set; }
        public DateTime LeverDatum { get; private set; }
        public DateTime BestelDatum { get; private set; }
        private Dictionary<Strip,int> StripsEnAantallen { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Levering)
            {
                var x = obj as Levering;
                return x.BestelDatum == BestelDatum &&LeverDatum==x.LeverDatum && GeefStripsEnAantallen().SequenceEqual(x.GeefStripsEnAantallen());
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, LeverDatum,BestelDatum, StripsEnAantallen);
        }

    }
}
