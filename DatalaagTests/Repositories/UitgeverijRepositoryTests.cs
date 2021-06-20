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
    public class UitgeverijRepositoryTests
    {
        [TestMethod()]
        public void GeefUitgeverijViaId_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Uitgeverij u1 = new Uitgeverij("Dargaud");
            int id = uow.Uitgeverijen.VoegUitgeverijToe(u1);

            Assert.IsTrue(uow.Uitgeverijen.GeefUitgeverijViaId(id).Id == id, "De uitgeverij is niet teruggevonden");
        }

        [TestMethod()]
        public void GeefUitgeverijViaId_OnbestaandeId_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            
            Assert.AreEqual(null, uow.Uitgeverijen.GeefUitgeverijViaId(5));
        }

        [TestMethod()]
        public void GeefAlleUitgeverijen_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Uitgeverij uitgeverij1 = new Uitgeverij("Dargaud");
            uitgeverij1.Id = uow.Uitgeverijen.VoegUitgeverijToe(uitgeverij1);
            Uitgeverij uitgeverij2 = new Uitgeverij("okedan");
            uitgeverij2.Id = uow.Uitgeverijen.VoegUitgeverijToe(uitgeverij2);

            var uitgeverijen = uow.Uitgeverijen.GeefAlleUitgeverijen().ToArray();

            Assert.IsTrue(uitgeverijen.Length == 2, "uitgeverijen zijn niet correct toegevoegd");
            Assert.AreEqual(uitgeverij1, uitgeverijen[0]);
            Assert.AreEqual(uitgeverij2, uitgeverijen[1]);
        }

        [TestMethod()]
        public void GeefAlleUitgeverijen_DatabankIsLeeg_ReturntLegeLijst()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.AreEqual(0, uow.Uitgeverijen.GeefAlleUitgeverijen().Count());
        }

        [TestMethod()]
        public void VoegUitgeverijToe_EigenId_MaaktNieuweIdAan()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Uitgeverij uitgeverij = new Uitgeverij(5,"Dargaud");
            uitgeverij.Id = uow.Uitgeverijen.VoegUitgeverijToe(uitgeverij);

            Assert.AreEqual(1, uitgeverij.Id);
        }
        
        [TestMethod()]
        public void VoegUitgeverijToe_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            uow.Uitgeverijen.VoegUitgeverijToe(new Uitgeverij("CoppensCopies"));
            Assert.IsTrue(uow.Uitgeverijen.GeefUitgeverijViaNaam("CoppensCopies").Naam == "CoppensCopies",
                "De uitgeverij is niet toegevoegd of gevonden");
        }
        
        [TestMethod()]
        public void GeefUitgeverijViaNaam_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            string naam = "okedan";
            uow.Uitgeverijen.VoegUitgeverijToe(new Uitgeverij(naam));

            Assert.IsTrue(uow.Uitgeverijen.GeefUitgeverijViaNaam(naam).Naam == naam, "uitgeverij is niet gevonden");
        }

        [TestMethod]
        public void GeefUitgeverijViaNaam_CaseSensitive_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Uitgeverij uitgeverij = new Uitgeverij("Dargaud");
            uitgeverij.Id = uow.Uitgeverijen.VoegUitgeverijToe(uitgeverij);

            Assert.AreEqual(uitgeverij, uow.Uitgeverijen.GeefUitgeverijViaNaam("dargaud"));
        }

        [TestMethod()]
        public void GeefUitgeverijViaNaam_ExtraSpaties_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Uitgeverij uitgeverij = new Uitgeverij("Dargaud");
            uitgeverij.Id = uow.Uitgeverijen.VoegUitgeverijToe(uitgeverij);

            Assert.AreEqual(null, uow.Uitgeverijen.GeefUitgeverijViaNaam("    Dargaud   "));
        }

        [TestMethod()]
        public void GeefUitgeverijViaNaam_UitgeverijBestaatNiet_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.AreEqual(null, uow.Uitgeverijen.GeefUitgeverijViaNaam("Dargaud"));
        }

        [TestMethod()]
        public void GeefUitgeverijViaNaam_UitgeverijIsNull_ThrowsSqlException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.ThrowsException<SqlException>(() => uow.Uitgeverijen.GeefUitgeverijViaNaam(null));
        }

        [TestMethod()]
        public void GeefUitgeverijViaNaam_UitgeverijNaamIsLeeg_ReturntNull()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.AreEqual(null, uow.Uitgeverijen.GeefUitgeverijViaNaam(""));
        }

        [TestMethod()]
        public void UpdateUitgeverij_MoetLukken()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Uitgeverij u1 = new Uitgeverij("Dargaud");
            int uId = uow.Uitgeverijen.VoegUitgeverijToe(u1);
            u1.Id = uId;
            string nieuweNaam = "veranderd";
            u1.Naam = nieuweNaam;
            uow.Uitgeverijen.UpdateUitgeverij(u1);

            Assert.IsTrue(uow.Uitgeverijen.GeefUitgeverijViaId(u1.Id).Naam == nieuweNaam,
                $"De uitgeverij is niet veranderd Naam moet {nieuweNaam} zijn maar is {u1.Naam}");
        }

        [TestMethod()]
        public void UpdateUitgeverij_UitgeverijIsNull_ThrowsNullReferenceException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.ThrowsException<NullReferenceException>(() => uow.Uitgeverijen.UpdateUitgeverij(null));
        }

        [TestMethod()]
        public void UpdateUitgeverij_UitgeverijBestaatNiet_ThrowsDataLayerException()
        {
            UnitOfWorkTest uow = new UnitOfWorkTest();
            Assert.ThrowsException<DataLayerException>(() => uow.Uitgeverijen.UpdateUitgeverij(new Uitgeverij(1, "Dargaud")));
        }
    }
}