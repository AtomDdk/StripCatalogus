using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface IBestellingRepository
    {
        void VoegBestellingToe(Bestelling bestelling);
        List<Bestelling> GeefAlleBestellingen();
        Bestelling GeefBestellingVoorId(int id);
    }
}
