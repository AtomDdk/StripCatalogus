using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domeinlaag
{
    public class CatalogusManager : ICatalogusManager
    {
        private IUnitOfWork _databaseManager;
        public CatalogusManager(IUnitOfWork databaseManager)
        {
            _databaseManager = databaseManager;
        }

        #region Controle op beschikbaarheid

        /// <summary>
        /// Controleert of de strip kan worden aangepast/toegevoegd aan de databank.
        /// </summary>
        /// <param name="reeks"></param>
        /// <param name="reeksNummer"></param>
        /// <returns></returns>
        public bool IsStripBeschikbaar(Strip strip)
        {
            if (strip.Reeks == null)
            {
                if (strip.ReeksNummer == null)
                {
                    //dit stuk nog moeten aanpassen, we gaan hier nog wat prutsen waarschijnlijk
                    ZoekStripArguments args = new ZoekStripArguments();
                    args.Titel = strip.Titel;
                    args.Reeks = null;
                    List<Strip> vergelijkingStrips = ZoekStrips(args);
                    var stripAuteurs = strip.GeefAuteurs();
                    foreach (Strip s in vergelijkingStrips)
                    {
                        if (strip.Titel == s.Titel && stripAuteurs.SequenceEqual(s.GeefAuteurs()) && strip.Uitgeverij.Equals(s.Uitgeverij))
                            return false;
                    }
                    return true;
                }
                else return false;
            }
            else if (strip.ReeksNummer == null)
            {
                return false;
            }
            else
            {
                List<int> nummers = _databaseManager.Reeksen.GeefAlleNummersVanReeks(strip.Reeks).ToList();
                if (nummers.Contains((int)strip.ReeksNummer))
                    return false;
                else return true;
            }

        }
        private bool IsAuteurBeschikbaar(Auteur auteur)
        {
            var x = _databaseManager.Auteurs.GeefAuteurViaNaam(auteur.Naam);
            if (x == null)
                return true;
            else return false;
        }
        private bool IsUitgeverijBeschikbaar(Uitgeverij uitgeverij)
        {
            var x = _databaseManager.Uitgeverijen.GeefUitgeverijViaNaam(uitgeverij.Naam);
            if (x == null)
                return true;
            else return false;
        }
        private bool IsReeksBeschikbaar(Reeks reeks)
        {
            var x = _databaseManager.Reeksen.GeefReeksViaNaam(reeks.Naam);
            if (x == null)
                return true;
            else return false;
        }

        private bool ControleerAuteursVoorStrip(Strip strip)
        {
            foreach (Auteur auteur in strip.GeefAuteurs())
            {
                Auteur controleAuteur = GeefAuteurViaId(auteur.Id);
                if (controleAuteur == null)
                    throw new CatalogusException($"Een auteur met Id {auteur.Id} bevindt zich niet in de databank.");
                if (!controleAuteur.Equals(auteur))
                    throw new CatalogusException($"De entry van de auteur met id {auteur.Id} kwam niet overeen met de entry in de databank.");
            }
            return true;
        }

        #endregion


        #region VoegToe methodes

        /// <summary>
        /// Voegt een strip toe, elk onderdeel moet al in de databank zitten. Gooit ArgumentExceptions als de arguments niet aan de businessregels voldoen.
        /// </summary>
        /// <param name="titel"></param>
        /// De Titel van de strip
        /// <param name="reeks"></param>
        /// De Reeks waartoe de strip toe behoort. Deze mag null zijn.
        /// <param name="reeksNummer"></param>
        /// Het reeksnummer, dit mag enkel null zijn als het tot "geen reeks" behoort
        /// /// <param name="auteurs"></param>
        /// De lijst met auteurs, deze mag geen dubbels bevatten en moeten allemaal al in de databank zitten, deze moet minstens 1 element bevatten.
        /// <param name="uitgeverij"></param>
        /// De Uitgeverij, deze moet al in de databank zitten en mag niet null zijn.
        /// <returns></returns>
        public Strip VoegStripToe(string titel, Reeks reeks, int? reeksNummer, List<Auteur> auteurs, Uitgeverij uitgeverij)
        {
            Strip strip = new Strip(titel, reeks, reeksNummer, auteurs, uitgeverij);
            Uitgeverij controleUitgeverij = GeefUitgeverijViaId(uitgeverij.Id);
            if (controleUitgeverij == null)
                throw new CatalogusException($"Een uitgeverij met id { uitgeverij.Id } bevindt zich niet in de databank.");
            if (!controleUitgeverij.Equals(uitgeverij))
                throw new CatalogusException($"De entry van de uitgeverij met id {uitgeverij.Id} kwam niet overeen met de entry in de databank.");
            ControleerAuteursVoorStrip(strip);
            if (reeks != null)
            {

                Reeks controleReeks = GeefReeksViaId(reeks.Id);
                if (controleReeks == null)
                    throw new CatalogusException($"Een reeks met id {reeks.Id} bevindt zich niet in de databank.");
                if (!controleReeks.Equals(reeks))
                    throw new CatalogusException($"De meegeven reeks kwam niet overeen met de entry van die reeks in de databank.");
            }
            if (!IsStripBeschikbaar(strip))
                throw new CatalogusException("Dit nummer van deze reeks bestaat al in de databank.");
            strip.Id = _databaseManager.Strips.VoegStripToe(strip);
            return strip;
        }

        /// <summary>
        /// Voegt een Auteur toe aan de databank. De naam moet uniek zijn.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        public Auteur VoegAuteurToe(string naam)
        {
            Auteur auteur = new Auteur(naam);
            if (IsAuteurBeschikbaar(auteur))
            {
                auteur.Id = _databaseManager.Auteurs.VoegAuteurToe(auteur);
                return auteur;
            }
            else throw new CatalogusException("Deze Auteursnaam bestaat al in de databank.");
        }

        /// <summary>
        /// Voegt een Uitgeverij toe aan de databank met de gegeven naam.
        /// </summary>
        /// <param name="naam"></param>
        /// <returns></returns>
        public Uitgeverij VoegUitgeverijToe(string naam)
        {
            Uitgeverij uitgeverij = new Uitgeverij(naam);
            if (IsUitgeverijBeschikbaar(uitgeverij))
            {
                uitgeverij.Id = _databaseManager.Uitgeverijen.VoegUitgeverijToe(uitgeverij);
                return uitgeverij;
            }
            else throw new CatalogusException("Een uitgeverij met deze naam bestaat al in de databank.");
        }

        /// <summary>
        /// Voegt een reeks toe met de gegeven reeksNaam.
        /// </summary>
        /// <param name="reeksNaam"></param>
        /// <returns></returns>
        public Reeks VoegReeksToe(string reeksNaam)
        {
            Reeks reeks = new Reeks(reeksNaam);
            if (IsReeksBeschikbaar(reeks))
            {
                reeks.Id = _databaseManager.Reeksen.VoegReeksToe(reeks);
                return reeks;
            }
            else throw new CatalogusException("Een reeks met deze naam bestaat al in de databank.");
        }


        #endregion


        #region GeefViaIdMethodes
        /// <summary>
        /// returnt een strip op basis van de Id. Returnt null als deze niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Strip GeefStripViaId(int id)
        {
            return _databaseManager.Strips.GeefStripViaId(id);
        }

        /// <summary>
        /// returnt een Auteur op basis van de Id. Returnt null als deze niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Auteur GeefAuteurViaId(int id)
        {
            return _databaseManager.Auteurs.GeefAuteurViaId(id);
        }

        /// <summary>
        /// Geeft de Uitgeverij in de databank voor de gegeven Id. Returnt null als deze niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Uitgeverij GeefUitgeverijViaId(int id)
        {
            return _databaseManager.Uitgeverijen.GeefUitgeverijViaId(id);
        }

        /// <summary>
        /// Geeft de reeks in de databank voor de gegeven Id. Returnt null als deze niet bestaat.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Reeks GeefReeksViaId(int id)
        {
            return _databaseManager.Reeksen.GeefReeksOpId(id);
        }


        #endregion





        /// <summary>
        /// Past de gegeven Strip aan, de Id moet hetzelfde blijven.
        /// </summary>
        /// <param name="strip"></param>
        public void UpdateStrip(Strip strip)
        {
            ControleerAuteursVoorStrip(strip);
            Strip oudeStrip = GeefStripViaId(strip.Id);
            Uitgeverij uitgeverij = GeefUitgeverijViaId(strip.Uitgeverij.Id);
            if (uitgeverij == null)
                throw new CatalogusException("Deze uitgeverij bestaat niet.");
            else if (!uitgeverij.Equals(strip.Uitgeverij))
                throw new CatalogusException("De uitgeverij werd aangepast.");
            if (oudeStrip.Equals(strip))
                throw new CatalogusException("Er is niks veranderd.");
            else
            {
                if (oudeStrip.Reeks == null)
                {
                    if (IsStripBeschikbaar(strip))
                        _databaseManager.Strips.UpdateStrip(strip);
                    else throw new CatalogusException("Deze combinatie reeks en reeksnummer is al in gebruik.");
                }
                else if (oudeStrip.Reeks.Equals(strip.Reeks) && oudeStrip.ReeksNummer == strip.ReeksNummer)
                    _databaseManager.Strips.UpdateStrip(strip);
                else
                {
                    if (IsStripBeschikbaar(strip))
                        _databaseManager.Strips.UpdateStrip(strip);
                    else throw new CatalogusException("Deze combinatie reeks en reeksnummer is al in gebruik.");
                }
            }
        }
        /// <summary>
        /// Past de gegeven Reeks aan, de Id moet hetzelfde blijven.
        /// </summary>
        /// <param name="reeks"></param>
        public void UpdateReeks(Reeks reeks)
        {
            Reeks oudeReeks = GeefReeksViaId(reeks.Id);
            if (oudeReeks == null)
                throw new CatalogusException("Deze uitgeverij bestaat niet.");
            else if (oudeReeks.Equals(reeks))
                throw new CatalogusException("Er is niks veranderd.");
            else if (IsReeksBeschikbaar(reeks))
                _databaseManager.Reeksen.UpdateReeks(reeks);
            else throw new CatalogusException("Deze naam bestaat al.");
        }
        /// <summary>
        /// Past de gegeven Auteur aan, de Id moet hetzelfde blijven.
        /// </summary>
        /// <param name="auteur"></param>
        public void UpdateAuteur(Auteur auteur)
        {
            Auteur oudeAuteur = GeefAuteurViaId(auteur.Id);
            if (oudeAuteur == null)
                throw new CatalogusException("Deze auteur bestaat niet.");
            else if (oudeAuteur.Equals(auteur))
                throw new CatalogusException("Er is niks veranderd.");
            else if (IsAuteurBeschikbaar(auteur))
                _databaseManager.Auteurs.UpdateAuteur(auteur);
            else throw new CatalogusException("Deze naam bestaat al in de databank.");
        }
        /// <summary>
        /// Past de gegeven Uitgeverij aan, de Id moet hetzelfde blijven.
        /// </summary>
        /// <param name="uitgeverij"></param>
        public void UpdateUitgeverij(Uitgeverij uitgeverij)
        {
            Uitgeverij oudeUitgeverij = GeefUitgeverijViaId(uitgeverij.Id);
            if (oudeUitgeverij == null)
                throw new CatalogusException("Deze uitgeverij bestaat niet.");
            else if (oudeUitgeverij.Equals(uitgeverij))
                throw new CatalogusException("Er is niks veranderd.");
            else if (IsUitgeverijBeschikbaar(uitgeverij))
                _databaseManager.Uitgeverijen.UpdateUitgeverij(uitgeverij);
            else throw new CatalogusException("Deze naam bestaat al.");
        }


        /// <summary>
        /// Returnt alle auteurs in de databank in een lijst, ongesorteerd.
        /// </summary>
        /// <returns></returns>
        public List<Auteur> GeefAlleAuteurs()
        {
            return _databaseManager.Auteurs.GeefAlleAuteurs().ToList();
        }

        /// <summary>
        /// Returnt alle Uitgeverijen in de databank in een lijst, ongesorteerd.
        /// </summary>
        /// <returns></returns>
        public List<Uitgeverij> GeefAlleUitgeverijen()
        {
            return _databaseManager.Uitgeverijen.GeefAlleUitgeverijen().ToList();
        }
        /// <summary>
        /// Returnt alle Reeksen in de databank in een lijst, ongesorteerd.
        /// </summary>
        /// <returns></returns>
        public List<Reeks> GeefAlleReeksen()
        {
            return _databaseManager.Reeksen.GeefAlleReeksen().ToList();
        }
        /// <summary>
        /// Returnt alle Strips in de databank in een lijst, ongesorteerd.
        /// </summary>
        /// <returns></returns>
        public List<Strip> GeefAlleStrips()
        {
            var result = _databaseManager.Strips.GeefAlleStrips();
            if (result == null)
                return new List<Strip>();
            else return result.ToList();
        }
        /// <summary>
        /// Returnt alle Strips in de databank in een lijst, ongesorteerd en gefilterd volgens de vershillende ZoekStripArguments.
        /// </summary>
        /// <param name="args">Stel de parameters in waarop gefilterd moet worden.</param>
        /// <returns></returns>
        public List<Strip> ZoekStrips(ZoekStripArguments args)
        {
            IEnumerable<Strip> strips = GeefAlleStrips();
            if (args.FilterOpAuteur)
                strips = strips.Where(s => s.GeefAuteurs().Contains(args.Auteur));
            if (args.FilterOpReeks)
            {
                if (args.Reeks == null)
                    strips = strips.Where(s => s.Reeks == null);
                else
                {
                    //deze lijn code geeft nullreference exceptions als een strip null als reeks heeft dus !=null moeten toevoegen.
                    strips = strips.Where(s => s.Reeks!=null && s.Reeks.Equals(args.Reeks));
                }
            }
            if (args.FilterOpReeksNummer)
                    strips = strips.Where(s => s.ReeksNummer == args.ReeksNummer);
            if (args.FilterOpTitel)
            {
                string titel = args.Titel;
                //while(titel.Contains(" "))
                //{
                //    titel.Replace("  ", "");
                //}
                titel = titel.Trim();
                titel = titel.ToLower();
                strips = strips.Where(s => s.Titel.ToLower().Contains(titel));
            }
            if (args.FilterOpUitgeverij)
                strips = strips.Where(s => s.Uitgeverij.Equals(args.Uitgeverij));
            return strips.ToList();
        }
    }
}
