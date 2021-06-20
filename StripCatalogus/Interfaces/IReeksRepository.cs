using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface IReeksRepository
    {
        /// <summary>
        /// Voegt een Reeks toe aan de databank en returnt de Id.
        /// </summary>
        /// <param name="reeks"></param>
        /// <returns></returns>
        int VoegReeksToe(Reeks reeks);
        /// <summary>
        /// Geeft een Reeks aan de hand van een Id. Returnt null indien de Reeks niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Reeks GeefReeksOpId(int id);
        /// <summary>
        /// Geeft een Reeks op de exacte naam. Returnt null indien de Reeks niet bestaat.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        Reeks GeefReeksViaNaam(string naam);
        /// <summary>
        /// Geeft een IEnumerable van all reeksen.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Reeks> GeefAlleReeksen();
        /// <summary>
        /// Geeft een IEnumerable van reeksen aan de hand van een deel van de naam.
        /// </summary>
        /// <param name="deelVanReeksNaam"></param>
        /// <returns></returns>
        //IEnumerable<Reeks> ZoekReeksenOpDeelVanNaam(string deelVanReeksNaam);
        /// <summary>
        /// Geeft een IEnumerable van de nummers van de meegegeven Reeks. Zoekt aan de hand van de Id.
        /// </summary>
        /// <param name="reeks"></param>
        /// <returns></returns>
        IEnumerable<int> GeefAlleNummersVanReeks(Reeks reeks);
        /// <summary>
        /// Update de meegegeven reeks aan de hand van de id.
        /// </summary>
        /// <param name="reeks"></param>
        void UpdateReeks(Reeks reeks);
    }
}
