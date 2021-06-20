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
    public class BestellingsRepositoryTests
    {
        [TestMethod()]
        public void GeefAlleBestellingenTest()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest(true);
            var bestellingen = uow.Bestellingen.GeefAlleBestellingen();

            Assert.AreEqual(2, bestellingen.Count);
        }

        [TestMethod()]
        public void GeefBestellingVoorIdTest()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest(true);
            Bestelling bestelling = uow.Bestellingen.GeefBestellingVoorId(1);

            DateTime date = new DateTime(2000, 01, 01, 12, 0, 0);
            Assert.AreEqual(1, bestelling.Id);
            Assert.AreEqual(9, bestelling.GeefStripsEnAantallen().First().Value);
            Assert.AreEqual(uow.Strips.GeefStripViaId(1).Aantal, bestelling.GeefStripsEnAantallen().First().Key.Aantal);
            Assert.AreEqual(uow.Strips.GeefStripViaId(2).Aantal, bestelling.GeefStripsEnAantallen().Last().Key.Aantal);
            Assert.AreEqual(2, bestelling.GeefStripsEnAantallen().Last().Value);
            Assert.AreEqual(date, bestelling.Datum);
        }

        [TestMethod()]
        public void VoegBestellingToeTest()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest(true);

            Dictionary<Strip, int> strips = new Dictionary<Strip, int>
            {
                {uow.Strips.GeefStripViaId(3), 20 },
                {uow.Strips.GeefStripViaId(1), 10 },
            };

            DateTime date = new DateTime(2000, 01, 01, 1, 12, 00);
            uow.Bestellingen.VoegBestellingToe(new Bestelling(strips, date));
            Bestelling bestelling = uow.Bestellingen.GeefBestellingVoorId(3);


            Assert.AreEqual(3, bestelling.Id);
            Assert.AreEqual(2, bestelling.GeefStripsEnAantallen().Count);
            Assert.AreEqual(20, bestelling.GeefStripsEnAantallen().First().Value);
            Assert.AreEqual(10, bestelling.GeefStripsEnAantallen().Last().Value);
            Assert.AreEqual(date, bestelling.Datum);
            Assert.AreEqual(26, uow.Strips.GeefStripViaId(3).Aantal);
            Assert.AreEqual(19, uow.Strips.GeefStripViaId(1).Aantal);
        }
    }
}