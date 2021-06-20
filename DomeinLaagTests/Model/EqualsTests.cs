using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelTests.Tests
{
    [TestClass()]
    public class EqualsTests
    {
        [TestMethod()]
        public void StripEqualsTest_IsGelijkMetNullAlsReeksEnReeksNummer()
        {
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            Strip s = new Strip("TestStrip1", null, null, new List<Auteur>() { auteur1 }, uitgeverij1);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(s.GeefAuteurs()[0]);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, auteurs, s.Uitgeverij);

            Assert.IsTrue(s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijk_VerschillendeTitel_MetNullAlsReeksEnReeksNummer()
        {
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            Strip s = new Strip("TestStrip1", null, null, new List<Auteur>() { auteur1 }, uitgeverij1);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(s.GeefAuteurs()[0]);
            Strip nieuweStrip = new Strip(s.Titel+"abc", s.Reeks, s.ReeksNummer, auteurs, s.Uitgeverij);

            Assert.IsFalse(s.Equals(nieuweStrip), "De Equals methode werkte niet correct: een andere titel returnde geen false"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetNullAlsReeksEnReeksNummerBijEenVanDeTwee()
        {
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            Strip s = new Strip("TestStrip1", null, null, new List<Auteur>() { auteur1 }, uitgeverij1);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(s.GeefAuteurs()[0]);
            Strip nieuweStrip = new Strip(s.Titel + "abc", new Reeks("test"), 2, auteurs, s.Uitgeverij);

            Assert.IsFalse(s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendeUitgeverij_NaamVerschilt()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            Uitgeverij andereUitgeverij = new Uitgeverij(uitgeverij1.Id, "AndereUitgeverij");
            Strip s = new Strip(titel, reeks ,reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, auteurs, andereUitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendeUitgeverij_IdVerschilt()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            Uitgeverij andereUitgeverij = new Uitgeverij(2, uitgeverij1.Naam);
            Strip s = new Strip(titel, reeks, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, auteurs, andereUitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendeReeks_NaamVerschilt()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            Reeks andereReeks = new Reeks(reeks1.Id, "AndereNaam");

            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, andereReeks, s.ReeksNummer, auteurs, s.Uitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendeReeks_IdVerschilt()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            Reeks andereReeks = new Reeks(2, reeks1.Naam);
            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel,andereReeks, s.ReeksNummer, auteurs, s.Uitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendReeksNummer()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            int anderReeksNummer = 123;
            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, reeks1, anderReeksNummer, auteurs, s.Uitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendAuteurs_EenAuteurExtra()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            List<Auteur> anderAuteurs= new List<Auteur>() { auteurs[0] };
            anderAuteurs.Add(new Auteur(2, "tweedeAuteurs"));
            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, anderAuteurs, s.Uitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsGelijkMetDezelfdeAuteurs_AuteursInAndereVolgorde()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Auteur auteur2 = new Auteur(2, "TestAuteur2");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1,auteur2 };

            List<Auteur> anderAuteurs = new List<Auteur>() { auteurs[1],auteur1 };
            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, anderAuteurs, s.Uitgeverij);

            Assert.IsTrue(s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }

        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendAuteurs_AndereNaamVoorAuteur()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            List<Auteur> anderAuteurs = new List<Auteur>() { };
            anderAuteurs.Add(new Auteur(1, "AndereAuteur"));
            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, anderAuteurs, s.Uitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsNietGelijkMetVerschillendAuteurs_AndereIdVoorAuteur()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            List<Auteur> anderAuteurs = new List<Auteur>() { };
            anderAuteurs.Add(new Auteur(2, auteur1.Naam));
            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, anderAuteurs, s.Uitgeverij);

            Assert.IsTrue(!s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod()]
        public void StripEqualsTest_IsGelijkMetZelfdeAuteurs_AnderInstantieVanLijst_ZelfdeContent()
        {
            string titel = "TestStrip1";
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            int reeksnummer = 2;
            Reeks reeks1 = new Reeks(1, "testReeks1");
            List<Auteur> auteurs = new List<Auteur>() { auteur1 };

            List<Auteur> anderAuteurs = new List<Auteur>() { };
            anderAuteurs.Add(new Auteur(auteur1.Id, auteur1.Naam));
            Strip s = new Strip(titel, reeks1, reeksnummer, auteurs, uitgeverij1);
            Strip nieuweStrip = new Strip(s.Titel, s.Reeks, s.ReeksNummer, anderAuteurs, s.Uitgeverij);

            Assert.IsTrue(s.Equals(nieuweStrip), "De Equals methode werkte niet correct"); ;
        }
        [TestMethod]
        public void AuteursEqualTest_IsNietGelijkMetAndereNaam()
        {
            string naam1 = "EersteNaam";
            Auteur auteur1 = new Auteur(1, naam1);
            Auteur auteur2 = new Auteur(auteur1.Id, "nieuweNaam");

            Assert.IsTrue(!auteur1.Equals(auteur2), "De equals methode werkte niet correct");
        }
        [TestMethod]
        public void AuteursEqualTest_IsNietGelijkMetAndereId()
        {
            string naam1 = "EersteNaam";
            Auteur auteur1 = new Auteur(1, naam1);
            Auteur auteur2 = new Auteur(2, auteur1.Naam);

            Assert.IsTrue(!auteur1.Equals(auteur2), "De equals methode werkte niet correct");
        }

        [TestMethod]
        public void UitgeverijEqualTest_IsNietGelijkMetAndereNaam()
        {
            string naam1 = "EersteNaam";
            Uitgeverij uitgeverij1 = new Uitgeverij(1, naam1);
            Uitgeverij uitgeverij2 = new Uitgeverij(uitgeverij1.Id, "nieuweNaam");

            Assert.IsTrue(!uitgeverij1.Equals(uitgeverij2), "De equals methode werkte niet correct");
        }
        [TestMethod]
        public void UitgeverijEqualTest_IsNietGelijkMetAndereId()
        {
            string naam1 = "EersteNaam";
            Uitgeverij uitgeverij1 = new Uitgeverij(1, naam1);
            Uitgeverij uitgeverij2 = new Uitgeverij(2, uitgeverij1.Naam);

            Assert.IsTrue(!uitgeverij1.Equals(uitgeverij2), "De equals methode werkte niet correct");
        }

        [TestMethod]
        public void ReeksEqualTest_IsNietGelijkMetAndereNaam()
        {
            string naam1 = "EersteNaam";
            Reeks reeks1 = new Reeks(1, naam1);
            Reeks reeks2 = new Reeks(reeks1.Id, "nieuweNaam");

            Assert.IsTrue(!reeks1.Equals(reeks2), "De equals methode werkte niet correct");
        }

        [TestMethod]
        public void ReeksEqualTest_IsNietGelijkMetAndereId()
        {
            string naam1 = "EersteNaam";
            Reeks reeks1 = new Reeks(1, naam1);
            Reeks reeks2 = new Reeks(2, reeks1.Naam);

            Assert.IsTrue(!reeks1.Equals(reeks2), "De equals methode werkte niet correct");
        }
    }
}