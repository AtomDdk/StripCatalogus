using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface IStripRepository
    {
        /// <summary>
        /// Voegt een Strip toe an de databank en returnt een Id.
        /// </summary>
        /// <param name="strip"></param>
        /// <returns></returns>
        int VoegStripToe(Strip strip);
        /// <summary>
        /// Returnt een IEnumerable van alle strips in de databank.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Strip> GeefAlleStrips();
        /// <summary>
        /// Returnt een strip aan de hand van de meegegeven Id. Returnt null indien de Strip niet bestaat.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Strip GeefStripViaId(int id);
        /// <summary>
        /// Update de meegegeven strip aan de hand van de Id.
        /// </summary>
        /// <param name="strip"></param>
        void UpdateStrip(Strip strip);
    }
}
