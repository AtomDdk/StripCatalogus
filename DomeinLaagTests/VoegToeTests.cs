using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag;
using System;
using System.Collections.Generic;
using System.Text;
using Domeinlaag.Interfaces;
using StripCatalogus___Data_Layer;
using Domeinlaag.Model;
using System.Linq;
using IntegratieTests;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System.Collections.ObjectModel;
using Domeinlaag.Exceptions;

namespace Domeinlaag.Tests
{

    [TestClass()]
    public class VoegToeTests
    {
        #region setup methods
        private List<Auteur> VoegTestAuteursToe(int aantal = 1)
        {
            List<Auteur> result = new List<Auteur>();
            List<String> auteursnamen = new List<String> { "Goscinny René", "Uderzo Albert" };
            for (int i = 0; i < aantal; i++)
            {
                result.Add(cm.VoegAuteurToe(auteursnamen[i]));
            }
            return result;
        }
        private List<Auteur> VoegTweedeSetTestAuteursToe( int aantal = 1)
        {
            List<Auteur> result = new List<Auteur>();
            List<String> auteursnamen = new List<String> { "TweedeSetAuteur1", "TweedeSetAuteur2" };
            for (int i = 0; i < aantal; i++)
            {
                result.Add(cm.VoegAuteurToe(auteursnamen[i]));
            }
            return result;
        }

        private Uitgeverij VoegTestUitgeverijToe()
        {
            return cm.VoegUitgeverijToe("Dargaud");
        }
        private Uitgeverij VoegTweedeTestUitgeverijToe()
        {
            return cm.VoegUitgeverijToe("TestUitgeverij");
        }
        private Reeks VoegTestReeksToe()
        {
            return cm.VoegReeksToe("Asterix");
        }
        private Reeks VoegTweedeTestReeksToe()
        {
            return cm.VoegReeksToe("TestReeks");
        }
        public UnitOfWork Databasemanager { get; private set; }

        CatalogusManager cm;
        #endregion

        [TestMethod()]
        public void S1x1x1xAddStrip_MetJuisteGegevens_MeerdereAuteurs_MoetStripToevoegen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            //Act
            cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij);


            //Assert
            List<Strip> teruggegevenStrips = cm.GeefAlleStrips();

            Strip teruggegevenStrip = teruggegevenStrips[0];
            ReadOnlyCollection<Auteur> teruggekregenAuteurs = teruggegevenStrip.GeefAuteurs();

            Assert.AreEqual(teruggegevenStrip.Titel, titel);
            Assert.AreEqual(teruggegevenStrip.ReeksNummer, reeksnummer);
            Assert.AreEqual(teruggegevenStrip.Reeks, reeks);
            for (int i = 0; i < auteurs.Count; i++)
            {
                Assert.AreEqual(teruggekregenAuteurs[i], auteurs[i]);
            }
            Assert.AreEqual(teruggegevenStrip.Uitgeverij, uitgeverij);
        }
        [TestMethod()]
        public void S1x1x2xAddStrip_MetNaamReedsInDeDatabank_MoetStripToevoegen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe();
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();
            cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij);

            int reeksNummer2 = 8;
            Reeks reeks2 = VoegTweedeTestReeksToe();
            List<Auteur> auteurs2 = VoegTweedeSetTestAuteursToe();
            Uitgeverij uitgeverij2 = VoegTweedeTestUitgeverijToe();

            cm.VoegStripToe(titel, reeks2, reeksNummer2, auteurs2, uitgeverij2);
            //Act


            //Assert
            List<Strip> teruggegevenStrips = cm.GeefAlleStrips();

            Strip teruggegevenStrip = teruggegevenStrips[1];
            ReadOnlyCollection<Auteur> teruggekregenAuteurs = teruggegevenStrip.GeefAuteurs();

            Assert.AreEqual(teruggegevenStrip.Titel, titel);
            Assert.AreEqual(teruggegevenStrip.ReeksNummer, reeksNummer2);
            Assert.AreEqual(teruggegevenStrip.Reeks, reeks2);
            for (int i = 0; i < auteurs.Count; i++)
            {
                Assert.AreEqual(teruggekregenAuteurs[i], auteurs2[i]);
            }
            Assert.AreEqual(teruggegevenStrip.Uitgeverij, uitgeverij2);
        }

        [TestMethod()]
        public void S1x1x3xAddStrip_ZonderReeksEnZonderReeksNummer_MoetStripToevoegen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int? reeksnummer = null;

            Reeks reeks = null;
            List<Auteur> auteurs = VoegTestAuteursToe();
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            //Act
            cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij);


            //Assert
            List<Strip> teruggegevenStrips = cm.GeefAlleStrips();

            Strip teruggegevenStrip = teruggegevenStrips[0];
            ReadOnlyCollection<Auteur> teruggekregenAuteurs = teruggegevenStrip.GeefAuteurs();

            Assert.AreEqual(teruggegevenStrip.Titel, titel);
            Assert.AreEqual(teruggegevenStrip.ReeksNummer, null);
            Assert.AreEqual(teruggegevenStrip.Reeks, null);
            for (int i = 0; i < auteurs.Count; i++)
            {
                Assert.AreEqual(teruggekregenAuteurs[i], auteurs[i]);
            }
            Assert.AreEqual(teruggegevenStrip.Uitgeverij, uitgeverij);
        }

        [TestMethod()]
        public void S1x1x4xAddStrip_MetZelfdeReeksnummerMaarAndereReeks_MoetStripsToevoegen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks1 = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            string titel2 = titel;

            Reeks reeks2 = VoegTweedeTestReeksToe();
            List<Auteur> auteurs2 = auteurs;

            Uitgeverij uitgeverij2 = uitgeverij;


            //Act
            cm.VoegStripToe(titel, reeks1, reeksnummer, auteurs, uitgeverij);
            cm.VoegStripToe(titel2, reeks2, reeksnummer, auteurs2, uitgeverij2);

            //Assert
            List<Strip> teruggegevenStrips = cm.GeefAlleStrips();

            Assert.AreEqual(2, teruggegevenStrips.Count);

            Strip teruggegevenStrip1 = teruggegevenStrips[0];
            Strip teruggegevenStrip2 = teruggegevenStrips[1];

            Assert.AreEqual(teruggegevenStrip1.Titel, titel);
            Assert.AreEqual(teruggegevenStrip1.Reeks, reeks1);
            Assert.AreEqual(teruggegevenStrip1.ReeksNummer, reeksnummer);
            Assert.AreEqual(teruggegevenStrip2.Titel, titel2);
            Assert.AreEqual(teruggegevenStrip2.Reeks, reeks2);
            Assert.AreEqual(teruggegevenStrip2.ReeksNummer, reeksnummer);
        }


        //strip toevoegen met foutieve waarden
        [TestMethod()]
        public void S1x2x1xAddStrip_MetLegeTitel_MoetStripExceptionGeven()
        {
            //Arrange
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x2xAddStrip_ZonderTitel_MoetStripExceptionGeven()
        {
            //Arrange
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = null;
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();


            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x3xAddStrip_ZonderUitgeverij_MoetStripExceptionGeven()
        {
            
            //Arrange
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = null;


            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x4xAddStrip_NullAlsReeksMaarMetReeksNummer_MoetStripExceptionGeven()
        {

            //Arrange
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = null;
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();


            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x5xAddStrip_MetReeksMaarZonderReeksNummer_MoetStripExceptionGeven()
        {

            //Arrange
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int? reeksnummer = null;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();


            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x6xAddStrip_MetNegatiefReeksNummer_MoetStripExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = -6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x7xAddStrip_ZonderAuteurs_MoetStripExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = null;
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, null, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x8xAddStrip_LegeLijstAuteurs_MoetStripExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = new List<Auteur>();
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, null, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x2x9xAddStrip_LijstAuteursMetDubbelsIn_MoetStripExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            auteurs.Add(auteurs[0]);

            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, null, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x3x1xAddStrip_EenVanDeAuteursNogNietInDeDatabank_MoetCatalogusExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            auteurs.Add(new Auteur(1,"testAuteur"));

            Assert.ThrowsException<CatalogusException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x3x1xDeel2AddStrip_DeAuteursNogNietInDeDatabank_MoetStripExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = new List<Auteur>() {new Auteur("NieuweAuteur") };
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x3x2xAddStrip_ReeksNogNietInDeDatabank_MoetCatalogusExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = new Reeks("nieuweReeks");
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();

            Assert.ThrowsException<CatalogusException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }


        [TestMethod()]
        public void S1x3x3xAddStrip_DeUitgeverijNogNietInDeDatabank_MoetStripExceptionGeven()
        {
            //Arrange
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksnummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = new Uitgeverij("TestUitgeverij");

            Assert.ThrowsException<StripException>(() =>
                cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 0, "De strip werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S1x4x1xAddStrip_MetReeksEnReeksNummerReedsInDeDatabank_MoetCatalogusExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel1 = "Asterix en de Gothen";
            int reeksNummer1 = 6;

            Reeks reeks1 = VoegTestReeksToe();
            List<Auteur> auteurs1 = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij1 = VoegTestUitgeverijToe();

            Strip strip = cm.VoegStripToe(titel1, reeks1, reeksNummer1, auteurs1, uitgeverij1);

            string titel2 = "tweedeTitel";
            List<Auteur> auteurs2 = VoegTweedeSetTestAuteursToe();
            Uitgeverij uitgeverij2 = VoegTweedeTestUitgeverijToe();

            Assert.ThrowsException<CatalogusException>(() =>
                cm.VoegStripToe(titel2, reeks1, reeksNummer1, auteurs2, uitgeverij2), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 1, "De strip werd ondanks de exception toch toegevoegd.");
            Assert.IsTrue(result[0].Titel == titel1);
            Assert.IsTrue(result[0].GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]));
            Assert.IsTrue(result[0].GeefAuteurs()[1].Equals(strip.GeefAuteurs()[1]));
            Assert.IsTrue(result[0].Uitgeverij.Equals(strip.Uitgeverij));
        }


        [TestMethod()]
        public void S1x4x2xAddStrip_DieAlInDbZit_MoetCatalogusExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int reeksNummer = 6;

            Reeks reeks = VoegTestReeksToe();
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();
            //Act
            cm.VoegStripToe(titel, reeks, reeksNummer, auteurs, uitgeverij);


            Assert.ThrowsException<CatalogusException>(() => 
                cm.VoegStripToe(titel, reeks, reeksNummer, auteurs, uitgeverij),"Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 1, "De strip werd ondanks de exception toch toegevoegd.");

        }

        [TestMethod()]
        public void S1x4x2xAddStrip_DieAlInDbZit_ZonderReeksEnReeksNummer_MoetCatalogusExceptionGeven()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string titel = "Asterix en de Gothen";
            int? reeksNummer = null;

            Reeks reeks = null;
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();
            //Act
            cm.VoegStripToe(titel, reeks, reeksNummer, auteurs, uitgeverij);


            Assert.ThrowsException<CatalogusException>(() =>
                cm.VoegStripToe(titel, reeks, reeksNummer, auteurs, uitgeverij), "Er werd niet correct een exception gegooid.");
            var result = cm.GeefAlleStrips();
            Assert.IsTrue(result.Count == 1, "De strip werd ondanks de exception toch toegevoegd.");

        }


        [TestMethod()]
        public void S2x1xVoegReeksToe_MoetCorrectVerlopen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string reeksNaam = "TestReeks";
            Reeks verwachteReeks = cm.VoegReeksToe(reeksNaam);

            var result = cm.GeefAlleReeksen();
            Assert.IsTrue(result.Count == 1, "het correcte aantal Reeksen is niet toegevoegd.");
            Assert.AreEqual(verwachteReeks, result[0]);
        }

        [TestMethod()]
        public void S2x2xVoegReeksToe_MetLegeNaam_MoetReeksExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string reeksNaam = "";

            Assert.ThrowsException<ReeksException>(() =>
                cm.VoegReeksToe(reeksNaam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleReeksen();
            Assert.IsTrue(result.Count == 0, "De reeks werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S2x3xVoegReeksToe_ZonderNaam_MoetReeksExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string reeksNaam = null;

            Assert.ThrowsException<ReeksException>(() =>
                cm.VoegReeksToe(reeksNaam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleReeksen();
            Assert.IsTrue(result.Count == 0, "De reeks werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S2x4xVoegReeksToe_NaamZitAlInDeDatabank_MoetCatalogusExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string reeksNaam = "TestReeks";
            Reeks verwachteReeks = cm.VoegReeksToe(reeksNaam);

            Assert.ThrowsException<CatalogusException>(() =>
                cm.VoegReeksToe(reeksNaam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleReeksen();
            Assert.IsTrue(result.Count == 1, "De reeks werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S3x1xVoegUitgeverijToe_MoetCorrectVerlopen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string uitgeverijNaam = "CoppensCopies";
            Uitgeverij verwachteUitgeverij = cm.VoegUitgeverijToe(uitgeverijNaam);

            var result = cm.GeefAlleUitgeverijen();
            Assert.IsTrue(result.Count == 1, "het correcte aantal Uitgeverijen is niet toegevoegd.");
            Assert.AreEqual(verwachteUitgeverij, result[0]);
        }

        [TestMethod()]
        public void S3x2xVoegUitgeverijToe_MetLegeNaam_MoetUitgeverijExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string naam = "";

            Assert.ThrowsException<UitgeverijException>(() =>
                cm.VoegUitgeverijToe(naam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleUitgeverijen();
            Assert.IsTrue(result.Count == 0, "De Uitgeverij werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S3x3xVoegUitgeverijToe_Zondernaam_MoetUitgeverijExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string naam = null;

            Assert.ThrowsException<UitgeverijException>(() =>
                cm.VoegUitgeverijToe(naam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleUitgeverijen();
            Assert.IsTrue(result.Count == 0, "De Uitgeverij werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S3x4xVoegUitgeverijToe_NaamZitAlInDeDatabank_MoetCatalogusExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string naam = "TestUitgeverij";
            cm.VoegUitgeverijToe(naam);

            Assert.ThrowsException<CatalogusException>(() =>
                cm.VoegUitgeverijToe(naam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleUitgeverijen();
            Assert.IsTrue(result.Count == 1, "De Uitgeverij werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S4x1xVoegAuteurToe_MoetCorrectVerlopen()
        {
            UnitOfWorkTest dbm = new UnitOfWorkTest();
            cm = new CatalogusManager(dbm);

            string auteursNaam = "SinisterSigi";
            Auteur verwachteAuteur = cm.VoegAuteurToe(auteursNaam);

            var result = cm.GeefAlleAuteurs();
            Assert.IsTrue(result.Count == 1, "De auteur is niet correct toegevoegd.");
            Assert.AreEqual(verwachteAuteur, result[0]);
        }

        [TestMethod()]
        public void S4x2xVoegAuteurToe_MetLegeNaam_MoetAuteurExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string naam = "";

            Assert.ThrowsException<AuteurException>(() =>
                cm.VoegAuteurToe(naam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleAuteurs();
            Assert.IsTrue(result.Count == 0, "De Auteur werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S4x3xVoegAuteurToe_ZonderNaam_MoetAuteurExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string naam = null;

            Assert.ThrowsException<AuteurException>(() =>
                cm.VoegAuteurToe(naam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleAuteurs();
            Assert.IsTrue(result.Count == 0, "De Auteur werd ondanks de exception toch toegevoegd.");
        }

        [TestMethod()]
        public void S4x4xVoegAuteurToe_NaamReedsInDeDatabank_MoetCatalogusExceptionWerpen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            string naam = "TestUitgeverij";
            cm.VoegAuteurToe(naam);
            Assert.ThrowsException<CatalogusException>(() =>
                cm.VoegAuteurToe(naam), "Er werd niet correct een exception gegooid.");

            var result = cm.GeefAlleAuteurs();
            Assert.IsTrue(result.Count == 1, "De Auteur werd ondanks de exception toch toegevoegd.");
        }

    }
}