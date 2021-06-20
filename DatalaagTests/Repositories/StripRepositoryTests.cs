using Domeinlaag.Exceptions;
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
    public class StripRepositoryTests
    {
        private List<Auteur> VoegAuteursToe(UnitOfWorkTest uow)
        {
            List<Auteur> auteurs = new List<Auteur>() { new Auteur("Goscinny René"), new Auteur("Uderzo Albert"), new Auteur("Zack Snyder") };
            foreach (var x in auteurs)
                x.Id = uow.Auteurs.VoegAuteurToe(x);
            return auteurs;
        }
        private List<Uitgeverij> VoegUitgeverijenToe(UnitOfWorkTest uow)
        {
            List<Uitgeverij> uitgeverijen = new List<Uitgeverij>() { new Uitgeverij("Dargaud"), new Uitgeverij("Standaard") };
            foreach (var x in uitgeverijen)
                x.Id = uow.Uitgeverijen.VoegUitgeverijToe(x);
            return uitgeverijen;
        }
        private List<Reeks> VoegReeksenToe(UnitOfWorkTest uow)
        {
            List<Reeks> reeksen = new List<Reeks>() { new Reeks("Asterix"), new Reeks("Obelix") };
            foreach (var x in reeksen)
                x.Id = uow.Reeksen.VoegReeksToe(x);
            return reeksen;
        }
        [TestMethod()]
        public void VoegStripToe_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            uow.Strips.VoegStripToe(new Strip("Asterix en de Gothen", reeksen[0], 6, auteurs, uitgeverijen[0]));

            Assert.IsTrue(uow.Strips.GeefAlleStrips().Count() == 1, "De strip is niet toegevoegd");
        }
        [TestMethod()]
        public void VoegStripToe_OnbekendeUitgeverij_MoetFalen()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            Assert.ThrowsException<StripException>(() => uow.Strips.VoegStripToe(
                new Strip("Asterix en de Gothen", reeksen[0], 6, auteurs, new Uitgeverij("standaard"))));
        }
        [TestMethod()]
        public void VoegStripToe_OnbekendeReeks_MoetFalen()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);

            Assert.ThrowsException<SqlException>(() => uow.Strips.VoegStripToe(
                new Strip("Asterix en de Gothen", new Reeks("Asterix"), 6, auteurs, uitgeverijen[0])));
        }
        [TestMethod()]
        public void VoegStripToe_ZonderReeksEnReeksnummer_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            Strip strip = new Strip("teststrip", null, null, auteurs, uitgeverijen[0]);

            uow.Strips.VoegStripToe(strip);
            Assert.AreEqual(1, uow.Strips.GeefAlleStrips().Count());
        }

        [TestMethod()]
        public void VoegStripToe_StripMetId_MaaktNieuwId()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            Strip strip1 = new Strip(5, "Asterix en de gothen", reeksen[0], 1, auteurs, uitgeverijen[0]);
            strip1.Id = uow.Strips.VoegStripToe(strip1);

            Assert.AreEqual(1, strip1.Id);
        }
        [TestMethod()]
        public void GeefAlleStrips_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            Strip strip1 = new Strip("Asterix en de gothen", null, null, auteurs, uitgeverijen[0]);
            strip1.Id = uow.Strips.VoegStripToe(strip1);
            Strip strip2 = new Strip("Asterix en de germanen", reeksen[0], 2, auteurs, uitgeverijen[0]);
            strip2.Id = uow.Strips.VoegStripToe(strip2);
            Strip strip3 = new Strip("Asterix en de belgen", reeksen[0], 3, auteurs, uitgeverijen[0]);
            strip3.Id = uow.Strips.VoegStripToe(strip3);

            var strips = uow.Strips.GeefAlleStrips().ToArray();

            Assert.AreEqual(3, strips.Length);
            Assert.AreEqual(strip1.Titel, strips[0].Titel);
            Assert.AreEqual(3, strips[0].GeefAuteurs().Count);
            Assert.AreEqual(strip2.Titel, strips[1].Titel);
            Assert.AreEqual(strip3.Titel, strips[2].Titel);
        }

        [TestMethod()]
        public void GeefAlleStrip_LegeDatabank_ReturnsLegeLijst()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.AreEqual(0, uow.Strips.GeefAlleStrips().Count());
        }

        [TestMethod()]
        public void GeefStripViaId_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            Strip strip = new Strip("Asterix en de Gothen", reeksen[0], 6, auteurs, uitgeverijen[0]);
            strip.Id = uow.Strips.VoegStripToe(strip);

            Strip gezochteStrip = uow.Strips.GeefStripViaId(strip.Id);

            Assert.AreEqual(strip, gezochteStrip);
        }

        [TestMethod()]
        public void GeefStripViaId_IdBestaatNiet_ReturnsNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.AreEqual(null, uow.Strips.GeefStripViaId(5));
        }
        [TestMethod()]
        public void UpdateStrip_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            // strip aanmaken en toevoegen aan DB
            Strip strip = new Strip("Asterix en de Gothen", reeksen[0], 1, new List<Auteur>() { auteurs[0], auteurs[1] }, uitgeverijen[0]);
            strip.Id = uow.Strips.VoegStripToe(strip);

            // nieuwe attributen aanmaken
            string nieuweTitel = "Asterix en de germanen"; Reeks nieuweReeks = reeksen[1];
            int nieuweReeksNummer = 2; Uitgeverij nieuweUitgeverij = uitgeverijen[1];
            List<Auteur> nieuweAuteurs = new List<Auteur>() { auteurs[0], auteurs[2] };

            // nieuwe attributen instellen
            strip.Titel = nieuweTitel;
            strip.VeranderReeksEnReeksNummer(nieuweReeks, nieuweReeksNummer);
            strip.StelAuteursIn(nieuweAuteurs);
            strip.Uitgeverij = nieuweUitgeverij;

            //strip updaten
            uow.Strips.UpdateStrip(strip);

            // geupdate strip opzoeken in de DB en vegelijken
            Strip geupdateStrip = uow.Strips.GeefStripViaId(strip.Id);
            // de attributen apart opzoeken.
            //Assert.AreEqual(geupdateStrip.Titel, nieuweTitel, $"Titel is niet geupdate. Moet {nieuweTitel} zijn maar is {geupdateStrip.Titel}.");
            //Assert.AreEqual(geupdateStrip.Reeks, nieuweReeks, $"Reeks is niet geupdate. Moet {nieuweReeks} zijn maar is {geupdateStrip.Reeks}.");
            //Assert.AreEqual(geupdateStrip.ReeksNummer, nieuweReeksNummer, $"Reeksnummer is niet geupdate. Moet {nieuweReeksNummer} zijn maar is {geupdateStrip.ReeksNummer}.");
            //Assert.AreEqual(geupdateStrip.GeefAuteurs(), nieuweAuteurs, $"Auteurs zijn niet geupdate. Moet {nieuweAuteurs} zijn maar is {geupdateStrip.GeefAuteurs()}.");
            //Assert.AreEqual(geupdateStrip.Uitgeverij, nieuweUitgeverij, $"Uitgeverij is niet geupdate. Moet {nieuweUitgeverij} zijn maar is {geupdateStrip.Uitgeverij}");

            // de hele strip vergelijken
            Assert.AreEqual(geupdateStrip, strip);
        }

        [TestMethod()]
        public void UpdateStrip_ReeksEnReeksNummerNullMaken_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            // strip aanmaken en toevoegen aan DB
            Strip strip = new Strip("Asterix en de Gothen", reeksen[0], 1, new List<Auteur>() { auteurs[0], auteurs[1] }, uitgeverijen[0]);
            strip.Id = uow.Strips.VoegStripToe(strip);

            // strip aanpassen
            strip.VeranderReeksEnReeksNummer(null, null);

            // strip updaten
            uow.Strips.UpdateStrip(strip);

            // strip uit db halen en vergelijken
            Strip geupdateStrip = uow.Strips.GeefStripViaId(strip.Id);
            Assert.AreEqual(strip, geupdateStrip);
        }

        [TestMethod()]
        public void UpdateStrip_AuteurBestaatNiet_ThrowsException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            // strip aanmaken en toevoegen aan DB
            Strip strip = new Strip("Asterix en de Gothen", reeksen[0], 1, new List<Auteur>() { auteurs[0], auteurs[1] }, uitgeverijen[0]);
            strip.Id = uow.Strips.VoegStripToe(strip);

            // strip aanpassen
            strip.StelAuteursIn(new List<Auteur>() { new Auteur(9, "Testing") });

            // strip updaten
            Assert.ThrowsException<SqlException>(() => uow.Strips.UpdateStrip(strip));
        }

        [TestMethod()]
        public void UpdateStrip_UitgeverijBestaatNiet_ThrowsException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            // strip aanmaken en toevoegen aan DB
            Strip strip = new Strip("Asterix en de Gothen", reeksen[0], 1, new List<Auteur>() { auteurs[0], auteurs[1] }, uitgeverijen[0]);
            strip.Id = uow.Strips.VoegStripToe(strip);

            // strip aanpassen
            strip.Uitgeverij = new Uitgeverij(9, "testUitgeverij");

            // strip updaten
            Assert.ThrowsException<SqlException>(() => uow.Strips.UpdateStrip(strip));
        }

        [TestMethod()]
        public void UpdateStrip_ReeksBestaatNiet_ThrowsException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            List<Uitgeverij> uitgeverijen = VoegUitgeverijenToe(uow);
            List<Auteur> auteurs = VoegAuteursToe(uow);
            List<Reeks> reeksen = VoegReeksenToe(uow);

            // strip aanmaken en toevoegen aan DB
            Strip strip = new Strip("Asterix en de Gothen", reeksen[0], 1, new List<Auteur>() { auteurs[0], auteurs[1] }, uitgeverijen[0]);
            strip.Id = uow.Strips.VoegStripToe(strip);

            // strip aanpassen
            strip.VeranderReeksEnReeksNummer(new Reeks(9, "testreeks"), 5);

            // strip updaten
            Assert.ThrowsException<SqlException>(() => uow.Strips.UpdateStrip(strip));
        }
    }
}