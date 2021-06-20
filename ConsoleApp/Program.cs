using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using ImportEnExport;
using IntegratieTests;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //UnitOfWorkTest testWork = new UnitOfWorkTest();
            //CatalogusManager manager = new CatalogusManager(testWork);
            Reader.GoesBrrrr();

            UnitOfWork dbm = new UnitOfWork();
            CatalogusManager _cm = new CatalogusManager(dbm);

            var x = _cm.GeefReeksViaId(25);
            Console.ReadLine();
            //CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            //string titel = "Asterix en de Gothen";
            //string reeksNaam = "Asterix";
            //int reeksnummer = 6;
            //List<String> auteursnamen = new List<String> { "Goscinny René", "Uderzo Albert" };
            //string uitgeverijNaam = "Dargaud";

            //Reeks reeks1 = _cm.VoegReeksToe(reeksNaam);
            //List<Auteur> auteurs = new List<Auteur>();
            //foreach (string naam in auteursnamen)
            //{
            //    auteurs.Add(_cm.VoegAuteurToe(naam));
            //}
            //Uitgeverij uitgeverij = _cm.VoegUitgeverijToe(uitgeverijNaam);

            //string titel2 = "Graveyard Shift";
            //string reeksNaam2 = "Batman (The New 52)";
            //List<String> auteursnamen2 = new List<String> { "Scott Snyder" };
            //string uitgeverijNaam2 = "DC Comics";

            //Reeks reeks2 = _cm.VoegReeksToe(reeksNaam2);
            //List<Auteur> auteurs2 = new List<Auteur>();
            //foreach (string naam in auteursnamen2)
            //{
            //    auteurs.Add(_cm.VoegAuteurToe(naam));
            //}
            //Uitgeverij uitgeverij2 = _cm.VoegUitgeverijToe(uitgeverijNaam2);


            ////Act
            //_cm.VoegStripToe(titel, reeks1, reeksnummer, auteurs, uitgeverij);
            //_cm.VoegStripToe(titel2, reeks2, reeksnummer, auteurs2, uitgeverij2);


            //var result = _cm.GeefAlleStrips();
            //Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        //private void VoegStripToeTest()
        //{
        //    UnitOfWorkTest testWork = new UnitOfWorkTest();
        //    CatalogusManager manager = new CatalogusManager(testWork);

        //    Strip strip1 = new Strip("testTitel", "testReeks", 2, new List<Auteur> { new Auteur("testAuteur") }, new Uitgeverij("testUitgeverij"));
        //    manager.VoegStripToe(strip1.Titel, strip1.Reeks, strip1.ReeksNummer, new List<string> { "testAuteur" }, strip1.Uitgeverij.Naam);

        //    Strip test2 = manager.GeefStripViaId(1);
        //    Console.ReadLine();
        //}
        public static void VoegInitieleTestStripToe(CatalogusManager manager)
        {
            string titel = "Asterix en de testen";
            int nummer = 3;
            string reeksNaam = "Asterix";
            List<string> auteursNamen = new List<string> { "Kegelman" };
            string uitgeverijNaam = "CoppensTraitors";

            Reeks reeks = manager.VoegReeksToe(reeksNaam);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(manager.VoegAuteurToe(auteursNamen[0]));
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe(uitgeverijNaam);

            manager.VoegStripToe(titel, reeks, nummer, auteurs, uitgeverij);
        }
    }
}
