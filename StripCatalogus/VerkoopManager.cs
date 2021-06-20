using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domeinlaag
{
    public class VerkoopManager : IVerkoopManager
    {
        private IUnitOfWork Uow;
        private CatalogusManager Catalogus;
        public VerkoopManager(IUnitOfWork uow)
        {
            Uow = uow;
            Catalogus = new CatalogusManager(Uow);
        }
        private bool ControleerBeschikbaarheidBestelling(Dictionary<Strip, int> stripsEnAantallen)
        {
            bool result = true;
            foreach (KeyValuePair<Strip, int> pair in stripsEnAantallen)
            {
                if (pair.Key.Aantal < pair.Value)
                    result = false;
            }
            return result;
        }
        public void VoegBestellingToe(Dictionary<Strip, int> stripsEnAantallen)
        {
            bool beschikbaar = ControleerBeschikbaarheidBestelling(stripsEnAantallen);

            if (beschikbaar)
            {
                Bestelling bestelling = new Bestelling(stripsEnAantallen, DateTime.Now);
                foreach(KeyValuePair<Strip,int> pair in bestelling.GeefStripsEnAantallen())
                {
                    pair.Key.Aantal -= pair.Value;
                }
                Uow.Bestellingen.VoegBestellingToe(bestelling);
            }
            else throw new VerkoopException("Niet alle strips zijn in voorraad.");
        }
        public void VoegLeveringToe(Dictionary<Strip, int> stripsEnAantallen, DateTime bestelDatum, DateTime leverDatum)
        {
            Levering levering = new Levering(stripsEnAantallen, bestelDatum, leverDatum);
            foreach (KeyValuePair < Strip,int> pair in levering.GeefStripsEnAantallen())
            {
                pair.Key.Aantal += pair.Value;
            }
            Uow.Leveringen.VoegLeveringToe(levering);
        }
        public List<Bestelling> GeefAlleBestellingen()
        {
            return Uow.Bestellingen.GeefAlleBestellingen();
        }
        public Bestelling GeefBestellingVoorId(int id)
        
        {
            if (id >= 0)
                return Uow.Bestellingen.GeefBestellingVoorId(id);
            else throw new VerkoopException("Id kan niet kleiner zijn dan 0");
        }
        public List<Levering> GeefAlleLeveringen()
        {
            return Uow.Leveringen.GeefAlleLeveringen();
        }
        public Levering GeefLeveringVoorId(int id)
        {

            if (id >= 0)
                return Uow.Leveringen.GeefLeveringVoorId(id);
            else throw new VerkoopException("Id kan niet kleiner zijn dan 0");
        }
        public List<Strip> GeefAlleStrips()
        {
            return Catalogus.GeefAlleStrips();
            //mogenlijks enkel diegene returnen die een aantal groter dan 0 hebben???
        }
        public List<Strip> GeefAlleStripsVoorVerkoop()
        {
            var temp = GeefAlleStrips();
            return temp.Where(s => s.Aantal > 0).ToList();
        }
        public List<Strip> ZoekStrips(ZoekStripArguments args)
        {
            return Catalogus.ZoekStrips(args);

        }
        public List<Strip> ZoekStripsVoorVerkoop(ZoekStripArguments args)
        {
            var temp = ZoekStrips(args);
            return temp.Where(s => s.Aantal > 0).ToList();
        }

    }
}
