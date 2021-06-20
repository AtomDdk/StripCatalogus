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
    public class AuteurRepositoryTests
    {
        [TestMethod()] 
        public void VoegAuteurToe_MetEigenIdMoetCorrect_MoetNieuweIdAlsIdentityMaken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Auteur auteur = new Auteur(5, "Bob");
            uow.Auteurs.VoegAuteurToe(auteur);
            Auteur auteurDb = uow.Auteurs.GeefAuteurViaNaam(auteur.Naam);

            Assert.AreEqual(auteurDb.Id, 1);
        }
        [TestMethod()]
        public void VoegAuteurToe_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Auteurs.VoegAuteurToe(new Auteur("SinisterSigi"));
            Assert.IsTrue(uow.Auteurs.GeefAuteurViaNaam("SinisterSigi").Naam == "SinisterSigi",
                "De auteur is niet toegevoegd of gevonden.");
        }

        [TestMethod()]
        public void VoegAuteurToe_ZelfdeNaam_MoetSqlExceptionGooien()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Auteurs.VoegAuteurToe(new Auteur("Bob"));
            Assert.ThrowsException<SqlException>(() => uow.Auteurs.VoegAuteurToe(new Auteur("Bob")),
                "De auteur is toch toegevoegd ookal zit er een auteur in met dezelfde naam");
        }
        [TestMethod()]
        public void GeefAuteurViaId_OnbestaandeId_MoetNullReturnen()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.AreEqual(uow.Auteurs.GeefAuteurViaId(2), null);
        }

        [TestMethod()]
        public void GeefAuteurViaId_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            string naam = "Bob";
            int auteurId = uow.Auteurs.VoegAuteurToe(new Auteur(naam));

            Assert.AreEqual(uow.Auteurs.GeefAuteurViaId(auteurId).Naam, naam);
        }

        [TestMethod()]
        public void GeefAlleAuteurs_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Auteurs.VoegAuteurToe(new Auteur("Test 1"));
            uow.Auteurs.VoegAuteurToe(new Auteur("Test 2"));

            var auteurs = uow.Auteurs.GeefAlleAuteurs().ToArray();
            Assert.IsTrue(auteurs.Length == 2, "Niet alle auteurs zijn toegevoegd");
            Assert.AreEqual(auteurs[0].Naam, "Test 1");
            Assert.AreEqual(auteurs[1].Naam, "Test 2");
        }

        [TestMethod()]
        public void GeefAlleAuteurs_AlsDatabankLeegIs_MoetLegeLijstReturnen()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            var auteurs = uow.Auteurs.GeefAlleAuteurs().ToArray();
            Assert.IsTrue(auteurs.Length == 0);
        }



        [TestMethod()]
        public void GeefAuteurViaNaam_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            string naam = "Bob";
            uow.Auteurs.VoegAuteurToe(new Auteur(naam));

            Assert.AreEqual(uow.Auteurs.GeefAuteurViaNaam(naam).Naam, naam);
        }

        [TestMethod()]
        public void GeefAuteurViaNaam_CaseSensitive_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Auteur auteur = new Auteur("Bob");
            auteur.Id = uow.Auteurs.VoegAuteurToe(auteur);

            Assert.AreEqual(uow.Auteurs.GeefAuteurViaNaam("bob"), auteur);
        }

        [TestMethod()]
        public void GeefAuteurViaNaam_ExtraSpaties_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Auteurs.VoegAuteurToe(new Auteur("Bob"));

            Assert.AreEqual(uow.Auteurs.GeefAuteurViaNaam("    Bob    "), null);
        }

        [TestMethod()]
        public void GeefAuteurViaNaam_AuteurBestaatNiet_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Auteurs.VoegAuteurToe(new Auteur("Bob"));

            Assert.AreEqual(uow.Auteurs.GeefAuteurViaNaam("bib"), null);
        }

        [TestMethod()]
        public void GeefAuteurViaNaam_NaamIsNull_ThrowsSqlException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Auteurs.VoegAuteurToe(new Auteur("Bob"));

            Assert.ThrowsException<SqlException>(() => uow.Auteurs.GeefAuteurViaNaam(null));
        }

        [TestMethod()]
        public void GeefAuteurViaNaam_NaamIsLegeString_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Auteurs.VoegAuteurToe(new Auteur("Bob"));

            Assert.AreEqual(uow.Auteurs.GeefAuteurViaNaam(""), null);
        }

        [TestMethod()]
        public void UpdateAuteur_MoetLukken() 
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            // auteur aanmaken en toevoegen
            Auteur auteur = new Auteur("Bob");
            auteur.Id = uow.Auteurs.VoegAuteurToe(auteur);
            
            // auteursNaam veranderen en updaten
            string nieuweNaam = "veranderd";
            auteur.Naam = nieuweNaam;
            uow.Auteurs.UpdateAuteur(auteur);

            // nieuwe auteur opzoeken en vergelijken
            Auteur geupdateAuteur = uow.Auteurs.GeefAuteurViaId(auteur.Id);
            Assert.AreEqual(auteur, geupdateAuteur, $"De auteur is niet geupdate. Moet {auteur} zijn maar is {geupdateAuteur}.");
        }

        [TestMethod()]
        public void UpdateAuteur_AuteurIsNull_ThrowsSqlException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.ThrowsException<NullReferenceException>(() => uow.Auteurs.UpdateAuteur(null));
        }

        [TestMethod()]
        public void UpdateAuteur_AuteurBestaatNiet_ThrowsDataLayerException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();

            Assert.ThrowsException<DataLayerException>(() => uow.Auteurs.UpdateAuteur(new Auteur(1, "Bob")));
        }
    }
}
