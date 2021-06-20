using Domeinlaag.Model;
using StripCatalogus___Data_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripCatalogus___Data_Layer
{
    public static class Mapper
    {
        public static Auteur DbToDomain(AuteurDb auteurDb)
        {
            return auteurDb == null ? null : new Auteur(auteurDb.Id, auteurDb.Naam);
        }
        public static AuteurDb DomainToDb(Auteur auteur)
        {
            return new AuteurDb { Id = auteur.Id, Naam = auteur.Naam };
        }
        public static Uitgeverij DbToDomain(UitgeverijDb uitgeverijDb)
        {
            return uitgeverijDb == null ? null : new Uitgeverij(uitgeverijDb.Id, uitgeverijDb.Naam);
        }
        public static UitgeverijDb DomainToDb(Uitgeverij uitgeverij)
        {
            return new UitgeverijDb { Id = uitgeverij.Id, Naam = uitgeverij.Naam };
        }
        public static Reeks DbToDomain(ReeksDb reeksDb)
        {
            return reeksDb == null ? null : new Reeks(reeksDb.Id, reeksDb.Naam);
        }
        public static ReeksDb DomainToDb(Reeks reeks)
        {
            return reeks == null ? null : new ReeksDb { Id = reeks.Id, Naam = reeks.Naam };
        }
        public static Strip DbToDomain(StripDb stripDb)
        {
            return stripDb == null ? null : new Strip(
                stripDb.Id,
                stripDb.Titel,
                DbToDomain(stripDb.Reeks),
                stripDb.ReeksNummer,
                stripDb.Auteurs.Select(x => new Auteur(x.Id, x.Naam)).ToList(),
                DbToDomain(stripDb.Uitgeverij),
                stripDb.Aantal
                );
        }
        public static StripDb DomainToDb(Strip strip)
        {
            return new StripDb
            {
                Id = strip.Id,
                Aantal = strip.Aantal,
                Auteurs = strip.GeefAuteurs().Select(auteur => DomainToDb(auteur)).ToList(),
                Reeks = DomainToDb(strip.Reeks),
                ReeksNummer = strip.ReeksNummer,
                Titel = strip.Titel,
                Uitgeverij = DomainToDb(strip.Uitgeverij)
            };
        }
        public static Bestelling DbToDomain(BestellingDb bestellingDb)
        {
            return bestellingDb == null ? null : new Bestelling(
                bestellingDb.Id,
                bestellingDb.StripsEnAantal.ToDictionary(x => DbToDomain(x.Key), x => x.Value),
                bestellingDb.Datum
                );
        }
        public static BestellingDb DomainToDb(Bestelling bestelling)
        {
            return new BestellingDb
            {
                Id = bestelling.Id,
                Datum = bestelling.Datum,
                StripsEnAantal = bestelling.GeefStripsEnAantallen().ToDictionary(x => DomainToDb(x.Key), x => x.Value)
            };
        }
        public static Levering DbToDomain(LeveringDb leveringDb)
        {
            return leveringDb == null ? null :  new Levering(
                leveringDb.Id,
                leveringDb.StripsEnAantal.ToDictionary(x => DbToDomain(x.Key), x => x.Value),
                leveringDb.BestelDatum,
                leveringDb.LeverDatum
                );
        }
        public static LeveringDb DomainToDb(Levering levering)
        {
            return new LeveringDb
            {
                Id = levering.Id,
                LeverDatum = levering.LeverDatum,
                BestelDatum = levering.BestelDatum,
                StripsEnAantal = levering.GeefStripsEnAantallen().ToDictionary(x => DomainToDb(x.Key), x => x.Value)
            };
        }
    }
}
