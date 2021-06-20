using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domeinlaag.Interfaces
{
    public interface ICatalogusManager
    {
        public Strip VoegStripToe(string titel, Reeks reeks, int? reeksNummer, List<Auteur> auteurs, Uitgeverij uitgeverij);
        public Auteur VoegAuteurToe(string naam);
        Uitgeverij VoegUitgeverijToe(string naam);
        void UpdateStrip(Strip strip);
        void UpdateReeks(Reeks reeks);
        void UpdateAuteur(Auteur auteur);
        void UpdateUitgeverij(Uitgeverij uitgeverij);
        public Strip GeefStripViaId(int id);
        public Auteur GeefAuteurViaId(int id);
        public Uitgeverij GeefUitgeverijViaId(int id);
        public List<Reeks> GeefAlleReeksen();
        List<Auteur> GeefAlleAuteurs();
        Reeks VoegReeksToe(string reeksNaam);
        List<Uitgeverij> GeefAlleUitgeverijen();
        List<Strip> GeefAlleStrips();
        List<Strip> ZoekStrips(ZoekStripArguments args);

    }
}


