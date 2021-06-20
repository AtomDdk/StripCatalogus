using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag;
using System;
using System.Collections.Generic;
using System.Text;
using Domeinlaag.Model;
using IntegratieTests;
using Moq;
using Domeinlaag.Interfaces;

namespace ZoekTests.Tests
{
    [TestClass()]
    public class ZoekStripTests
    {
        private Mock<IUnitOfWork> mUoW;
        private List<Strip> MockedStrips;
        public ZoekStripTests()
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
            Strip strip4 = new Strip("TestStrip1", null,null, new List<Auteur>() { auteur1 }, uitgeverij1);
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
        [TestMethod()]
        public void ZoekStripTest_GebruiktGeefAlleStrips()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(m => m.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(1));
            manager.ZoekStrips(new ZoekStripArguments());
            mUoW.Verify(Mock => Mock.Strips.GeefAlleStrips(), Times.Once());

        }

        [TestMethod()]
        public void S1x1x1xZoekStripOpNaamTest_DeelVanNaam_MoetCorrecteStripsReturnen()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(2));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Titel = "Test";

            List<Strip> result = manager.ZoekStrips(args);

            Assert.IsTrue(result.Count == 2, "Er werd niet correct een strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De eerste gereturnde strip was niet correct.");
            Assert.IsTrue(result[1].Equals(MockedStrips[1]), "De tweede gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x1x2xZoekStripOpNaamTest_DeelVanNaamZonderCorrectHoofdLetters_MoetCorrecteStripsReturnen()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(2));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Titel = "test";

            List<Strip> result = manager.ZoekStrips(args);

            Assert.IsTrue(result.Count == 2, "Er werd niet correct een strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De eerste gereturnde strip was niet correct.");
            Assert.IsTrue(result[1].Equals(MockedStrips[1]), "De tweede gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x1x3xZoekStripOpNaamTest_ExacteNaam_MoetCorrecteStripsReturnen()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(2));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Titel = MockedStrips[0].Titel;

            List<Strip> result = manager.ZoekStrips(args);

            Assert.IsTrue(result.Count == 1, "Er werd niet correct een strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De gereturnde strip was niet correct.");
        }
        //voor ik hier verder doe: MOCKEN??? Uiteindelijk trek ik toch gewoon alles binnen, geen nood hier om de databank aan te spreken.

        //vanaf hier mock
        [TestMethod()]
        public void S1x1x4xZoekStripOpNaamTest_ExacteNaam_VerschillendeHoofdletters_moetCorrecteStripReturnen()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(3));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Titel = "TESTstrip1";

            List<Strip> result = manager.ZoekStrips(args);

            Assert.IsTrue(result.Count == 1, "Er werd niet correct een strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x1x5xZoekStripOpNaamTest_ExacteNaam_ExtraSpaties_moetCorrecteStripReturnen()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(3));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Titel = "            teststrip1              ";

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Er werd niet correct een strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x2x1xZoekStripOpReeksNummer_DeelVanReeksNummer_MagDeStripNietReturnen()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips());
            ZoekStripArguments args = new ZoekStripArguments();
            args.ReeksNummer = 1;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Er werd meer dan één strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x2x2xZoekStripOpReeksNummer_DeelVanReeksNummer_MagDeStripNietReturnen()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips());
            ZoekStripArguments args = new ZoekStripArguments();
            args.ReeksNummer = 2;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Er werd meer dan één strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[1]), "De gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x3x1xZoekStripOpReeks_ZonderReeksNummer_ReturntAlleStripsVanReeks()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips());
            ZoekStripArguments args = new ZoekStripArguments();
            args.Reeks = MockedStrips[0].Reeks;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 2, "Het correct aantal strips werd niet gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De gereturnde strip was niet correct.");
            Assert.IsTrue(result[1].Equals(MockedStrips[2]), "De gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x3x2xZoekStripOpReeks_InclusiefReeksNummer_ReturntAlleStripsVanReeks()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips());
            ZoekStripArguments args = new ZoekStripArguments();
            args.Reeks = MockedStrips[0].Reeks;
            args.ReeksNummer = (int)MockedStrips[0].ReeksNummer;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Er werd meer dan één strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x3x3xZoekStripOpReeks_OnbestaandeReeks_ReturntAlleStripsVanReeks()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips());
            ZoekStripArguments args = new ZoekStripArguments();
            args.Reeks = new Reeks("onbestaandeReeks");

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 0, "Er werd toch een strip gereturned.");
        }
        [TestMethod()]
        public void S1x3x4xZoekStripOpReeks_GeenReeks_ReturntAlleStripsVanReeks()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Reeks = null;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Er werd toch een strip gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[3]), "De gereturnde strip was niet correct.");
        }
        [TestMethod()]
        public void S1x4x1xZoekStripOpUitgeverij_ReturntCorrecteStrips()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips());
            ZoekStripArguments args = new ZoekStripArguments();
            args.Uitgeverij = new Uitgeverij("OnbestaandeUitgeverij");

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 0, "Er werd toch een strip gereturned.");
        }
        [TestMethod()]
        public void S1x4x2xZoekStripOpUitgeverij_UitgeverijDieNogNietBestaat_ReturntGeenEnkeleStrip()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Uitgeverij = MockedStrips[0].Uitgeverij;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 2, "Het correcte aantal strips werd niet gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]),"De eerste gereturnde strip was niet correct");
            Assert.IsTrue(result[1].Equals(MockedStrips[3]), "De tweede gereturnde strip was niet correct");
        }
        [TestMethod()]
        public void S1x5x1xZoekStripOpAuteurs_AuteurDieNogNietBestaat_ReturntGeenEnkeleStrip()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Auteur = new Auteur("Onbestaande Auteur");

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 0, "Het correcte aantal strips werd niet gereturned.");
        }
        [TestMethod()]
        public void S1x5x2xZoekStripOpAuteurs_ReturntCorrecteStrips()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Auteur = MockedStrips[1].GeefAuteurs()[1];

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 2, "Het correcte aantal strips werd niet gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[1]), "De eerste gereturnde strip was niet correct");
            Assert.IsTrue(result[1].Equals(MockedStrips[2]), "De tweede gereturnde strip was niet correct");
        }
        [TestMethod()]
        public void S1x6x1xZoekStripOpExacteWaarden_ReturntExactDieStrip()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips());
            ZoekStripArguments args = new ZoekStripArguments();
            args.Auteur = MockedStrips[0].GeefAuteurs()[0];
            args.Titel = MockedStrips[0].Titel;
            args.Reeks= MockedStrips[0].Reeks;
            args.ReeksNummer = (int)MockedStrips[0].ReeksNummer;
            args.Uitgeverij = MockedStrips[0].Uitgeverij;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Het correcte aantal strips werd niet gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De eerste gereturnde strip was niet correct");
        }
        [TestMethod()]
        public void S1x6x2xZoekStripOpAuteurEnUitgeverij_ReturntCorrecteStrips()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Auteur = MockedStrips[0].GeefAuteurs()[0];
            args.Uitgeverij = MockedStrips[0].Uitgeverij;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 2, "Het correcte aantal strips werd niet gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De eerste gereturnde strip was niet correct");
            Assert.IsTrue(result[1].Equals(MockedStrips[3]), "De eerste gereturnde strip was niet correct");
        }
        [TestMethod()]
        public void S1x6x3xZoekStripOpTitelEnReeks_ReturntCorrecteStrips()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Titel = MockedStrips[2].Titel;
            args.Reeks = MockedStrips[0].Reeks;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Het correcte aantal strips werd niet gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[2]), "De eerste gereturnde strip was niet correct");
        }
        [TestMethod()]
        public void S1x6x4xZoekStripOpReeksEnReeksNummer_ReturntCorrecteStrips()
        {
            CatalogusManager manager = new CatalogusManager(mUoW.Object);
            mUoW.Setup(unit => unit.Strips.GeefAlleStrips()).Returns(GeefMoqStrips(4));
            ZoekStripArguments args = new ZoekStripArguments();
            args.Reeks = MockedStrips[2].Reeks;
            args.ReeksNummer = (int)MockedStrips[0].ReeksNummer;

            List<Strip> result = manager.ZoekStrips(args);
            Assert.IsTrue(result.Count == 1, "Het correcte aantal strips werd niet gereturned.");
            Assert.IsTrue(result[0].Equals(MockedStrips[0]), "De eerste gereturnde strip was niet correct");
        }
    }
}