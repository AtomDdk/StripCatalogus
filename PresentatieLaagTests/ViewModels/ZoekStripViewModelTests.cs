using Domeinlaag;
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
    [TestClass()]
    public class ZoekStripViewModelTests
    {
        private Mock<ICatalogusManager> MaakFakeCatalogusManager()
        {
            var catalogusManagerMock = new Mock<ICatalogusManager>();
            catalogusManagerMock.Setup(mock => mock.GeefAlleUitgeverijen()).Returns(new List<Uitgeverij> { new Uitgeverij("uitgeverij 1"), new Uitgeverij("uitgeverij 2") });
            catalogusManagerMock.Setup(mock => mock.GeefAlleReeksen()).Returns(new List<Reeks> { new Reeks("reeks 1"), new Reeks("reeks 2") });
            catalogusManagerMock.Setup(mock => mock.GeefAlleAuteurs()).Returns(new List<Auteur> { new Auteur("auteur 1"), new Auteur("auteur 2") });
            catalogusManagerMock.Setup(mock => mock.GeefAlleStrips()).Returns(new List<Strip> { new Strip("titel 1", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")) });
            catalogusManagerMock.Setup(mock => mock.ZoekStrips(It.IsAny<ZoekStripArguments>())).Returns(new List<Strip>());

            return catalogusManagerMock;
        }

        private Mock<IViewFactory> MaakFakeViewFactory()
        {
            var viewFactoryMock = new Mock<IViewFactory>();
            viewFactoryMock.Setup(mock => mock.MaakVoegStripToeView(null));
            return viewFactoryMock;
        }

        private Mock<IPopupBox> MaakFakePopupBox()
        {
            var popupBoxMock = new Mock<IPopupBox>();
            return popupBoxMock;
        }

        // Wanneer er op de 'Update Strip'-knop gedrukt terwijl de gebruiker een strip heeft geselecteerd, dan moet er een call worden gemaakt naar de viewFactory om een VoegStripToeView te instantiëren.
        [TestMethod]
        public void OpenUpdateStripView_WanneerEenStripWerdGeselecteerd_CalledMaakVoegStripToeView()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);

            vm.GeselecteerdeStrip = new Strip("titel 1", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1"));

            vm.OpenUpdateStripViewCommand.Execute(null);

            fakeViewFactory.Verify(mock => mock.MaakVoegStripToeView(It.IsAny<Strip>()), Times.Once);
        }

        // Wanneer er op de 'Update Strip'-knop gedrukt terwijl de gebruiker geen strip heeft geselecteerd, dan moet er een call gemaakt worden naar de PopupBox-klasse zodat er een errorbericht op het scherm verschijnt.
        // Het correcte errorbericht moet meegegeven worden aan Popup-klasse zodat deze kan weergegeven worden op het scherm.
        [TestMethod]
        public void OpenUpdateStripView_WanneerGeenStripIsGeselecteerd_CalledShowErrorMessage()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);

            Strip erIsGeenStripGeselecteerd = null;

            vm.OpenUpdateStripViewCommand.Execute(erIsGeenStripGeselecteerd);

            fakePopUpBox.Verify(fake => fake.ShowErrorMessage("Gelieve eerst een strip te selecteren."));
        }

        // Wanneer er op de 'Voeg Strip Toe'-knop gedrukt wordt moet altijd de factory gecalled worden voor een nieuwe VoegStripToeView te instantiëren.
        [TestMethod]
        public void OpenVoegStripToeView_GaatAltijd_MaakVoegStripToeCallen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);

            vm.OpenVoegStripToeViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakVoegStripToeView(null), Times.Once);
        }

        // Wanneer er op de 'X-knop naast de UitgeverijComboBox wordt geklikt, dan moet de index van de gezochte uitgeverij ingesteld worden op -1
        // zodat de uitgeverijselectie ongedaan wordt gemaakt.
        [TestMethod]
        public void MaakUitgeverijSelectieOngedaan_GaatAltijd_IndexVanGezochteUitgeverijInstellenOpMinÉén()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.MaakUitgeverijSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: -1 , actual: vm.IndexVanGezochteUitgeverij);
        }

        // Wanneer er op de 'X-knop naast de ReeksComboBox wordt geklikt, dan moet de index van de gezochte reeks ingesteld worden op -1
        // zodat de reeksselectie ongedaan wordt gemaakt.
        [TestMethod]
        public void MaakReeksSelectieOngedaan_GaatAltijd_IndexVanGezochteReeksInstellenOpMinÉén()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.MaakReeksSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: -1, actual: vm.IndexVanGezochteReeks);
        }

        // Wanneer er op de 'X-knop naast de AuteurComboBox wordt geklikt, dan moet de index van de gezochte auteur ingesteld worden op -1
        // zodat de auteurselectie ongedaan wordt gemaakt.
        [TestMethod]
        public void MaakAuteurSelectieOngedaan_GaatAltijd_IndexVanGezochteAuteurInstellenOpMinÉén()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.MaakAuteurSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: -1, actual: vm.IndexVanGezochteAuteur);
        }

        // Wanneer er op de 'Update Auteur'-knop gedrukt wordt moet altijd de factory gecalled worden voor een nieuwe UpdateAuteurView te instantiëren.
        [TestMethod]
        public void OpenUpdateAuteurView_GaatAltijd_MaakUpdateAuteurViewCallen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.OpenUpdateAuteurViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakUpdateAuteurView(), Times.Once);
        }

        // Wanneer er op de 'Update Uitgeverij'-knop gedrukt wordt moet altijd de factory gecalled worden voor een nieuwe UpdateUitgeverijView te instantiëren.
        [TestMethod]
        public void OpenUpdateUitgeverijView_GaatAltijd_MaakUpdateUitgeverijViewCallen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.OpenUpdateUitgeverijViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakUpdateUitgeverijView(), Times.Once);
        }

        // Wanneer er op de 'Update Reeks'-knop gedrukt wordt moet altijd de factory gecalled worden voor een nieuwe UpdateReeksView te instantiëren.
        [TestMethod]
        public void OpenUpdateReeksView_GaatAltijd_MaakUpdateReeksViewCallen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.OpenUpdateReeksViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakUpdateReeksView(), Times.Once);
        }

        // Wanneer er op de 'Voeg Reeks Toe'-knop gedrukt wordt moet altijd de factory gecalled worden voor een nieuwe VoegReeksToeView te instantiëren.
        [TestMethod]
        public void OpenVoegReeksToeView_GaatAltijd_MaakVoegReeksToeViewCallen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.OpenVoegReeksToeViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakVoegReeksToeView(), Times.Once);
        }

        // Wanneer er op de 'Voeg Uitgeverij Toe'-knop gedrukt wordt moet altijd de factory gecalled worden voor een nieuwe VoegUitgeverijToeView te instantiëren.
        [TestMethod]
        public void OpenVoegUitgeverijToeView_GaatAltijd_MaakVoegUitgeverijToeViewCallen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.OpenVoegUitgeverijToeViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakVoegUitgeverijToeView(), Times.Once);
        }

        // Wanneer er op de 'Voeg Auteur Toe'-knop gedrukt wordt moet altijd de factory gecalled worden voor een nieuwe VoegAuteurToeView te instantiëren.
        [TestMethod]
        public void OpenVoegAuteurToeView_GaatAltijd_MaakVoegAuteurToeViewCallen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.OpenVoegAuteurToeViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakVoegAuteurToeView(), Times.Once);
        }

        // Wanneer de RefreshStripLijst een true boolean ontvangt, dan moeten de Strips, Reeksen en Uitgeverijen opnieuw opgevuld worden zodat deze de laatste nieuwe objecten bevatten uit de database.
        [TestMethod]
        public void RefreshStripLijst_GaatAltijd_AlleCollectiesHervullen()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.RefreshAlleCollecties();

            fakeManager.Verify(fake => fake.GeefAlleStrips(), Times.Exactly(2));
            fakeManager.Verify(fake => fake.GeefAlleReeksen(), Times.Exactly(2));
            fakeManager.Verify(fake => fake.GeefAlleUitgeverijen(), Times.Exactly(2));
            fakeManager.Verify(fake => fake.GeefAlleAuteurs(), Times.Exactly(2));
        }

        [TestMethod]
        public void ZoekStrips_GebruikerZoektOpReeksNummer_ZoekStripArgsWordtIngesteld()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.GezochtReeksNummer = "5";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 5, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrips_GebruikerZoektOpReeksNummerDatLettersBevat_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.GezochtReeksNummer = "a5c";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrips_GebruikerZoektOpReeksNummerDatAlleenMaarUitLettersBestaat_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.GezochtReeksNummer = "abc";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrips_GebruikerZoektOpReeksNummerDatEnkelSpatiesBevat_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.GezochtReeksNummer = "    ";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrips_GebruikerZoektOpReeksNummerDatBestaatUitEenLegeString_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new ZoekStripViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object);
            vm.GezochtReeksNummer = "";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }
























    }
}
