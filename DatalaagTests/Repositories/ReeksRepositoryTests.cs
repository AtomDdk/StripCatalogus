using Domeinlaag.Model;
using IntegratieTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StripCatalogus___Data_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StripCatalogus___Data_Layer.Repositories.Tests
{
    [TestClass()]
    public class ReeksRepositoryTests
    {
        private IEnumerable<Reeks> VoegReeksenToe(UnitOfWork uow)
        {
            List<Reeks> reeksen = new List<Reeks>() { new Reeks("Asterix"), new Reeks("Obelix"), new Reeks("Jommeke") };
            foreach (var x in reeksen)
                x.Id = uow.Reeksen.VoegReeksToe(x);
            return reeksen;
        }

        [TestMethod()]
        public void GeefReeksViaId_OnbestaandeId_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.AreEqual(uow.Reeksen.GeefReeksOpId(5), null);
        }

        [TestMethod()]
        public void GeefReeksViaId_ReturntReeks()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Test");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            Assert.AreEqual(reeks, uow.Reeksen.GeefReeksOpId(reeks.Id));
        }

        [TestMethod()]
        public void VoegReeksToe_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Asterix");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            Assert.AreEqual(uow.Reeksen.GeefReeksOpId(reeks.Id), reeks);
        }

        [TestMethod()]
        public void VoegReeksToe_ReeksMetId_MaaktNieuwIdAan()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks(5, "Asterix");

            int nieuwId = uow.Reeksen.VoegReeksToe(reeks);

            Assert.AreEqual(nieuwId, uow.Reeksen.GeefReeksViaNaam(reeks.Naam).Id);
        }

        [TestMethod()]
        public void GeefAlleNummersVanReeks_ReeksIsNull_ThrowsNullReferenceException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.ThrowsException<NullReferenceException>(() => uow.Reeksen.GeefAlleNummersVanReeks(null));
        }

        [TestMethod()]
        public void GeefAlleNummersVanReeks_OnbestaandeReeks_ReturnsLegeList()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.AreEqual(0, uow.Reeksen.GeefAlleNummersVanReeks(new Reeks(1, "Test")).Count());
        }

        [TestMethod()]
        public void GeefAlleNummersVanReeks_ReeksZonderNummers_ReturnsLegeList()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Asterix");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            var reeksen = uow.Reeksen.GeefAlleNummersVanReeks(reeks);

            Assert.IsTrue(reeksen.Count() == 0, $"De lengte van de list van reeksen is {reeksen.Count()} maar moet 0 zijn.");
        }

        [TestMethod()]
        public void GeefAlleNummersVanReeks_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Auteur auteur = new Auteur("Goscinny René");
            auteur.Id = uow.Auteurs.VoegAuteurToe(auteur);

            Uitgeverij uitgeverij = new Uitgeverij("Standaard");
            uitgeverij.Id = uow.Uitgeverijen.VoegUitgeverijToe(uitgeverij);

            Reeks reeks = new Reeks("Asterix");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            uow.Strips.VoegStripToe(new Strip("Asterix en de Gothen", reeks, 1, new List<Auteur>() { auteur }, uitgeverij));
            uow.Strips.VoegStripToe(new Strip("Asterix en de Germanen", reeks, 2, new List<Auteur>() { auteur }, uitgeverij));
            uow.Strips.VoegStripToe(new Strip("Asterix en de Belgen", reeks, 3, new List<Auteur>() { auteur }, uitgeverij));

            List<int> reeksNummers = uow.Reeksen.GeefAlleNummersVanReeks(reeks).ToList();

            Assert.AreEqual(reeksNummers.Count, 3);
            Assert.AreEqual(reeksNummers[0], 1);
            Assert.AreEqual(reeksNummers[1], 2);
            Assert.AreEqual(reeksNummers[2], 3);
        }


        [TestMethod()]
        public void GeefAlleReeksen_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            var reeksen = VoegReeksenToe(uow).ToArray();

            var testReeksen = uow.Reeksen.GeefAlleReeksen().ToArray();

            Assert.IsTrue(testReeksen.Length == 3, $"Het aantal reeksen is {testReeksen.Length} maar moet 3 zijn.");
            Assert.AreEqual(reeksen[0], testReeksen[0]);
            Assert.AreEqual(reeksen[1], testReeksen[2]); // blijkbaar worden de reeksen gesorteerd
            Assert.AreEqual(reeksen[2], testReeksen[1]);
        }

        [TestMethod()]
        public void GeefAlleReeksen_ReeksBestaatNiet_ReturntLegeLijst()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            var testReeksen = uow.Reeksen.GeefAlleReeksen();

            Assert.AreEqual(testReeksen.Count(), 0);
        }

        [TestMethod()]
        public void GeefReekViaNaam_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Test");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            Assert.AreEqual(uow.Reeksen.GeefReeksViaNaam(reeks.Naam), reeks);
        }

        [TestMethod()]
        public void GeefReeksViaNaam_CaseSensitive_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Test");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            Assert.AreEqual(reeks, uow.Reeksen.GeefReeksViaNaam("test"));
        }

        [TestMethod()]
        public void GeefReeksViaNaam_ExtraSpaties_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Test");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            Assert.AreEqual(null, uow.Reeksen.GeefReeksViaNaam("   Test   "));
        }

        [TestMethod()]
        public void GeefReeksViaNaam_ReeksBestaatNiet_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.AreEqual(uow.Reeksen.GeefReeksViaNaam("Test"), null);
        }

        [TestMethod()]
        public void GeefReeksViaNaam_NaamIsNull_ThrowsSqlException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.ThrowsException<SqlException>(() => uow.Reeksen.GeefReeksViaNaam(null));
        }

        [TestMethod()]
        public void GeefReeksViaNaam_LegeString_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.AreEqual(null, uow.Reeksen.GeefReeksViaNaam(""));
        }
        
        [TestMethod()]
        public void UpdateReeks_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Asterix");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);
            reeks.Naam = "Obelix";

            uow.Reeksen.UpdateReeks(reeks);

            Reeks veranderdeReeks = uow.Reeksen.GeefReeksOpId(reeks.Id);

            Assert.AreEqual(reeks, veranderdeReeks);
        }

        [TestMethod()]
        public void UpdateReeks_ReeksNaamBestaatAl_ThrowsSqlException() // <- Naam wordt getest.
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Reeks reeks1 = new Reeks("Asterix");
            reeks1.Id = uow.Reeksen.VoegReeksToe(reeks1);
            Reeks reeks2 = new Reeks("Obelix");
            reeks2.Id = uow.Reeksen.VoegReeksToe(reeks2);

            //Asterix = Obelix
            reeks1.Naam = reeks2.Naam;

            Assert.ThrowsException<SqlException>(() => uow.Reeksen.UpdateReeks(reeks1), $"De reeksnaam is veranderd ook al bestaat" +
                $"Die naam al.");
        }

        [TestMethod()]
        public void UpdateReeks_ReeksIsNull_ThrowsNullReferenceException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Reeks reeks = new Reeks("Test");
            reeks.Id = uow.Reeksen.VoegReeksToe(reeks);

            reeks.Naam = "spiksplinternieuwetest";

            Assert.ThrowsException<NullReferenceException>(() => uow.Reeksen.UpdateReeks(null));
        }

        [TestMethod()]
        public void UpdateReeks_ReeksBestaatNiet_ThrowsDataLayerException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.ThrowsException<DataLayerException>(() => uow.Reeksen.UpdateReeks(new Reeks(1, "Test")));
        }

    }
}