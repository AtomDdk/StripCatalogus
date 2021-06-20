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
    public class VoegUitgeverijToeViewModelTests
    {
        // Wanneer de gebruiker een lege uitgeverijnaam probeert toe te voegen dan moet de ShowErrorMessage-methode aangeroepen waaraan de correcte error wordt meegegeven.
        [TestMethod]
        public void VoegUitgeverijToeAanDb_LegeUitgeverijNaam_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            fakeManager.Setup(fake => fake.VoegUitgeverijToe(It.IsAny<string>())).Throws(new DomeinException("De uitgeverijnaam mag niet leeg zijn."));
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegUitgeverijToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenUitgeverijNaam = string.Empty;
            vm.VoegUitgeverijToeAanDbCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowErrorMessage("De uitgeverijnaam mag niet leeg zijn."), Times.Once);
        }

        // Wanneer de gebruiker een uitgeverijnaam invult dat niet leeg is dan moet er een call gemaakt worden naar de VoegUitgeverijToe-methode waaraan de ingevoerde uitgeverijnaam wordt meegegeven.
        [TestMethod]
        public void VoegUitgeverijToeAanDb_IngevuldeUitgeverijNaam_MaaktCallNaarVoegUitgeverijToe()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegUitgeverijToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenUitgeverijNaam = "uitgeverij 1";
            vm.VoegUitgeverijToeAanDbCommand.Execute(null);

            fakeManager.Verify(fake => fake.VoegUitgeverijToe("uitgeverij 1"), Times.Once);
        }

        // Wanneer de gebruiker een uitgeverijnaam invult dan moet de ShowSucces-methode aangeroepen waaraan het correcte bericht wordt meegegeven.
        [TestMethod]
        public void VoegUitgeverijToeAanDb_IngevuldeUitgeverijNaam_MaaktCallNaarShowSuccesMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegUitgeverijToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenUitgeverijNaam = "uitgeverij 1";
            vm.VoegUitgeverijToeAanDbCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De uitgeverij werd toegevoegd aan de database."), Times.Once);
        }


    }
}
