using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface IUnitOfWork
    {
        public IUitgeverijRepository Uitgeverijen { get; }
        public IStripRepository Strips { get; }
        public IAuteurRepository Auteurs { get; }
        public IReeksRepository Reeksen { get; }
        public IBestellingRepository Bestellingen { get; }
        public ILeveringRepository Leveringen { get; }

    }
}
