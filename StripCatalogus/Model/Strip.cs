using Domeinlaag.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Domeinlaag.Model
{
    public class Strip
    {
        public Strip(int id, string titel, Reeks reeks, int? reeksNummer, List<Auteur> auteurs, Uitgeverij uitgeverij,int aantal) 
            : this(id, titel, reeks, reeksNummer, auteurs, uitgeverij)
        {
            Aantal = aantal;
        }

        public Strip(string titel, Reeks reeks, int? reeksNummer, List<Auteur> auteurs, Uitgeverij uitgeverij)
        {
            Titel = titel;
            VeranderReeksEnReeksNummer(reeks, reeksNummer);
            StelAuteursIn(auteurs);
            Uitgeverij = uitgeverij;
        }

        public Strip(int id, string titel, Reeks reeks, int? reeksNummer, List<Auteur> auteurs, Uitgeverij uitgeverij) :
            this(titel, reeks, reeksNummer, auteurs, uitgeverij)
        {
            Id = id;
        }
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (value < 0)
                    throw new StripException("Het Id van de Strip moet groter dan 0 zijn.");
                _Id = value;
            }
        }
        private string _Titel;
        public string Titel
        {
            get { return _Titel; }
            set
            {
                if (value == null)
                    throw new StripException("Titel mag niet null zijn");
                if (value == "")
                    throw new StripException("Title mag niet leeg zijn.");
                _Titel = value;
            }
        }

        public void VeranderReeksEnReeksNummer(Reeks reeks,int? reeksNummer)
        {
            if (reeks == null)
            {
                if (reeksNummer == null)
                {
                    ReeksNummer = reeksNummer;
                    Reeks = reeks;
                }
                else throw new StripException("Als een strip geen reeks heeft mag het ook geen reeksnummer hebben.");
            }
            else if (reeksNummer == null)
            {
                throw new StripException("Als een strip geen reeksNummer heeft mag het ook geen reeks hebben.");
            }
            else
            {
                ReeksNummer = reeksNummer;
                Reeks = reeks;
            }
        }

        public Reeks Reeks { get; private set; }
        private int? _ReeksNummer;
        public int? ReeksNummer 
        { get 
            {  return _ReeksNummer; } 
            private set 
            { 
                if (value < 0) 
                    throw new StripException("Het reeksnummer moet groter zijn dan 0.");
                _ReeksNummer = value;
            } 
        }
        private List<Auteur> Auteurs { get; set; }
        public ReadOnlyCollection<Auteur> GeefAuteurs()
        {
            return Auteurs.AsReadOnly();
        }
        public void StelAuteursIn(List<Auteur> toeTeVoegenAuteurs)
        {
            if (toeTeVoegenAuteurs != null && toeTeVoegenAuteurs.Count >= 1)
            {
                List<Auteur> auteurs = new List<Auteur>();
                foreach (Auteur auteur in toeTeVoegenAuteurs)
                {
                    if (auteur.Id <= 0)
                        throw new StripException("De auteur moet een geldige Id hebben voor deze aan een strip kan gekoppeld worden.");
                    if (!auteurs.Contains(auteur))
                        auteurs.Add(auteur);
                    else
                        throw new StripException("De lijst met toe te voegen auteurs bevat dubbels.");
                }
                auteurs.Sort((x, y) => x.Naam.CompareTo(y.Naam));
                Auteurs = auteurs;
            }
            else throw new StripException("Er moet minstens 1 auteur zijn.");
        }
        public void VerwijderAuteur(Auteur auteur)
        {
            if (Auteurs.Count >= 2)
            {
                if (Auteurs.Contains(auteur))
                    Auteurs.Remove(auteur);
                else throw new StripException($"{auteur.Naam} was geen auteur van deze strip.");
            }
            else throw new StripException("Deze strip heeft maar 1 auteur, er kan er geen verwijderd worden");
        }
        public void VoegAuteurToe(Auteur auteur)
        {
            if (Auteurs.Contains(auteur))
                throw new StripException("Deze auteur is al verbonden aan deze strip");
            else if (auteur.Id <= 0) throw new StripException("De Auteur moet een geldige Id hebben voor deze kan gekoppeld worden aan een strip.");
            else Auteurs.Add(auteur);
            Auteurs.Sort((x, y) => x.Naam.CompareTo(y.Naam));
        }
        private Uitgeverij _Uitgeverij;
        public Uitgeverij Uitgeverij
        {
            get { return _Uitgeverij; }
            set
            {
                if (value == null) throw new StripException("Uitgeverij mag niet null zijn");
                else if (value.Id <= 0) throw new StripException("De Uitgeverij moet een geldige id krijgen.");
                else _Uitgeverij = value;
            }
        }

        private int _Aantal=0;
        public int Aantal { get
            { return _Aantal; } 
            set { if (value >= 0)
                    _Aantal = value;
                else throw new StripException("Het aantal strips kan niet negatief zijn.");
            } }

        public override string ToString()
        {
            string x = null;
            foreach (var a in Auteurs)
                x += a.ToString();
            return $"Strip: {Id}, {Titel}, {Reeks}, {x}, {Uitgeverij}";
        }
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Strip strip)
            {
                result = Id == strip.Id &&
                   Titel == strip.Titel &&
                   Auteurs.SequenceEqual(strip.Auteurs) &&
                   Uitgeverij.Equals(strip.Uitgeverij);
                if (result)
                {
                    if (Reeks == null)
                    {
                        result = strip.Reeks== null;
                        if (result)
                        {
                            if (ReeksNummer == null)
                                result = strip.ReeksNummer == null;
                        }
                    }
                    else
                        result = Reeks.Equals(strip.Reeks) && ReeksNummer == strip.ReeksNummer;
                }
            }
            return result;      
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_Id, Id, _Titel, Titel, Reeks, Auteurs, _Uitgeverij, Uitgeverij);
        }
    }
}
