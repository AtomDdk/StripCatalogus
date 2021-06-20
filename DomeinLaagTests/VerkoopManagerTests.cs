using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag;
using System;
using System.Collections.Generic;
using System.Text;
using IntegratieTests;
using Domeinlaag.Model;

using Domeinlaag.Exceptions;
using System.Linq;

namespace Domeinlaag.Tests

{
    [TestClass()]
    public class VerkoopManagerTests
    {
        private VerkoopManager Vm;
        private CatalogusManager Cm;
        [TestMethod()]
        private Dictionary<Strip, int> GeefStripsEnAantallenMetVoorraadVan100(int aantal = 3)
        {
            Dictionary<Strip, int> result = new Dictionary<Strip, int>();
            Dictionary<Strip, int> voorraad = new Dictionary<Strip, int>();
            Auteur auteur = Cm.VoegAuteurToe("testAuteur");
            List<Auteur> auteurs = new List<Auteur>() { auteur };
            Uitgeverij uitgeverij = Cm.VoegUitgeverijToe("TestUitgeverij");
            for (int i = 0; i < aantal; i++)
            {
                Strip strip = Cm.VoegStripToe("testTitel" + i, null, null, auteurs, uitgeverij);
                voorraad.Add(strip, 100);
                result.Add(strip, 5*i);
            }
            Vm.VoegLeveringToe(voorraad, DateTime.Now.AddDays(-1), DateTime.Today);
            return result;
        }

        private Dictionary<Strip, int> GeefStripsEnAantallenMetOnvoldoendeVoorraad(int aantal = 3)
        {
            Dictionary<Strip, int> result = new Dictionary<Strip, int>();
            Dictionary<Strip, int> voorraad = new Dictionary<Strip, int>();
            Auteur auteur = Cm.VoegAuteurToe("testAuteur");
            List<Auteur> auteurs = new List<Auteur>() { auteur };
            Uitgeverij uitgeverij = Cm.VoegUitgeverijToe("TestUitgeverij");
            for (int i = 0; i < aantal; i++)
            {
                Strip strip = Cm.VoegStripToe("testTitel" + i, null, null, auteurs, uitgeverij);
                voorraad.Add(strip, 100);
                if (i == 1)
                    result.Add(strip, 999);
                else
                result.Add(strip, 5 * i);
            }
            Vm.VoegLeveringToe(voorraad, DateTime.Now.AddDays(-1), DateTime.Today);
            return result;
        }

        [TestMethod()]
        public void VoegBestellingToeTest_CorrecteVoorraadInStrips_WordtCorrectToegevoegd()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);
            var stripsEnAantallen = GeefStripsEnAantallenMetVoorraadVan100(3);
            Vm.VoegBestellingToe(stripsEnAantallen);

            foreach(KeyValuePair<Strip,int>pair in stripsEnAantallen)
            {
                Assert.IsTrue(pair.Key.Aantal == 100 - pair.Value,"Het aantal van de strips werd niet correct aangepast.");
            }


            var result = Vm.GeefAlleBestellingen();
            Assert.IsTrue(result.Count == 1, "Er werd meer dan 1 bestelling aangemaakt.");
            foreach (KeyValuePair<Strip, int> pair in result[0].GeefStripsEnAantallen())
            {
                Assert.IsTrue(pair.Key.Aantal == 100 - pair.Value, "Het aantal van de strips werd niet correct aangepast.");
            }

            var StripResult = Vm.GeefAlleStrips();
            Assert.IsTrue(StripResult[0].Aantal == 100);
            Assert.IsTrue(StripResult[1].Aantal == 95);
            Assert.IsTrue(StripResult[2].Aantal == 90);

        }
        [TestMethod()]
        public void VoegBestellingToeTest_OnvoldoendeVoorraadInStrips_WerptVerkoopException_ErWordtNiksToegevoegdEnGeenVoorraadAangepast()
        {

            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            var x = GeefStripsEnAantallenMetOnvoldoendeVoorraad(3);
            Assert.ThrowsException<VerkoopException>(() => Vm.VoegBestellingToe(x));

            var strips = Vm.GeefAlleStrips();
            Assert.IsTrue(Vm.GeefAlleBestellingen().Count == 0, "Er werd toch een bestelling toegevoegd.");
            foreach(Strip strip in strips)
            {
                Assert.IsTrue(strip.Aantal == 100, "het aantal van de strip werd toch aangepast.");
            }
        }
        [TestMethod()]
        public void VoegLeveringToeTest_VoorraadWordtCorrectAangepast()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            var x = GeefStripsEnAantallenMetVoorraadVan100(3);

            Vm.VoegLeveringToe(x, DateTime.Now.AddDays(-1), DateTime.Today);

            var result = Vm.GeefAlleStrips();
            Assert.IsTrue(Vm.GeefAlleLeveringen().Count == 2, "het aantal Leveringen in de databank was niet correct.");

            Assert.IsTrue(result[0].Aantal == 100, "Het aantal van de eerste strip klopte niet.");
            Assert.IsTrue(result[1].Aantal == 105, "Het aantal van de tweede strip klopte niet.");
            Assert.IsTrue(result[2].Aantal == 110, "Het aantal van de derde strip klopte niet.");

        }
        [TestMethod()]
        public void GeefAlleBestellingenTest_returntAlleBestellingen_StripsErinHebbenCorrecteVoorraad()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);
            var stripsEnAantallen = GeefStripsEnAantallenMetVoorraadVan100(3);
            Vm.VoegBestellingToe(stripsEnAantallen);
            Vm.VoegBestellingToe(stripsEnAantallen);
            Vm.VoegBestellingToe(stripsEnAantallen);

            var result = Vm.GeefAlleBestellingen();
            Assert.IsTrue(result.Count == 3, "Het aantal Bestellingen klopte niet.");
            var bestResult = result[0];
            var stripResult = bestResult.GeefStrips();
            Assert.IsTrue(stripResult[0].Aantal == 100, "Het aantal van de strip klopte niet.");
            Assert.IsTrue(stripResult[1].Aantal == 85, "Het aantal van de strip klopte niet.");
            Assert.IsTrue(stripResult[2].Aantal == 70, "Het aantal van de strip klopte niet.");
        }
        [TestMethod()]
        public void GeefAlleBestellingenTest_GeenBestellingenInDeDatabank_ReturntLegeLijst()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            var result = Vm.GeefAlleBestellingen();

            Assert.IsTrue(result.Count == 0);
        }
        [TestMethod()]
        public void GeefAlleLeveringenTest_GeeftAlleLeveringen_VoorraadIsCorrectInStripObjecten()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);
            var stripsEnAantallen = GeefStripsEnAantallenMetVoorraadVan100(3);
            Vm.VoegLeveringToe(stripsEnAantallen,DateTime.Now.AddDays(-1),DateTime.Today);
            Vm.VoegLeveringToe(stripsEnAantallen, DateTime.Now.AddDays(-1), DateTime.Today);
            Vm.VoegLeveringToe(stripsEnAantallen, DateTime.Now.AddDays(-1), DateTime.Today);

            var result = Vm.GeefAlleLeveringen();
            Assert.IsTrue(result.Count == 4, "Het aantal Leveringen klopte niet.");
            foreach(var x in result)
            {
                var strips = x.GeefStrips();
                Assert.IsTrue(strips[0].Aantal == 100);
                Assert.IsTrue(strips[1].Aantal == 115);
                Assert.IsTrue(strips[2].Aantal == 130);
            }
            
        }
        [TestMethod()]
        public void GeefAlleLeveringenTest_GeenLeveringenInDeDatabank_ReturntLegeLijst()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            var result = Vm.GeefAlleLeveringen();

            Assert.IsTrue(result.Count == 0);
        }
        [TestMethod()]
        public void GeefBestellingVoorIdTest_GeeftDeCorrecteBestellingTerugMetCorrecteVoorraad()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);
            var stripsEnAantallen = GeefStripsEnAantallenMetVoorraadVan100(3);
            Vm.VoegBestellingToe(stripsEnAantallen);

            var result = Vm.GeefBestellingVoorId(1);
            Assert.IsTrue(result.Datum.Date == DateTime.Today);
            Assert.IsTrue(result.GeefStripsEnAantallen().SequenceEqual(stripsEnAantallen));
        }
        [TestMethod()]
        public void GeefBestellingVoorIdTest_Onbestaandeid_ReturntNull()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            var result = Vm.GeefBestellingVoorId(1);

            Assert.IsTrue(result==null);
        }
        [TestMethod()]
        [ExpectedException(typeof(VerkoopException))]
        public void GeefBestellingVoorIdTest_NegatieveId_WerptVerkoopException()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            Vm.GeefBestellingVoorId(-1);
        }
        [TestMethod()]
        public void GeefLeveringVoorIdTest_GeeftDeCorrecteLeveringTerugMetCorrecteVoorraad()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);
            DateTime bestelDatum = new DateTime(2000,12,5);
            DateTime leverDatum = new DateTime(2000, 12, 7);
            var stripsEnAantallen = GeefStripsEnAantallenMetVoorraadVan100(3);
            Vm.VoegLeveringToe(stripsEnAantallen,bestelDatum,leverDatum);

            var result = Vm.GeefLeveringVoorId(2);
            Assert.IsTrue(result.BestelDatum.Equals(bestelDatum));
            Assert.IsTrue(result.LeverDatum.Equals(leverDatum));
            Assert.IsTrue(result.GeefStripsEnAantallen().SequenceEqual(stripsEnAantallen));
        }
        [TestMethod()]
        public void GeefLeveringVoorIdTest_OnbestaandeId_ReturntNull()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            var result = Vm.GeefLeveringVoorId(1);

            Assert.IsTrue(result == null);
        }
        [TestMethod()]
        [ExpectedException(typeof(VerkoopException))]
        public void GeefLeveringVoorIdTest_NegatieveId_WerptVerkoopException()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            Vm.GeefLeveringVoorId(-1);
        }
        [TestMethod]
        public void GeefAlleStripsTest_ReturntAlleStripsInCorrecteVormMetVoorraad()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            GeefStripsEnAantallenMetVoorraadVan100(3);
            var strip = Cm.GeefStripViaId(1);
            Cm.VoegStripToe("NieuweTestStrip11", null, null, strip.GeefAuteurs().ToList(), strip.Uitgeverij);

            var result = Vm.GeefAlleStrips();

            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result[0].Aantal == 100);
            Assert.IsTrue(result[1].Aantal == 100);
            Assert.IsTrue(result[2].Aantal == 100);
            Assert.IsTrue(result[3].Aantal == 0);

        }
        [TestMethod]
        public void GeefAlleStripsVoorVerkoopTest_ReturntAlleStripsMetEenAantalGroterDan0InCorrecteVormMetVoorraad()
        {
            var Uow = new UnitOfWorkTest();
            Vm = new VerkoopManager(Uow);
            Cm = new CatalogusManager(Uow);

            GeefStripsEnAantallenMetVoorraadVan100(3);
            var strip = Cm.GeefStripViaId(1);
            Cm.VoegStripToe("NieuweTestStrip11", null, null, strip.GeefAuteurs().ToList(), strip.Uitgeverij);

            var result = Vm.GeefAlleStripsVoorVerkoop();

            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result[0].Aantal == 100);
            Assert.IsTrue(result[1].Aantal == 100);
            Assert.IsTrue(result[2].Aantal == 100);
        }

    }
}