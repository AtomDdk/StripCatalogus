using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface IAuteurRepository
    {
        /// <summary>
        /// Voegt een auteur toe in de databank en returnt de Id.
        /// </summary>
        /// <param name="auteur"></param>
        /// <returns></returns>
        int VoegAuteurToe(Auteur auteur);
        /// <summary>
        /// Geeft een IEnumerable van auteurs terug op basis van een deel van de naam.
        /// </summary>
        /// <param name="deelVanAuteurNaam"></param>
        /// <returns></returns>
        //IEnumerable<Auteur> ZoekAuteursOpDeelVanNaam(string deelVanAuteurNaam);
        /// <summary>
        /// Geeft een IEnumerable van alle auteurs uit de databank.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Auteur> GeefAlleAuteurs();
        Auteur GeefAuteurViaId(int id);
        /// <summary>
        /// Geeft een auteur terug aan de hand van de exacte naam. Returnt null indien de Auteur niet bestaat.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        Auteur GeefAuteurViaNaam(string naam);
        /// <summary>
        /// Update de meegegeven auteur aan de hand van zijn id.
        /// </summary>
        /// <param name="auteur"></param>
        void UpdateAuteur(Auteur auteur);
    }
}
