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
    public class VoegAuteurToeViewModelTests
    {
        // Wanneer de gebruiker een lege auteurnaam probeert toe te voegen dan moet de ShowErrorMessage-methode aangeroepen waaraan de correcte error wordt meegegeven.
        [TestMethod]
        public void VoegAuteurToeAanDb_LegeAuteurNaam_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            fakeManager.Setup(fake => fake.VoegAuteurToe(It.IsAny<string>())).Throws(new DomeinException("De auteurnaam mag niet leeg zijn."));
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegAuteurToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenAuteurNaam = string.Empty;
            vm.VoegAuteurToeAanDbCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowErrorMessage("De auteurnaam mag niet leeg zijn."), Times.Once);
        }

        // Wanneer de gebruiker een auteurnaam invult dat niet leeg is dan moet er een call gemaakt worden naar de VoegAuteurToe-methode waaraan de ingevoerde auteurnaam wordt meegegeven.
        [TestMethod]
        public void VoegAuteurToeAanDb_IngevuldeAuteurNaam_MaaktCallVoegAuteurToe()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegAuteurToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenAuteurNaam = "auteur 1";
            vm.VoegAuteurToeAanDbCommand.Execute(null);

            fakeManager.Verify(fake => fake.VoegAuteurToe("auteur 1"), Times.Once);
        }

        // Wanneer de gebruiker een auteurnaam invult dan moet de ShowSucces-methode aangeroepen waaraan het correcte bericht wordt meegegeven.
        [TestMethod]
        public void VoegAuteurToeAanDb_IngevuldeAuteurNaam_MaaktCallNaarShowSuccesMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new VoegAuteurToeViewModel(catalogusManager: fakeManager.Object, popupBox: fakePopupBox.Object);

            vm.ToeTeVoegenAuteurNaam = "auteur 1";
            vm.VoegAuteurToeAanDbCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De auteur werd toegevoegd aan de database."), Times.Once);
        }





    }
}
