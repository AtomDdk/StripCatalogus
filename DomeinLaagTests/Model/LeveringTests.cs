using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using Domeinlaag.Exceptions;
using System.Text;
using System.Linq;

namespace ModelTests.Tests
{
    [TestClass()]
    public class LeveringTests
    {
        private Dictionary<Strip,int> GeefStripsEnAantallen(int aantal=3)
        {
            Dictionary<Strip, int> result = new Dictionary<Strip, int>();
            Auteur auteur = new Auteur(1,"testAuteur");
            List<Auteur> auteurs = new List<Auteur>() { auteur };
            Uitgeverij uitgeverij = new Uitgeverij(1,"TestUitgeverij");
            for(int i = 0; i < aantal; i++)
            {
                Strip strip = new Strip(1,"testTitel" + i, null, null, auteurs, uitgeverij);
                result.Add(strip, i+1);
            }


            return result;
        }

        [TestMethod()]
        public void LeveringConstructorTest_PropertiesCorrectGebonden()
        {
            var aantallen = GeefStripsEnAantallen();
            DateTime bestel = new DateTime(2020, 12, 5);
            DateTime lever = new DateTime(2020, 12, 8);
            Levering levering = new Levering(aantallen, bestel, lever);

            Assert.IsTrue(levering.BestelDatum.Equals(bestel));
            Assert.IsTrue(levering.LeverDatum.Equals(lever));
            Assert.IsTrue(levering.GeefStripsEnAantallen().SequenceEqual(aantallen));
        }
        [TestMethod()]
        [ExpectedException(typeof(LeveringException))]
        public void LeveringConstructorTest_LeverDatumInDeToekomst_MoetLeveringExceptionGooien()
        {
            var aantallen = GeefStripsEnAantallen();
            DateTime bestel = new DateTime(2020, 12, 5);
            DateTime lever = new DateTime(3020, 12, 8);
            Levering levering = new Levering(aantallen, bestel, lever);
        }
        [TestMethod()]
        [ExpectedException(typeof(LeveringException))]
        public void LeveringConstructorTest_LeverDatumNaBestelDatum_MoetLeveringExceptionGooien()
        {
            var aantallen = GeefStripsEnAantallen();
            DateTime bestel = new DateTime(2020, 12, 6);
            DateTime lever = new DateTime(2020, 12, 5);
            Levering levering = new Levering(aantallen, bestel, lever);
        }
        [TestMethod()]
        public void LeveringWordtAangemaakt_StripAantallenWordenNietAangepast()
        {
            var aantallen = GeefStripsEnAantallen(3);
            DateTime bestel = new DateTime(2020, 12, 5);
            DateTime lever = new DateTime(2020, 12, 8);
            Levering levering = new Levering(aantallen, bestel, lever);

            List<Strip> strips = levering.GeefStrips();

            Assert.IsTrue(strips[0].Aantal == 0,"Het aantal van de eerste strip werd toch aangepast.");
            Assert.IsTrue(strips[1].Aantal == 0, "Het aantal van de tweede strip werd toch aangepast.");
            Assert.IsTrue(strips[2].Aantal == 0, "Het aantal van de derde strip werd toch aangepast.");

        }
    }
}