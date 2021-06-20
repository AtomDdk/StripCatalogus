using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Domeinlaag.Exceptions;

namespace ModelTests.Tests
{
    [TestClass()]
    public class DomeinRegelTests
    {
        //auteurs
        [TestMethod()]
        [ExpectedException(typeof(AuteurException))]
        public void MaakAuteurZonderNaamTest_MoetAuteurExceptionGooien()
        {
            Auteur a1 = new Auteur(null);
        }
        [TestMethod()]
        [ExpectedException(typeof(AuteurException))]
        public void MaakAuteurMetFoutieveId_MoetAuteurExceptionGooien()
        {
            Auteur a1 = new Auteur(-1,"testnaam");
        }
        [TestMethod()]
        [ExpectedException(typeof(AuteurException))]
        public void MaakAuteurMetLegeNaamTest_MoetAuteurExceptionGooien()
        {
            Auteur a1 = new Auteur("");
        }

        //uitgeverijen
        [TestMethod()]
        [ExpectedException(typeof(UitgeverijException))]
        public void MaakUitgeverijZonderNaamTest_MoetUitgeverijExceptionGooien()
        {
            Uitgeverij a1 = new Uitgeverij(null);
        }
        [TestMethod()]
        [ExpectedException(typeof(UitgeverijException))]
        public void MaakUitgeverijMetFoutieveId_MoetUitgeverijExceptionGooien()
        {
            Uitgeverij a1 = new Uitgeverij(-1,"testString");
        }
        [TestMethod()]
        [ExpectedException(typeof(UitgeverijException))]
        public void MaakUitgeverijMetLegeNaamTest_MoetUitgeverijExceptionGooien()
        {
            Uitgeverij a1 = new Uitgeverij("");
        }

        //Reeksen
        [TestMethod()]
        [ExpectedException(typeof(ReeksException))]
        public void MaakReeksZonderNaamTest_MoetReeksExceptionGooien()
        {
            Reeks a1 = new Reeks(null);
        }
        [TestMethod()]
        [ExpectedException(typeof(ReeksException))]
        public void MaakReeksMetFoutieveIdTest_MoetReeksExceptionGooien()
        {
            Reeks a1 = new Reeks(-1,"testReeks");
        }
        [TestMethod()]
        [ExpectedException(typeof(ReeksException))]
        public void MaakReeksMetLegeNaamTest_MoetReeksExceptionGooien()
        {
            Reeks a1 = new Reeks("");
        }

        //strips
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void MaakStripMetFoutieveIdTest_MoetStripExceptionGooien()
        {
            string titel = "testTitel";
            Reeks reeks = new Reeks("testReeks");
            int reeksNummer = 2;
            List<Auteur> auteurs = new List<Auteur> { new Auteur("testAuteur") };
            Uitgeverij uitgeverij = new Uitgeverij("testUitgeverij");
            Strip a1 = new Strip(-2,titel,reeks,reeksNummer,auteurs,uitgeverij);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void MaakStripMetLegeStringAlsTitelTest_MoetStripExceptionGooien()
        {
            string titel = "";
            Reeks reeks = new Reeks("testReeks");
            int reeksNummer = 2;
            List<Auteur> auteurs = new List<Auteur> { new Auteur("testAuteur") };
            Uitgeverij uitgeverij = new Uitgeverij("testUitgeverij");
            Strip a1 = new Strip(titel, reeks, reeksNummer, auteurs, uitgeverij);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void MaakStripZonderTitelTest_MoetStripExceptionGooien()
        {
            string titel = null;
            Reeks reeks = new Reeks("testReeks");
            int reeksNummer = 2;
            List<Auteur> auteurs = new List<Auteur> { new Auteur("testAuteur") };
            Uitgeverij uitgeverij = new Uitgeverij("testUitgeverij");
            Strip a1 = new Strip(titel, reeks, reeksNummer, auteurs, uitgeverij);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void MaakStripZonderReeksTest_MoetStripExceptionGooien()
        {
            string titel = "testTitel";
            Reeks reeks = null;
            int reeksNummer = 2;
            List<Auteur> auteurs = new List<Auteur> { new Auteur("testAuteur") };
            Uitgeverij uitgeverij = new Uitgeverij("testUitgeverij");
            Strip a1 = new Strip(titel, reeks, reeksNummer, auteurs, uitgeverij);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void MaakStripZonderAuteursTest_MoetStripExceptionGooien()
        {
            string titel = "testTitel";
            Reeks reeks = new Reeks("testReeks");
            int reeksNummer = 2;
            List<Auteur> auteurs = null;
            Uitgeverij uitgeverij = new Uitgeverij("testUitgeverij");
            Strip a1 = new Strip(titel, reeks, reeksNummer, auteurs, uitgeverij);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void MaakStripZonderUitgeverijTest_MoetStripExceptionGooien()
        {
            string titel = "testTitel";
            Reeks reeks = new Reeks("testReeks");
            int reeksNummer = 2;
            List<Auteur> auteurs = new List<Auteur> { new Auteur("testAuteur") };
            Uitgeverij uitgeverij = null;
            Strip a1 = new Strip(titel, reeks, reeksNummer, auteurs, uitgeverij);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void MaakStripMetNegatiefReeksNummerTest_MoetStripExceptionGooien()
        {
            string titel = "testTitel";
            Reeks reeks = new Reeks("testReeks");
            int reeksNummer = -2;
            List<Auteur> auteurs = new List<Auteur> { new Auteur("testAuteur") };
            Uitgeverij uitgeverij = new Uitgeverij("testUitgeverij");
            Strip a1 = new Strip(titel, reeks, reeksNummer, auteurs, uitgeverij);
        }
    }
}