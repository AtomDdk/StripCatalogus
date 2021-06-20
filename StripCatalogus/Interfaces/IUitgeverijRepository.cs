using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface IUitgeverijRepository
    {
        /// <summary>
        /// Voegt een Uitgeverij toe aan de database en returnt de Id.
        /// </summary>
        /// <param name="uitgeverij"></param>
        /// <returns></returns>
        int VoegUitgeverijToe(Uitgeverij uitgeverij);
        /// <summary>
        /// Returnt een IEnumerable van uitgeverijen aan de hand van een deel van de naam.
        /// </summary>
        /// <param name="deelVanUitgeverijNaam"></param>
        /// <returns></returns>
        //IEnumerable<Uitgeverij> ZoekUitgeverijenOpDeelVanNaam(string deelVanUitgeverijNaam);
        /// <summary>
        /// Returnt een IEnumerable van alle uitgeverijen.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Uitgeverij> GeefAlleUitgeverijen();
        /// <summary>
        /// Returnt een Uitgeverij aan de hand van de meegegeven Id. Returnt null indien de Uigeverij niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Uitgeverij GeefUitgeverijViaId(int id);
        /// <summary>
        /// Returnt een Uitgeverij aan de hand van de exacte naam. Returnt null indien de Uitgeverij niet bestaat.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        Uitgeverij GeefUitgeverijViaNaam(string naam);
        /// <summary>
        /// Update de meegegeven uitgeverij aan de hand van zijn id.
        /// </summary>
        /// <param name="uitgeverij"></param>
        void UpdateUitgeverij(Uitgeverij uitgeverij);
    }
}
