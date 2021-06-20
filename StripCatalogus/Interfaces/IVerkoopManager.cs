using Domeinlaag.Model;
using System;
using System.Collections.Generic;

namespace Domeinlaag.Interfaces
{
    public interface IVerkoopManager
    {
        List<Bestelling> GeefAlleBestellingen();
        List<Levering> GeefAlleLeveringen();
        List<Strip> GeefAlleStrips();
        List<Strip> GeefAlleStripsVoorVerkoop();
        Bestelling GeefBestellingVoorId(int id);
        Levering GeefLeveringVoorId(int id);
        void VoegBestellingToe(Dictionary<Strip, int> stripsEnAantallen);
        void VoegLeveringToe(Dictionary<Strip, int> stripsEnAantallen, DateTime bestelDatum, DateTime leverDatum);
        List<Strip> ZoekStrips(ZoekStripArguments args);
        List<Strip> ZoekStripsVoorVerkoop(ZoekStripArguments args);
    }
}