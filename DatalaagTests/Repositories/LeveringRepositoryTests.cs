using Domeinlaag.Model;
using IntegratieTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StripCatalogus___Data_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripCatalogus___Data_Layer.Repositories.Tests
{
    [TestClass()]
    public class LeveringRepositoryTests
    {

        [TestMethod()]
        public void VoegLeveringToeTest()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest(true);

            DateTime bestelDatum = new DateTime(2000, 01, 01, 1, 16, 00);
            DateTime LeverDatum = new DateTime(2000, 02, 01, 1, 12, 00);
            Dictionary<Strip, int> strips = new Dictionary<Strip, int>
            {
                { uow.Strips.GeefStripViaId(1), 10 },
                {uow.Strips.GeefStripViaId(4), 20 }
            };

            Levering levering = new Levering(strips, bestelDatum, LeverDatum);
            uow.Leveringen.VoegLeveringToe(levering);

            Levering toegevoegdeLevering = uow.Leveringen.GeefLeveringVoorId(3);

            Assert.AreEqual(levering.BestelDatum, toegevoegdeLevering.BestelDatum);
            Assert.AreEqual(levering.LeverDatum, toegevoegdeLevering.LeverDatum);
            Assert.AreEqual(levering.GeefStripsEnAantallen().Count, toegevoegdeLevering.GeefStripsEnAantallen().Count());
            Assert.AreEqual(levering.GeefStripsEnAantallen().First().Value, toegevoegdeLevering.GeefStripsEnAantallen().First().Value);
            Assert.AreEqual(levering.GeefStripsEnAantallen().Last().Value, toegevoegdeLevering.GeefStripsEnAantallen().Last().Value);
            Assert.AreEqual(19, uow.Strips.GeefStripViaId(1).Aantal);
            Assert.AreEqual(0, uow.Strips.GeefStripViaId(4).Aantal);
        }

        [TestMethod()]
        public void GeefAlleLeveringenTest()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest(true);
            var leveringen = uow.Leveringen.GeefAlleLeveringen();

            Assert.AreEqual(2, leveringen.Count);
        }

        [TestMethod()]
        public void GeefLeveringVoorIdTest()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest(true);
            Levering levering = uow.Leveringen.GeefLeveringVoorId(1);

            DateTime bestelDatum = new DateTime(2000, 01, 01, 12, 30, 0);
            DateTime leverDatum = new DateTime(2000, 01, 01, 18, 00, 0);
            Assert.AreEqual(leverDatum, levering.LeverDatum);
            Assert.AreEqual(bestelDatum, levering.BestelDatum);
            Assert.AreEqual(2, levering.GeefStripsEnAantallen().Count);
            Assert.AreEqual(12, levering.GeefStripsEnAantallen().First().Value);
            Assert.AreEqual(19, levering.GeefStripsEnAantallen().Last().Value);
        }
    }
}