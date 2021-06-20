using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Presentatie_laag.Interfaces;
using Presentatie_laag.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresentatieLaagTests.ViewModels
{
    [TestClass]
    public class UpdateUitgeverijViewModelTests
    {
        // Wanneer de gebruiker een uitgeverij probeert te wijzigen zonder eerst een uitgeverij te selecteren, dan moet de ShowErrorMessage-methode gecalled worden waaraan de correcte foutmelding wordt meegegeven.
        [TestMethod]
        public void UpdateUitgeverij_WanneerErGeenUitgeverijWerdGeselecteerd_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            fakeManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(new List<Uitgeverij>() { new Uitgeverij(1, "uitgeverij 1"), new Uitgeverij(2, "uitgeverij 2") });
            var fakePopupBox = new Mock<IPopupBox>();
            var vm = new UpdateUitgeverijViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.NieuweUitgeverijNaam = "nieuweTestnaam";

            vm.UpdateUitgeverijCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowErrorMessage("Gelieve een uitgeverij te selecteren."), Times.Once);
        }

        // Wanneer de gebruiker een uitgeverij heeft geselecteerd maar hij heeft de nieuwe uitgeverijnaam niet ingevuld, dan moet de ShowErrorMessage-methode gecalled worden waaraan de correcte foutmelding wordt meegegeven.
        [TestMethod]
        public void UpdateUitgeverij_LegeUitgeverijNaam_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Uitgeverij> uitgeverijen = new List<Uitgeverij>() { new Uitgeverij(1, "uitgeverij 1"), new Uitgeverij(2, "uitgeverij 2") };
            fakeManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(uitgeverijen);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateUitgeverijViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeUitgeverij = uitgeverijen[0];
            vm.NieuweUitgeverijNaam = string.Empty;

            vm.UpdateUitgeverijCommand.Execute(null);
            fakePopupBox.Verify(fake => fake.ShowErrorMessage("Naam mag niet leeg zijn."), Times.Once);
        }

        // Wanneer de gebruiker een uitgeverij heeft geselecteerd en hij heeft de nieuwe uitgeverijnaam ingevoerd, dan moet de ShowSuccesMessage-methode gecalled worden waaraan het correcte bericht wordt meegegeven.
        [TestMethod]
        public void UpdateUitgeverij_UitgeverijWerdGeselecteerdEnUitgeverijNaamWerdIngevoerd_MaaktCallNaarShowSuccesMethode()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Uitgeverij> uitgeverijen = new List<Uitgeverij>() { new Uitgeverij(1, "uitgeverij 1"), new Uitgeverij(2, "uitgeverij 2") };
            fakeManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(uitgeverijen);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateUitgeverijViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeUitgeverij = uitgeverijen[0];
            vm.NieuweUitgeverijNaam = "nieuwe naam";

            vm.UpdateUitgeverijCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De uitgeverij werd gewijzigd."), Times.Once);
        }

        // Wanneer de gebruiker een uitgeverij heeft geselecteerd en hij heeft de nieuwe uitgeverijnaam ingevoerd, dan moet de UpdateUitgeverij-methode gecalled worden waaraan een uitgeverij object wordt meegegeven met de id van de geselecteerde uitgeverij en met de nieuwe naam.
        [TestMethod]
        public void UpdateUitgeverij_UitgeverijWerdGeselecteerdEnUitgeverijNaamWerdIngevoerd_MaaktCallNaarUpdateAuteur()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Uitgeverij> uitgeverijen = new List<Uitgeverij>() { new Uitgeverij(1, "uitgeverij 1"), new Uitgeverij(2, "uitgeverij 2") };
            fakeManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(uitgeverijen);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateUitgeverijViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeUitgeverij = uitgeverijen[0];
            vm.NieuweUitgeverijNaam = "nieuwe naam";

            vm.UpdateUitgeverijCommand.Execute(null);

            fakeManager.Verify(fake => fake.UpdateUitgeverij(new Uitgeverij(uitgeverijen[0].Id, vm.NieuweUitgeverijNaam)), Times.Once);
        }


        // Telkens de domeinlaag een exception gooit, moet deze opgevangen worden door de UpdateUitgeverij-methode, waarna de message van de opgevangen message doorgegeven wordt aan de _popupBox.ShowErrorMessage-methde.
        [TestMethod]
        public void UpdateUitgeverij_WanneerDomeinlaagExceptionGooit_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Uitgeverij> uitgeverijen = new List<Uitgeverij>() { new Uitgeverij(1, "uitgeverij 1"), new Uitgeverij(2, "uitgeverij 2") };
            fakeManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(uitgeverijen);
            fakeManager.Setup(fake => fake.UpdateUitgeverij(It.IsAny<Uitgeverij>())).Throws(new DomeinException("test error message"));
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateUitgeverijViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeUitgeverij = uitgeverijen[0];
            vm.NieuweUitgeverijNaam = "uitgeverijnaam";

            vm.UpdateUitgeverijCommand.Execute(null);
            fakePopupBox.Verify(fake => fake.ShowErrorMessage("test error message"), Times.Once);
        }



    }
}
