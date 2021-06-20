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
    public class UpdateReeksViewModelTests
    {
        // Wanneer de gebruiker een reeks probeert te wijzigen zonder eerst een reeks te selecteren, dan moet de ShowErrorMessage-methode gecalled worden waaraan de correcte foutmelding wordt meegegeven.
        [TestMethod]
        public void UpdateReeks_WanneerErGeenReeksWerdGeselecteerd_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            fakeManager.Setup(fake => fake.GeefAlleReeksen()).Returns(new List<Reeks>() { new Reeks(1, "reeks 1"), new Reeks(2, "reeks  2") });
            var fakePopupBox = new Mock<IPopupBox>();
            var vm = new UpdateReeksViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.NieuweReeksNaam= "nieuweTestnaam";

            vm.UpdateReeksCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowErrorMessage("Gelieve een reeks te selecteren."), Times.Once);
        }

        // Wanneer de gebruiker een reeks heeft geselecteerd maar hij heeft de nieuwe reeksnaam niet ingevuld, dan moet de ShowErrorMessage-methode gecalled worden waaraan de correcte foutmelding wordt meegegeven.
        [TestMethod]
        public void UpdateReeks_LegeReeksNaam_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Reeks> reeksen = new List<Reeks>() { new Reeks(1, "reeks 1"), new Reeks(2, "reeks 2") };
            fakeManager.Setup(fake => fake.GeefAlleReeksen()).Returns(reeksen);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateReeksViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeReeks = reeksen[0];
            vm.NieuweReeksNaam = string.Empty;

            vm.UpdateReeksCommand.Execute(null);
            fakePopupBox.Verify(fake => fake.ShowErrorMessage("De Naam van de reeks mag niet leeg zijn."), Times.Once);
        }

        // Wanneer de gebruiker een reeks heeft geselecteerd en hij heeft de nieuwe reeksnaam ingevoerd, dan moet de ShowSuccesMessage-methode gecalled worden waaraan het correcte bericht wordt meegegeven.
        [TestMethod]
        public void UpdateReeks_ReeksWerdGeselecteerdEnReeksNaamWerdIngevoerd_MaaktCallNaarShowSuccesMethode()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Reeks> reeksen = new List<Reeks>() { new Reeks(1, "reeks 1"), new Reeks(2, "reeks 2") };
            fakeManager.Setup(fake => fake.GeefAlleReeksen()).Returns(reeksen);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateReeksViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeReeks = reeksen[0];
            vm.NieuweReeksNaam = "nieuwe naam";

            vm.UpdateReeksCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De reeks werd gewijzigd."), Times.Once);
        }

        // Wanneer de gebruiker een reeks heeft geselecteerd en hij heeft de nieuwe reeksnaam ingevoerd, dan moet de UpdateReeks-methode gecalled worden waaraan een reeks object wordt meegegeven met de id van de geselecteerde reeks en met de nieuwe naam.
        [TestMethod]
        public void UpdateReeks_ReeksWerdGeselecteerdEnReeksNaamWerdIngevoerd_MaaktCallNaarUpdateReeks()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Reeks> reeksen = new List<Reeks>() { new Reeks(1, "reeks 1"), new Reeks(2, "reeks 2") };
            fakeManager.Setup(fake => fake.GeefAlleReeksen()).Returns(reeksen);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateReeksViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeReeks = reeksen[0];
            vm.NieuweReeksNaam = "nieuwe naam";

            vm.UpdateReeksCommand.Execute(null);

            fakeManager.Verify(fake => fake.UpdateReeks(new Reeks(reeksen[0].Id, vm.NieuweReeksNaam)), Times.Once);
        }

        // Telkens de domeinlaag een exception gooit, moet deze opgevangen worden door de UpdateReeks-methode, waarna de message van de opgevangen message doorgegeven wordt aan de _popupBox.ShowErrorMessage-methde.
        [TestMethod]
        public void UpdateReeks_WanneerDomeinlaagExceptionGooit_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Reeks> reeksen = new List<Reeks>() { new Reeks(1, "reeks 1"), new Reeks(2, "reeks 2") };
            fakeManager.Setup(fake => fake.GeefAlleReeksen()).Returns(reeksen);
            fakeManager.Setup(fake => fake.UpdateReeks(It.IsAny<Reeks>())).Throws(new DomeinException("test error message"));
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateReeksViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeReeks = reeksen[0];
            vm.NieuweReeksNaam = "REEKSNAAM";

            vm.UpdateReeksCommand.Execute(null);
            fakePopupBox.Verify(fake => fake.ShowErrorMessage("test error message"), Times.Once);
        }

    }
}
