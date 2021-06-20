using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface ILeveringRepository
    {
        void VoegLeveringToe(Levering levering);
        List<Levering> GeefAlleLeveringen();
        Levering GeefLeveringVoorId(int id);
    }
}
