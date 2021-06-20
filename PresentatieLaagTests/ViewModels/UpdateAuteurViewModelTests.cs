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
    public class UpdateAuteurViewModelTests
    {
        // Wanneer de gebruiker een auteur probeert te wijzigen zonder eerst een auteur te selecteren, dan moet de ShowErrorMessage-methode gecalled worden waaraan de correcte foutmelding wordt meegegeven.
        [TestMethod]
        public void UpdateAuteur_WanneerErGeenAuteurWerdGeselecteerd_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            fakeManager.Setup(fake => fake.GeefAlleAuteurs()).Returns(new List<Auteur>() { new Auteur(1, "auteur 1"), new Auteur(2, "auteur 2") });
            var fakePopupBox = new Mock<IPopupBox>();
            
            var vm = new UpdateAuteurViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.NieuweAuteurNaam = "nieuweTestnaam";

            vm.UpdateAuteurCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowErrorMessage("Gelieve een auteur te selecteren."), Times.Once);
        }

        // Wanneer de gebruiker een auteur heeft geselecteerd maar hij heeft de nieuwe auteurnaam niet ingevuld, dan moet de ShowErrorMessage-methode gecalled worden waaraan de correcte foutmelding wordt meegegeven.
        [TestMethod]
        public void UpdateAuteur_LegeAuteurNaam_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Auteur> auteurs = new List<Auteur>() { new Auteur(1, "auteur 1"), new Auteur(2, "auteur 2") };
            fakeManager.Setup(fake => fake.GeefAlleAuteurs()).Returns(auteurs);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateAuteurViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeAuteur = auteurs[0];
            vm.NieuweAuteurNaam = string.Empty;

            vm.UpdateAuteurCommand.Execute(null);
            fakePopupBox.Verify(fake => fake.ShowErrorMessage("Naam mag niet leeg zijn."), Times.Once);
        }

        // Wanneer de gebruiker een auteur heeft geselecteerd en hij heeft de nieuwe auteurnaam ingevoerd, dan moet de ShowSuccesMessage-methode gecalled worden waaraan het correcte bericht wordt meegegeven.
        [TestMethod]
        public void UpdateAuteur_AuteurWerdGeselecteerdEnAuteurNaamWerdIngevoerd_MaaktCallNaarShowSuccesMethode()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Auteur> auteurs = new List<Auteur>() { new Auteur(1, "auteur 1"), new Auteur(2, "auteur 2") };
            fakeManager.Setup(fake => fake.GeefAlleAuteurs()).Returns(auteurs);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateAuteurViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeAuteur = auteurs[0];
            vm.NieuweAuteurNaam = "nieuwe naam";

            vm.UpdateAuteurCommand.Execute(null);

            fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De auteur werd gewijzigd."), Times.Once);
        }

        // Wanneer de gebruiker een auteur heeft geselecteerd en hij heeft de nieuwe auteurnaam ingevoerd, dan moet de UpdateAuteur-methode gecalled worden waaraan een auteur object wordt meegegeven met de id van de geselecteerde auteur en met de nieuwe naam.
        [TestMethod]
        public void UpdateAuteur_AuteurWerdGeselecteerdEnAuteurNaamWerdIngevoerd_MaaktCallNaarUpdateAuteur()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Auteur> auteurs = new List<Auteur>() { new Auteur(1, "auteur 1"), new Auteur(2, "auteur 2") };
            fakeManager.Setup(fake => fake.GeefAlleAuteurs()).Returns(auteurs);
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateAuteurViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeAuteur = auteurs[0];
            vm.NieuweAuteurNaam = "nieuwe naam";

            vm.UpdateAuteurCommand.Execute(null);

            fakeManager.Verify(fake => fake.UpdateAuteur(new Auteur(auteurs[0].Id, vm.NieuweAuteurNaam)), Times.Once);
        }

        // Telkens de domeinlaag een exception gooit, moet deze opgevangen worden door de UpdateAuteur-methode, waarna de message van de opgevangen message doorgegeven wordt aan de _popupBox.ShowErrorMessage-methde.
        [TestMethod]
        public void UpdateAuteur_WanneerDomeinlaagExceptionGooit_MaaktCallNaarShowErrorMessage()
        {
            var fakeManager = new Mock<ICatalogusManager>();
            List<Auteur> auteurs = new List<Auteur>() { new Auteur(1, "auteur 1"), new Auteur(2, "auteur 2") };
            fakeManager.Setup(fake => fake.GeefAlleAuteurs()).Returns(auteurs);
            fakeManager.Setup(fake => fake.UpdateAuteur(It.IsAny<Auteur>())).Throws(new DomeinException("test error message"));
            var fakePopupBox = new Mock<IPopupBox>();

            var vm = new UpdateAuteurViewModel(manager: fakeManager.Object, popupBox: fakePopupBox.Object);
            vm.GeselecteerdeAuteur = auteurs[0];
            vm.NieuweAuteurNaam = "auteurnaam";

            vm.UpdateAuteurCommand.Execute(null);
            fakePopupBox.Verify(fake => fake.ShowErrorMessage("test error message"), Times.Once);
        }

    }
}
