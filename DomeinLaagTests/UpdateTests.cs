using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag;
using System;
using System.Collections.Generic;
using System.Text;
using IntegratieTests;
using Domeinlaag.Model;
using System.Linq;
using Domeinlaag.Exceptions;

namespace UpdateTests.Tests
{
    [TestClass()]
    public class UpdateTests
    {
        private void VoegInitieleTestStripToe(CatalogusManager manager)
        {
            string titel = "Asterix en de testen";
            int nummer = 1;
            string reeksNaam = "Asterix";
            List<string> auteursNamen = new List<string> { "Kegelman" };
            string uitgeverijNaam = "CoppensTraitors";

            Reeks reeks = manager.VoegReeksToe(reeksNaam);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(manager.VoegAuteurToe(auteursNamen[0]));
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe(uitgeverijNaam);

            manager.VoegStripToe(titel, reeks, nummer, auteurs, uitgeverij);
        }
        private void VoegTweedeTestStripToeUitAndereReeks(CatalogusManager manager)
        {
            string titel = "TestTitel2";
            int nummer = 2;
            string reeksNaam = "TestReeks2";
            List<string> auteursNamen = new List<string> { "TestAuteur2" };
            string uitgeverijNaam = "TestUitgeverij2";

            Reeks reeks = manager.VoegReeksToe(reeksNaam);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(manager.VoegAuteurToe(auteursNamen[0]));
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe(uitgeverijNaam);

            manager.VoegStripToe(titel, reeks, nummer, auteurs, uitgeverij);
        }
        private void VoegDerdeTestStripToeUitZelfdeReeks(CatalogusManager manager)
        {
            string titel = "TestStrip3";
            int nummer = 3;
            Reeks reeks = manager.GeefReeksViaId(1);
            string reeksNaam = "Asterix";

            //dit nog moeten aanpassen somehow
            if(reeks.Naam!=reeksNaam)
                reeks = manager.VoegReeksToe(reeksNaam);

            List<string> auteursNamen = new List<string> { "Kegelman" };
            Auteur auteur = manager.GeefAuteurViaId(1);
            List<Auteur> auteurs = new List<Auteur>();
            if (auteur.Naam != auteursNamen[0])
                auteurs.Add(manager.VoegAuteurToe(auteursNamen[0]));
            else
                auteurs.Add(auteur);

            Uitgeverij uitgeverij = manager.GeefUitgeverijViaId(1);
            string uitgeverijNaam = "CoppensTraitors";
            if(uitgeverij.Naam!=uitgeverijNaam)
                uitgeverij = manager.VoegUitgeverijToe(uitgeverijNaam);

            //tot hier
            manager.VoegStripToe(titel, reeks, nummer, auteurs, uitgeverij);
        }
        private void VoegVierdeTestStripToeMetMeerdereAuteurs(CatalogusManager manager)
        {
            string titel = "TestStrip4";
            int nummer = 4;
            string reeksNaam = "TestReeks4";
            List<string> auteursNamen = new List<string> { "TestAuteur3", "TestAuteur4" };
            string uitgeverijNaam = "TestUitgeverij3";

            Reeks reeks = manager.VoegReeksToe(reeksNaam);
            List<Auteur> auteurs = new List<Auteur>();
            foreach(string naam in auteursNamen)
            {
                auteurs.Add(manager.VoegAuteurToe(naam));
            }
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe(uitgeverijNaam);

            manager.VoegStripToe(titel, reeks, nummer, auteurs, uitgeverij);
        }
        private void VoegStripToeZonderReeksEnReeksNummer(CatalogusManager manager)
        {
            string titel = "StripZonderReeksEnReeksNmmer";
            List<string> auteursNamen = new List<string> { "reekslozeAuteur" };
            string uitgeverijNaam = "TestUitgeverij4";
            int? nummer = null;

            Reeks reeks = null;
            List<Auteur> auteurs = new List<Auteur>();
            foreach (string naam in auteursNamen)
            {
                auteurs.Add(manager.VoegAuteurToe(naam));
            }
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe(uitgeverijNaam);

            manager.VoegStripToe(titel, reeks, nummer, auteurs, uitgeverij);
        }
        private void VoegVijfdeTestStripToeMetZelfdeReeksNummer(CatalogusManager manager)
        {
            string titel = "TestStrip5";
            int nummer = 1;
            string reeksNaam = "TestReeks5";
            List<string> auteursNamen = new List<string> { "TestAuteur5"};
            string uitgeverijNaam = "TestUitgeverij5";

            Reeks reeks = manager.VoegReeksToe(reeksNaam);
            List<Auteur> auteurs = new List<Auteur>();
            foreach (string naam in auteursNamen)
            {
                auteurs.Add(manager.VoegAuteurToe(naam));
            }
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe(uitgeverijNaam);

            manager.VoegStripToe(titel, reeks, nummer, auteurs, uitgeverij);
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S1x1xUpdateStrip_ZonderIetsAanTePassen_GooitCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        public void S1x2x1xUpdateStrip_TitelEnReeksNummerAanpassen_MoetLukken()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            string nieuweTitel = "veranderdeWaarde";
            int nieuweWaarde = 123456;
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.Titel = nieuweTitel;
            strip.VeranderReeksEnReeksNummer(strip.Reeks, nieuweWaarde);
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 1, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == nieuweTitel, "De Titel is niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.ReeksNummer == nieuweWaarde, "Het ReeksNummer is niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(strip.Reeks), "De reeks kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }

        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void S1x2x2xUpdateStrip_TitelLeegMaken_GooitStripException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            string nieuweTitel = "";
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.Titel = nieuweTitel;
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void S1x2x3xUpdateStrip_TitelNullMaken_GooitStripException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            string nieuweTitel = null;
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.Titel = nieuweTitel;
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void S1x2x4xUpdateStrip_ReeksNummerNegatiefMaken_GooitStripException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            int ReeksNummer = -5;
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.VeranderReeksEnReeksNummer(strip.Reeks,ReeksNummer);
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void S1x2x5xUpdateStrip_ReeksNummerVerwijderenZonderReeksTeVerwijderen_GooitStripException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            int? ReeksNummer = null;
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.VeranderReeksEnReeksNummer(strip.Reeks, ReeksNummer);
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        public void S1x2x6xUpdateStrip_ReeksEnReeksNummerVerwijderen_MoetLukken()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.VeranderReeksEnReeksNummer(null, null);
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 1, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == strip.Titel, "De Titel kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.ReeksNummer == strip.ReeksNummer, "Het ReeksNummer kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Reeks==null, "De reeks kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }

        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void S1x2x7xUpdateStrip_ReeksVerwijderenZonderReeksNummerTeVerwijderen_GooitStripException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.VeranderReeksEnReeksNummer(null, strip.ReeksNummer);
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        public void S1x2x8xUpdateStrip_VeranderTitelNaarTitelVanStripUitAndereReeks_VerlooptCorrect()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            VoegTweedeTestStripToeUitAndereReeks(manager);

            Strip strip = manager.GeefStripViaId(1);
            Strip tweedeStrip = manager.GeefStripViaId(2);
            strip.Titel = tweedeStrip.Titel;
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 2, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == tweedeStrip.Titel, "De Titel werd niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.ReeksNummer == strip.ReeksNummer, "Het ReeksNummer kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(strip.Reeks), "De reeks kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S1x2x9xUpdateStrip_VeranderReeksNummerNaarEenReedsBestaandUitDezelfdeReeks_GooitCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            VoegDerdeTestStripToeUitZelfdeReeks(manager);

            Strip strip = manager.GeefStripViaId(1);
            Strip tweedeStrip = manager.GeefStripViaId(2);
            strip.VeranderReeksEnReeksNummer(strip.Reeks,tweedeStrip.ReeksNummer);
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        public void S1x2x10xUpdateStrip_VeranderReeksNummerNaarEenReedsBestaandUitEenAndereReeks_VerlooptCorrect()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            VoegTweedeTestStripToeUitAndereReeks(manager);

            Strip strip = manager.GeefStripViaId(1);
            Strip tweedeStrip = manager.GeefStripViaId(2);
            strip.VeranderReeksEnReeksNummer(strip.Reeks, tweedeStrip.ReeksNummer);
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 2, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == strip.Titel, "De Titel kwam niet meer overeen");
            Assert.IsTrue(nieuweStrip.ReeksNummer == tweedeStrip.ReeksNummer, "Het ReeksNummer werd niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(strip.Reeks), "De reeks kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }

        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S1x2x11xUpdateStrip_VerwijderReeksEnReeksNummer_ZitAlInDatabank_GooitCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegStripToeZonderReeksEnReeksNummer(manager);
            Strip initieleStrip = manager.GeefStripViaId(1);

            Reeks reeks = manager.VoegReeksToe("extraUitgeverij");

            Strip nieuweStrip = manager.VoegStripToe(initieleStrip.Titel, reeks, 3, initieleStrip.GeefAuteurs().ToList(), initieleStrip.Uitgeverij);
            nieuweStrip.VeranderReeksEnReeksNummer(null, null);

            manager.UpdateStrip(nieuweStrip);
        }
        [TestMethod()]
        public void S1x2x12xUpdateStrip_StripZonderReeksEnReeksNummer_PasAuteursEnUitgeverijAan()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            Strip temp = manager.GeefStripViaId(1);

            manager.VoegStripToe("TestTitelZonderReeks", null, null, temp.GeefAuteurs().ToList(), temp.Uitgeverij);
            Strip strip = manager.GeefStripViaId(2);
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe("nieuweUitgeverij");
            Auteur auteur = manager.VoegAuteurToe("nieuweAuteur");
            List<Auteur> auteurs = new List<Auteur> { auteur };
            strip.Uitgeverij = uitgeverij;
            strip.StelAuteursIn(auteurs);
            strip.Titel = "nieuweTestTitel";

            manager.UpdateStrip(strip);
            Strip result = manager.GeefStripViaId(2);

            Assert.IsTrue(result.Equals(strip), "De verwachte strip en de toegevoegde strip waren niet gelijk.");
        }

        [TestMethod()]
        public void S1x3x1xUpdateStrip_VeranderAuteur_VerlooptCorrect()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            VoegTweedeTestStripToeUitAndereReeks(manager);

            Strip strip = manager.GeefStripViaId(1);
            Strip tweedeStrip = manager.GeefStripViaId(2);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(tweedeStrip.GeefAuteurs()[0]);
            strip.StelAuteursIn(auteurs);
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 2, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == strip.Titel, "De Titel kwam niet meer overeen");
            Assert.IsTrue(nieuweStrip.ReeksNummer == strip.ReeksNummer, "Het ReeksNummer was niet meer correct.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(tweedeStrip.GeefAuteurs()[0]), "De Auteurs waren niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(strip.Reeks), "De reeks kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S1x3x2xUpdateStrip_VeranderAuteurNaarAuteurDieNogNietInDeDatbankZit_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);

            Strip strip = manager.GeefStripViaId(1);
            List<Auteur> auteurs = new List<Auteur>();
            auteurs.Add(new Auteur(1,"OnbestaandeAuteur"));
            strip.StelAuteursIn(auteurs);
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S1x3x3xUpdateStrip_VoegAuteurToeDieNogNietInDeDatbankZit_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);

            Strip strip = manager.GeefStripViaId(1);
            strip.VoegAuteurToe(new Auteur(1, "OnbestaandeAuteur"));
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        public void S1x3x4xUpdateStrip_VoegAuteurToeDieAlInDeDatabankZit_VerlooptCorrect()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            VoegTweedeTestStripToeUitAndereReeks(manager);

            Strip strip = manager.GeefStripViaId(1);
            Strip tweedeStrip = manager.GeefStripViaId(2);
            strip.VoegAuteurToe(tweedeStrip.GeefAuteurs()[0]);
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 2, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == strip.Titel, "De Titel kwam niet meer overeen");
            Assert.IsTrue(nieuweStrip.ReeksNummer == strip.ReeksNummer, "Het ReeksNummer was niet meer correct.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs().Count == 2, "Het aantal auteurs is niet correct.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs waren niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[1].Equals(tweedeStrip.GeefAuteurs()[0]), "De Auteurs waren niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(strip.Reeks), "De reeks kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void S1x3x5xUpdateStrip_AuteursLeegmaken_WerptStripException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);

            Strip strip = manager.GeefStripViaId(1);
            strip.StelAuteursIn(new List<Auteur>());
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        public void S1x3x6xUpdateStrip_EenVanDeAuteursVerwijderen_VerlooptCorrect()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegVierdeTestStripToeMetMeerdereAuteurs(manager);
            
            Strip strip = manager.GeefStripViaId(1);
            strip.VerwijderAuteur(strip.GeefAuteurs()[0]);
            manager.UpdateStrip(strip);
            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 1, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == strip.Titel, "De Titel kwam niet meer overeen");
            Assert.IsTrue(nieuweStrip.ReeksNummer == strip.ReeksNummer, "Het ReeksNummer was niet meer correct.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs().Count == 1, "Het aantal auteurs is niet correct.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs waren niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(strip.Reeks), "De reeks kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }
        [TestMethod()]
        public void S1x4x1xUpdateStrip_ReeksAanpassen_VerlooptCorrect()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            VoegTweedeTestStripToeUitAndereReeks(manager);

            Strip strip = manager.GeefStripViaId(1);
            Strip tweedeStrip = manager.GeefStripViaId(2);
            strip.VeranderReeksEnReeksNummer(tweedeStrip.Reeks, strip.ReeksNummer);
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleStrips().Count == 2, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == strip.Titel, "De Titel kwam niet meer overeen");
            Assert.IsTrue(nieuweStrip.ReeksNummer == strip.ReeksNummer, "Het ReeksNummer was niet meer correct.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs waren niet meer correct.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(tweedeStrip.Reeks), "De reeks werd niet correct aangepast.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(strip.Uitgeverij), "De Uitgeverij kwam niet meer overeen.");
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S1x4x2xUpdateStrip_ReeksAanpassenNaarAndereReeksMetReedsBestaandReeksNummer_GooitCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            VoegInitieleTestStripToe(manager);
            VoegVijfdeTestStripToeMetZelfdeReeksNummer(manager);

            Strip strip = manager.GeefStripViaId(1);
            Strip tweedeStrip = manager.GeefStripViaId(2);
            strip.VeranderReeksEnReeksNummer(tweedeStrip.Reeks, strip.ReeksNummer);
            manager.UpdateStrip(strip);
        }
        [TestMethod()]
        public void S1x4x5xUpdateStrip_StripZonderReeksEenReeksEnReeksNummerGeven()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());
            Uitgeverij uitgeverij = manager.VoegUitgeverijToe("TestUitgeverij");
            Auteur auteur = manager.VoegAuteurToe("TestAuteur");
            List<Auteur> auteurs = new List<Auteur> { auteur };
            Reeks reeks = manager.VoegReeksToe("TestReeks");

            Strip strip = manager.VoegStripToe("TestStrip1", null, null, auteurs, uitgeverij);
            strip.VeranderReeksEnReeksNummer(reeks, 3);
            manager.UpdateStrip(strip);

            Strip result = manager.GeefStripViaId(1);
            Assert.IsTrue(result.Equals(strip),"De strips waren niet equal.");
        }
        [TestMethod()]
        public void S1x5x1xUpdateStrip_UitgeverijAanpassen_MoetLukken()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            Uitgeverij nieuweUitgeverij = manager.VoegUitgeverijToe("nieuweUitgeverij"); ;
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.Uitgeverij = nieuweUitgeverij;
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);

            Assert.IsTrue(manager.GeefAlleUitgeverijen().Count == 2);
            Assert.IsTrue(manager.GeefAlleStrips().Count == 1, "Het aantal strips in de databank klopt niet.");
            Assert.IsTrue(nieuweStrip.Id == strip.Id, "De Id kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Titel == strip.Titel, "De Titel kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.ReeksNummer == strip.ReeksNummer, "Het ReeksNummer kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.GeefAuteurs()[0].Equals(strip.GeefAuteurs()[0]), "De Auteurs kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Reeks.Equals(strip.Reeks), "De Uitgeverij kwam niet meer overeen.");
            Assert.IsTrue(nieuweStrip.Uitgeverij.Equals(nieuweUitgeverij), "De Uitgeverij is niet correct aangepast.");
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S1x5x2xUpdateStrip_UitgeverijAanpassenNaarOnbestaandeUitgeverij_MoetCatalogusExceptionGooien()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            Uitgeverij nieuweUitgeverij = new Uitgeverij(1,"OnbestaandeUitgeverij");
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.Uitgeverij = nieuweUitgeverij;
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);
        }
        [TestMethod()]
        [ExpectedException(typeof(StripException))]
        public void S1x5x3xUpdateStrip_UitgeverijVerwijderen_MoetStripExceptionGooien()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            Uitgeverij nieuweUitgeverij = null;
            VoegInitieleTestStripToe(manager);
            Strip strip = manager.GeefStripViaId(1);
            strip.Uitgeverij = nieuweUitgeverij;
            manager.UpdateStrip(strip);

            Strip nieuweStrip = manager.GeefStripViaId(1);
        }
        [TestMethod()]
        public void S2x1xUpdateAuteur_MoetLukken()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegAuteurToe("TestAuteur");
            Auteur auteur1 = manager.GeefAuteurViaId(1);
            auteur1.Naam = "NieuweNaam";
            manager.UpdateAuteur(auteur1);

            var x = manager.GeefAlleAuteurs();
            Auteur nieuweAuteur = x[0];

            Assert.IsTrue(x.Count == 1, "Het aantal Auteurs in de databank klopt niet.");
            Assert.IsTrue(nieuweAuteur.Naam == auteur1.Naam, "De naam werd niet correct aangepast.");
            Assert.IsTrue(auteur1.Equals(nieuweAuteur), "De Auteurs kwamen niet correct overeen.");
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S2x2xUpdateAuteur_ZonderIetsAanTePassen_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegAuteurToe("TestAuteur");
            Auteur auteur1 = manager.GeefAuteurViaId(1);
            manager.UpdateAuteur(auteur1);
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S2x3xUpdateAuteur_VeranderNaamNaarEenAuteurNaamDieAlInDeDatabankZit_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegAuteurToe("TestAuteur");
            manager.VoegAuteurToe("TweedeAuteur");
            Auteur auteur1 = manager.GeefAuteurViaId(1);
            Auteur auteur2 = manager.GeefAuteurViaId(2);
            auteur1.Naam = auteur2.Naam;
            manager.UpdateAuteur(auteur1);
        }
        //Uitgeverij
        [TestMethod()]
        public void S3x1xUpdateUitgeverij_MoetLukken()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegUitgeverijToe("TestUitgeverij");
            Uitgeverij uitgeverij1 = manager.GeefUitgeverijViaId(1);
            uitgeverij1.Naam = "NieuweNaam";
            manager.UpdateUitgeverij(uitgeverij1);

            var x = manager.GeefAlleUitgeverijen();
            Uitgeverij nieuweUitgeverij = x[0];

            Assert.IsTrue(x.Count == 1, "Het aantal Uitgeverijen in de databank klopt niet.");
            Assert.IsTrue(nieuweUitgeverij.Naam == uitgeverij1.Naam, "De naam werd niet correct aangepast.");
            Assert.IsTrue(uitgeverij1.Equals(nieuweUitgeverij), "De Uitgeverijen kwamen niet correct overeen.");
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S3x2xUpdateUitgeverij_ZonderIetsAanTePassen_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegUitgeverijToe("TestUitgeverij");
            Uitgeverij uitgeverij1 = manager.GeefUitgeverijViaId(1);
            manager.UpdateUitgeverij(uitgeverij1);
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S3x3xUpdateUitgeverij_VeranderNaamNaarEenUitgeverijNaamDieAlInDeDatabankZit_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegUitgeverijToe("TestUitgeverij");
            manager.VoegUitgeverijToe("TweedeUitgeverij");
            Uitgeverij uitgeverij1 = manager.GeefUitgeverijViaId(1);
            Uitgeverij uitgeverij2 = manager.GeefUitgeverijViaId(2);
            uitgeverij1.Naam = uitgeverij2.Naam;
            manager.UpdateUitgeverij(uitgeverij1);
        }
        //reeks
        [TestMethod()]
        public void S4x1xUpdateReeks_MoetLukken()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegReeksToe("TestReeks");
            Reeks reeks1 = manager.GeefReeksViaId(1);
            reeks1.Naam = "NieuweNaam";
            manager.UpdateReeks(reeks1);

            var x = manager.GeefAlleReeksen();
            Reeks nieuweReeks = x[0];

            Assert.IsTrue(x.Count == 1, "Het aantal Reeksen in de databank klopt niet.");
            Assert.IsTrue(nieuweReeks.Naam == reeks1.Naam, "De naam werd niet correct aangepast.");
            Assert.IsTrue(reeks1.Equals(nieuweReeks), "De Uitgeverijen kwamen niet correct overeen.");
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S4x2xUpdateReeks_ZonderIetsAanTePassen_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegReeksToe("TestReeks");
            Reeks reeks1 = manager.GeefReeksViaId(1);
            manager.UpdateReeks(reeks1);
        }
        [TestMethod()]
        [ExpectedException(typeof(CatalogusException))]
        public void S4x3xUpdateReeks_VeranderNaamNaarEenUitgeverijNaamDieAlInDeDatabankZit_WerptCatalogusException()
        {
            CatalogusManager manager = new CatalogusManager(new UnitOfWorkTest());

            manager.VoegReeksToe("TestReeks");
            manager.VoegReeksToe("NieuweReeks");
            Reeks reeks1 = manager.GeefReeksViaId(1);
            Reeks reeks2 = manager.GeefReeksViaId(2);
            reeks1.Naam = reeks2.Naam;
            manager.UpdateReeks(reeks1);
        }
    }
}