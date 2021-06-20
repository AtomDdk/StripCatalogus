using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag;
using System;
using System.Collections.Generic;
using System.Text;
using Domeinlaag.Model;
using IntegratieTests;
using System.Linq;

namespace ModelTests.Tests
{
    [TestClass()]
    public class CatalogusManagerTests
    {
        private CatalogusManager cm;
        public CatalogusManagerTests()
        {
        }
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
        private List<Strip> VoegTestStripsToe(int aantal = 1)
        {
            List<Strip> returnStrips = new List<Strip>();
            var auteurs = VoegTestAuteursToe();
            var uitgeverij = VoegTestUitgeverijToe();
            var reeks = VoegTestReeksToe();
            string titel = "eersteTitel";
            int reeksnummer = 123456;

            Strip result = cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij);
            returnStrips.Add(result);

            for (int i = 1; i < aantal; i++)
                returnStrips.Add(cm.VoegStripToe(titel, reeks, reeksnummer + i, auteurs, uitgeverij));


            return returnStrips;
        }
        [TestMethod()]
        public void VoegStripToe_MoetToegevoegdeStripReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var auteurs = VoegTestAuteursToe();
            var uitgeverij = VoegTestUitgeverijToe();
            var reeks = VoegTestReeksToe();
            string titel = "eersteTitel";
            int reeksnummer = 123456;

            Strip result = cm.VoegStripToe(titel, reeks, reeksnummer, auteurs, uitgeverij);
            Strip expectedResult = cm.GeefStripViaId(1);

            Assert.AreEqual(result, expectedResult, "Het gereturnde type was niet correct");
        }
        [TestMethod]
        public void VoegUitgeverijToe_MoetToegevoegdeUitgeverijReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            string naam = "nieuweUitgeverij";

            Uitgeverij result = cm.VoegUitgeverijToe(naam);

            var expected = cm.GeefUitgeverijViaId(1);

            Assert.AreEqual(result, expected, "De uitgeverijen kwamen niet volledig overeen");
        }
        [TestMethod]
        public void VoegAuteurToe_MoetToegevoegdeAuteurReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            string naam = "nieuweAuteur";

            Auteur result = cm.VoegAuteurToe(naam);

            var expected = cm.GeefAuteurViaId(1);

            Assert.AreEqual(result, expected, "De auteurs kwamen niet volledig overeen");
        }
        [TestMethod]
        public void VoegReeksToe_MoetToegevoegdeReeksReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            string naam = "nieuweReeks";

            Reeks result = cm.VoegReeksToe(naam);

            var expected = cm.GeefReeksViaId(1);

            Assert.AreEqual(result, expected, "De reeksen kwamen niet volledig overeen");
        }

        [TestMethod]
        public void GeefReeksViaId_MoetCorrecteReeksReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            VoegTestReeksToe();
            string naam = "nieuweReeks";

            cm.VoegReeksToe(naam);

            var expected = cm.GeefReeksViaId(2);

            Assert.AreEqual(expected.Id, 2, "De Id was niet correct");
            Assert.AreEqual(expected.Naam, naam, "De naam was niet correct");
        }
        [TestMethod]
        public void GeefAuteurViaId_MoetCorrecteAuteurReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            VoegTestAuteursToe(2);
            string naam = "nieuweAuteur";

            cm.VoegAuteurToe(naam);

            var expected = cm.GeefAuteurViaId(3);

            Assert.AreEqual(expected.Id, 3, "De Id was niet correct");
            Assert.AreEqual(expected.Naam, naam, "De naam was niet correct");
        }
        [TestMethod]
        public void GeefUitgeverijViaId_MoetCorrecteUitgeverijReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            VoegTestUitgeverijToe();
            string naam = "nieuweUitgeverij";

            cm.VoegUitgeverijToe(naam);

            var expected = cm.GeefUitgeverijViaId(2);

            Assert.AreEqual(expected.Id, 2, "De Id was niet correct");
            Assert.AreEqual(expected.Naam, naam, "De naam was niet correct");
        }
        [TestMethod]
        public void GeefStripViaId_MoetCorrecteStripReturnenMetAlleDataTypes()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();
            string titel = "nieuweStrip";
            int reeksNummer = 5;
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            var reeks = VoegTestReeksToe();

            cm.VoegStripToe(titel+"kh", reeks, reeksNummer+1, auteurs, uitgeverij);
            cm.VoegStripToe(titel, reeks, reeksNummer, auteurs, uitgeverij);

            var expected = cm.GeefStripViaId(2);

            Assert.AreEqual(expected.Id, 2, "De Id was niet correct");
            Assert.AreEqual(expected.Titel, titel, "De naam was niet correct");
            Assert.AreEqual(expected.Uitgeverij, uitgeverij, "De uitgeverij was niet correct");
            Assert.IsTrue(expected.GeefAuteurs().SequenceEqual(auteurs), "De auteurs waren niet correct");
            Assert.AreEqual(expected.Reeks, reeks, "De reeks was niet correct");
            Assert.AreEqual(expected.ReeksNummer, reeksNummer, "Het ReeksNummer was niet correct");
        }
        [TestMethod]
        public void GeefStripViaId_ZonderReeksEnReeksNummer_MoetCorrecteStripReturnenMetAlleDataTypes()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            Uitgeverij uitgeverij = VoegTestUitgeverijToe();
            string titel = "nieuweStrip";
            int? reeksNummer = null;
            List<Auteur> auteurs = VoegTestAuteursToe(2);
            Reeks reeks = null;

            cm.VoegStripToe(titel + "kh", reeks, reeksNummer + 1, auteurs, uitgeverij);
            cm.VoegStripToe(titel, reeks, reeksNummer, auteurs, uitgeverij);

            var expected = cm.GeefStripViaId(2);

            Assert.AreEqual(expected.Id, 2, "De Id was niet correct");
            Assert.AreEqual(expected.Titel, titel, "De naam was niet correct");
            Assert.AreEqual(expected.Uitgeverij, uitgeverij, "De uitgeverij was niet correct");
            Assert.IsTrue(expected.GeefAuteurs().SequenceEqual(auteurs), "De auteurs waren niet correct");
            Assert.AreEqual(expected.Reeks, null, "De reeks was niet correct");
            Assert.AreEqual(expected.ReeksNummer, null, "Het ReeksNummer was niet correct");
        }
        [TestMethod]
        public void GeefReeksViaId_OnbestaandeId_MoetNullReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefReeksViaId(2);

            Assert.AreEqual(expected, null, "Er werd geen null gereturned");
        }
        [TestMethod]
        public void GeefUitgeverijViaId_OnbestaandeId_MoetNullReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefUitgeverijViaId(2);

            Assert.AreEqual(expected, null, "Er werd geen null gereturned");
        }
        [TestMethod]
        public void GeefAuteurViaId_OnbestaandeId_MoetNullReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefAuteurViaId(2);

            Assert.AreEqual(expected, null, "Er werd geen null gereturned");
        }
        [TestMethod]
        public void GeefStripViaId_OnbestaandeId_MoetNullReturnen()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefStripViaId(2);

            Assert.AreEqual(expected, null, "Er werd geen null gereturned");
        }
        [TestMethod]
        public void GeefAlleStrips_MetLegeDatabank_GeeftLegeLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefAlleStrips();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 0, "De count van de list was niet correct");
        }
        [TestMethod]
        public void GeefAlleAuteurs_MetLegeDatabank_GeeftLegeLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefAlleAuteurs();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 0, "De count van de list was niet correct");
        }
        [TestMethod]
        public void GeefAlleUitgeverijen_MetLegeDatabank_GeeftLegeLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefAlleUitgeverijen();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 0, "De count van de list was niet correct");
        }
        [TestMethod]
        public void GeefAlleReeksen_MetLegeDatabank_GeeftLegeLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            var expected = cm.GeefAlleReeksen();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 0, "De count van de list was niet correct");
        }

        //check dat alles gereturned wordt

        [TestMethod]
        public void GeefAlleStrips_GeeftAlleEntriesInEenLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            VoegTestStripsToe(8);
            var expected = cm.GeefAlleStrips();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 8, "De count van de list was niet correct.");
            Assert.IsTrue(expected[0].Id == 1, "De id was niet correct");
            Assert.IsTrue(expected[1].Id == 2, "De id was niet correct");
            Assert.IsTrue(expected[2].Id == 3, "De id was niet correct");
            Assert.IsTrue(expected[3].Id == 4, "De id was niet correct");
            Assert.IsTrue(expected[4].Id == 5, "De id was niet correct");
            Assert.IsTrue(expected[5].Id == 6, "De id was niet correct");
            Assert.IsTrue(expected[6].Id == 7, "De id was niet correct");
            Assert.IsTrue(expected[7].Id == 8, "De id was niet correct");
        }
        [TestMethod]
        public void GeefAlleAuteurs_GeeftAlleEntriesInEenLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            VoegTestAuteursToe(2);
            var expected = cm.GeefAlleAuteurs();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 2, "De count van de list was niet correct");
            Assert.IsTrue(expected[0].Id == 1, "De id was niet correct");
            Assert.IsTrue(expected[1].Id == 2, "De id was niet correct");
        }
        [TestMethod]
        public void GeefAlleUitgeverijen_GeeftAlleEntriesInEenLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());

            VoegTestUitgeverijToe();
            VoegTweedeTestUitgeverijToe();
            var expected = cm.GeefAlleUitgeverijen();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 2, "De count van de list was niet correct");
            Assert.IsTrue(expected[0].Id == 1, "De id was niet correct");
            Assert.IsTrue(expected[1].Id == 2, "De id was niet correct");
        }
        [TestMethod]
        public void GeefAlleReeksen_GeeftAlleEntriesInEenLijst()
        {
            cm = new CatalogusManager(new UnitOfWorkTest());
            VoegTestReeksToe();
            VoegTweedeTestReeksToe();
            var expected = cm.GeefAlleReeksen();

            Assert.IsTrue(expected != null, "er werd null gereturned");
            Assert.AreEqual(expected.Count, 2, "De count van de list was niet correct");
            Assert.IsTrue(expected[0].Id == 1, "De id was niet correct");
            Assert.IsTrue(expected[1].Id == 2, "De id was niet correct");
        }

    }
}