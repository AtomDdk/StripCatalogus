using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
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
    public class VoegReeksToeViewModelTests
    {
        // Wanneer de gebruiker een lege reeksnaam probeert toe te voegen dan moet de ShowErrorMessage-methode aangeroepen waaraan de correcte error wordt meegegeven.
        [TestMethod]
        public void VoegReeksToeAanDb_LegeReeksNaam_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            fakeManager.Setup(fake => fake.VoegReeksToe(It.IsAny<string>())).Throws(new DomeinException("De reeksnaam mag niet leeg zijn."));
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegReeksToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenReeksNaam = string.Empty;
            vm.VoegReeksToeAanDbCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowErrorMessage("De reeksnaam mag niet leeg zijn."), Times.Once);
        }

        // Wanneer de gebruiker een reeksnaam invult dat niet leeg is dan moet er een call gemaakt worden naar de VoegReeksToe-methode waaraan de ingevoerde reeksnaam wordt meegegeven.
        [TestMethod]
        public void VoegReeksToeAanDb_IngevuldeReeksNaam_MaaktCallVoegReeksToe()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegReeksToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenReeksNaam = "reeks 1";
            vm.VoegReeksToeAanDbCommand.Execute(null);

            fakeManager.Verify(fake => fake.VoegReeksToe("reeks 1"), Times.Once);
        }

        // Wanneer de gebruiker een reeksnaam invult dan moet de ShowSucces-methode aangeroepen waaraan het correcte bericht wordt meegegeven.
        [TestMethod]
        public void VoegReeksToeAanDb_IngevuldeReeksNaam_MaaktCallNaarShowSuccesMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegReeksToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenReeksNaam = "reeks 1";
            vm.VoegReeksToeAanDbCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De reeks werd toegevoegd aan de database."), Times.Once);
        }



    }
}
