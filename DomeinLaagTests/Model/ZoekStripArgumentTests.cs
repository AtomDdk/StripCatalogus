using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag;
using System;
using System.Collections.Generic;
using System.Text;
using Domeinlaag.Model;
using Domeinlaag.Exceptions;
using Moq;
using Domeinlaag.Interfaces;

namespace ModelTests.Tests
{
    [TestClass()]
    public class ZoekStripArgumentTests
    {
        private Mock<IUnitOfWork> mUoW;
        private List<Strip> MockedStrips;
        public ZoekStripArgumentTests()
        {
            mUoW = new Mock<IUnitOfWork>();
            MockedStrips = new List<Strip>();
            SetupStrips();
        }
        private void SetupStrips()
        {
            //strip 1
            Reeks reeks1 = new Reeks(1, "TestReeks1");
            Auteur auteur1 = new Auteur(1, "TestAuteur1");
            Uitgeverij uitgeverij1 = new Uitgeverij(1, "TestUitgeverij1");
            Strip strip1 = new Strip("TestStrip1", reeks1, 1, new List<Auteur>() { auteur1 }, uitgeverij1);
            MockedStrips.Add(strip1);
            //strip 2
            Reeks reeks2 = new Reeks(2, "TestReeks2");
            Auteur auteur2 = new Auteur(2, "TestAuteur2");
            Auteur auteur3 = new Auteur(3, "TestAuteur3");
            Uitgeverij uitgeverij2 = new Uitgeverij(2, "TestUitgeverij2");
            Strip strip2 = new Strip("TestStrip2", reeks2, 2, new List<Auteur>() { auteur2, auteur3 }, uitgeverij2);
            MockedStrips.Add(strip2);
            //strips 3
            Uitgeverij uitgeverij3 = new Uitgeverij(3, "TestUitgeverij1");
            Strip strip3 = new Strip("TestStrip3", reeks1, 13, new List<Auteur>() { auteur3 }, uitgeverij3);
            MockedStrips.Add(strip3);
            //strip 4
            Strip strip4 = new Strip("TestStrip1", null, null, new List<Auteur>() { auteur1 }, uitgeverij1);
            MockedStrips.Add(strip4);
        }
        private List<Strip> GeefMoqStrips(int aantal = -1)
        {
            if (aantal == -1)
                return MockedStrips;
            else
            {
                List<Strip> strips = new List<Strip>();
                for (int i = 0; i < aantal; i++)
                {
                    strips.Add(MockedStrips[i]);
                }
                return strips;
            }
        }




        //effectieve testen
        [TestMethod()]
        public void StelAuteurIn_BoolOpTrue()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.Auteur = new Auteur("testNaam");

            Assert.IsTrue(zoken.FilterOpAuteur == true);
            Assert.IsTrue(zoken.FilterOpUitgeverij== false);
            Assert.IsTrue(zoken.FilterOpReeks == false);
            Assert.IsTrue(zoken.FilterOpReeksNummer== false);
            Assert.IsTrue(zoken.FilterOpTitel == false);
        }
        [TestMethod()]
        public void StelUitgeverijIn_BoolOpTrue()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.Uitgeverij = new Uitgeverij("testNaam");

            Assert.IsTrue(zoken.FilterOpAuteur == false);
            Assert.IsTrue(zoken.FilterOpUitgeverij == true);
            Assert.IsTrue(zoken.FilterOpReeks == false);
            Assert.IsTrue(zoken.FilterOpReeksNummer == false);
            Assert.IsTrue(zoken.FilterOpTitel == false);
        }
        [TestMethod()]
        public void StelReeksIn_BoolOpTrue()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.Reeks= new Reeks("testNaam");

            Assert.IsTrue(zoken.FilterOpAuteur == false);
            Assert.IsTrue(zoken.FilterOpUitgeverij == false);
            Assert.IsTrue(zoken.FilterOpReeks == true);
            Assert.IsTrue(zoken.FilterOpReeksNummer == false);
            Assert.IsTrue(zoken.FilterOpTitel == false);
        }
        [TestMethod()]
        public void StelReeksNummerIn_BoolOpTrue()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.ReeksNummer = 1;

            Assert.IsTrue(zoken.FilterOpAuteur == false);
            Assert.IsTrue(zoken.FilterOpUitgeverij == false);
            Assert.IsTrue(zoken.FilterOpReeks == false);
            Assert.IsTrue(zoken.FilterOpReeksNummer == true);
            Assert.IsTrue(zoken.FilterOpTitel == false);
        }
        [TestMethod()]
        public void StelTitelIn_BoolOpTrue()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.Titel = "test";

            Assert.IsTrue(zoken.FilterOpAuteur == false);
            Assert.IsTrue(zoken.FilterOpUitgeverij == false);
            Assert.IsTrue(zoken.FilterOpReeks == false);
            Assert.IsTrue(zoken.FilterOpReeksNummer == false);
            Assert.IsTrue(zoken.FilterOpTitel == true);
        }
        [TestMethod()]
        public void StelAllesIn_BoolsOpTrue()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.Titel = "test";
            zoken.ReeksNummer = 1;
            zoken.Reeks = new Reeks("testNaam");
            zoken.Uitgeverij = new Uitgeverij("testNaam");
            zoken.Auteur = new Auteur("testNaam");

            Assert.IsTrue(zoken.FilterOpAuteur == true);
            Assert.IsTrue(zoken.FilterOpUitgeverij == true);
            Assert.IsTrue(zoken.FilterOpReeks == true);
            Assert.IsTrue(zoken.FilterOpReeksNummer == true);
            Assert.IsTrue(zoken.FilterOpTitel == true);
        }
        [TestMethod()]
        public void StelTitelIn_DaarnaFilterTerugOpFalse_WordtNietGebruiktInSearch()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Titel = MockedStrips[0].Titel;
            args.FilterOpTitel = false;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 4, "Het correcte aantal strips werd niet gereturned,er werd toch gefilterd");
        }
        [TestMethod()]
        public void StelAuteurIn_DaarnaFilterTerugOpFalse_WordtNietGebruiktInSearch()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Auteur = MockedStrips[0].GeefAuteurs()[0];
            args.FilterOpAuteur = false;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 4, "Het correcte aantal strips werd niet gereturned,er werd toch gefilterd");
        }
        [TestMethod()]
        public void StelUitgeverijIn_DaarnaFilterTerugOpFalse_WordtNietGebruiktInSearch()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Uitgeverij = MockedStrips[0].Uitgeverij;
            args.FilterOpUitgeverij= false;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 4, "Het correcte aantal strips werd niet gereturned,er werd toch gefilterd");
        }
        [TestMethod()]
        public void StelReeksIn_DaarnaFilterTerugOpFalse_WordtNietGebruiktInSearch()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Reeks = MockedStrips[0].Reeks;
            args.FilterOpReeks= false;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 4, "Het correcte aantal strips werd niet gereturned,er werd toch gefilterd");
        }
        [TestMethod()]
        public void StelReeksNummerIn_DaarnaFilterTerugOpFalse_WordtNietGebruiktInSearch()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.ReeksNummer = (int)MockedStrips[0].ReeksNummer;
            args.FilterOpReeksNummer = false;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 4, "Het correcte aantal strips werd niet gereturned,er werd toch gefilterd");
        }


        [TestMethod()]
        [ExpectedException(typeof(ZoekStripArgumentsException))]
        public void StelFilterOpAuteurInOpTrue_VerwachtException()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.FilterOpAuteur = true;
        }
        [TestMethod()]
        [ExpectedException(typeof(ZoekStripArgumentsException))]
        public void StelFilterOpUitgeverijInOpTrue_VerwachtException()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.FilterOpUitgeverij= true;
        }
        [TestMethod()]
        [ExpectedException(typeof(ZoekStripArgumentsException))]
        public void StelFilterOpTitelInOpTrue_VerwachtException()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.FilterOpTitel = true;
        }
        [TestMethod()]
        [ExpectedException(typeof(ZoekStripArgumentsException))]
        public void StelFilterOpReeksInOpTrue_VerwachtException()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.FilterOpReeks = true;
        }
        [TestMethod()]
        [ExpectedException(typeof(ZoekStripArgumentsException))]
        public void StelFilterOpReeksNummerInOpTrue_VerwachtException()
        {
            ZoekStripArguments zoken = new ZoekStripArguments();
            zoken.FilterOpReeksNummer = true;
        }
    }
}