using Domeinlaag.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domeinlaag.Model
{
    public class Auteur : IComparable,IComparable<Auteur>
    {
        public Auteur(string naam)
        {

            Naam = naam;

        }

        public Auteur(int id, string naam) : this(naam)
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
                    throw new AuteurException("Het Id van de Auteur moet groter dan 0 zijn.");

                _Id = value;
            }
        }
        private string _Naam;
        public string Naam
        {
            get { return _Naam; }
            set
            {
                if (value == null)
                    throw new AuteurException("Naam mag niet null zijn");
                else if (value == "")
                    throw new AuteurException("Naam mag niet leeg zijn.");
                _Naam = value;
            }
        }
        public override string ToString()
        {
            return $"Auteur: {Id}, {Naam}";
        }
        public override bool Equals(object obj)
        {
            if (obj is Auteur)
            {
                var x = obj as Auteur;
                return Naam.Equals(x.Naam)&&Id.Equals(x.Id);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_Id, Id, _Naam, Naam);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Auteur);
        }

        public int CompareTo([AllowNull] Auteur other)
        {
            return Id.CompareTo(other.Id);
        }
    }
}
