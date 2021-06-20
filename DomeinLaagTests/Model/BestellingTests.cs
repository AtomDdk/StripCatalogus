using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Domeinlaag.Exceptions;

namespace ModelTests.Tests
{
    [TestClass()]
    public class BestellingTests
    {
        private Dictionary<Strip, int> GeefStripsEnAantallenMetVoldoendeVoorraad(int aantal = 3)
        {
            Dictionary<Strip, int> result = new Dictionary<Strip, int>();
            Auteur auteur = new Auteur(1, "testAuteur");
            List<Auteur> auteurs = new List<Auteur>() { auteur };
            Uitgeverij uitgeverij = new Uitgeverij(1, "TestUitgeverij");
            for (int i = 0; i < aantal; i++)
            {
                Strip strip = new Strip(1, "testTitel" + i, null, null, auteurs, uitgeverij);
                strip.Aantal = 100;
                result.Add(strip, i + 1);
            }


            return result;
        }
        private Dictionary<Strip, int> GeefStripsEnAantallenMetOnVoldoendeVoorraad(int aantal = 3)
        {
            Dictionary<Strip, int> result = new Dictionary<Strip, int>();
            Auteur auteur = new Auteur(1, "testAuteur");
            List<Auteur> auteurs = new List<Auteur>() { auteur };
            Uitgeverij uitgeverij = new Uitgeverij(1, "TestUitgeverij");
            for (int i = 0; i < aantal; i++)
            {
                Strip strip = new Strip(1, "testTitel" + i, null, null, auteurs, uitgeverij);
                result.Add(strip, i + 1);
            }


            return result;
        }
        [TestMethod()]
        public void BestellingConstructorTest_PropertiesCorrectGebonden()
        {
            var aantallen = GeefStripsEnAantallenMetVoldoendeVoorraad();
            DateTime datum = new DateTime(2020, 12, 5);
            Bestelling bestelling = new Bestelling(aantallen, datum);

            Assert.IsTrue(bestelling.Datum.Equals(datum));
            Assert.IsTrue(bestelling.GeefStripsEnAantallen().SequenceEqual(aantallen));
        }
        [TestMethod()]
        public void BestellingConstructorTest_VoorraadVanStripsCorrectAangepast()
        {
            var aantallen = GeefStripsEnAantallenMetVoldoendeVoorraad(3);
            DateTime datum = new DateTime(2020, 12, 5);
            Bestelling bestelling = new Bestelling(aantallen, datum);

            List<Strip> strips = bestelling.GeefStrips();

            Assert.IsTrue(strips[0].Aantal == 100);
            Assert.IsTrue(strips[1].Aantal == 100);
            Assert.IsTrue(strips[2].Aantal == 100);

        }
        [TestMethod()]

        [ExpectedException(typeof(BestellingException))]
        public void BestellingConstructorTest_DatumInDeToekomst_WerptBestellingException()
        {
            var aantallen = GeefStripsEnAantallenMetVoldoendeVoorraad();
            DateTime datum = new DateTime(3020, 12, 5);
            Bestelling bestelling = new Bestelling(aantallen, datum);
        }
        [TestMethod()]
        public void BestellingConstructorTest_VoorraadVanStripsIsOntoereikend_KanTochWordenAangemaakt()
        {
            var aantallen = GeefStripsEnAantallenMetOnVoldoendeVoorraad(3);
            DateTime datum = new DateTime(2020, 12, 5);
            Bestelling bestelling = new Bestelling(aantallen, datum);

            var result = bestelling.GeefStripsEnAantallen();
            foreach(KeyValuePair<Strip,int> pair in result)
            {
                Assert.IsTrue(pair.Key.Aantal<pair.Value,"Het aantal was niet kleiner, dus de test kon niet correct verlopen.");
            }
            
        }
    }
}