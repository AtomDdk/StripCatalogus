using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Presentatie_laag.Interfaces;
using Presentatie_laag.ViewModels;
using Presentatie_laag.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PresentatieLaagTests.ViewModels
{
    [TestClass]
    public class VoegStripToeViewModelTests
    {
        private Mock<ICatalogusManager> MaakFakeCatalogusManager()
        {
            var catalogusManagerMock = new Mock<ICatalogusManager>();
            catalogusManagerMock.Setup(mock => mock.GeefAlleUitgeverijen()).Returns(new List<Uitgeverij> { new Uitgeverij("uitgeverij 1"), new Uitgeverij("uitgeverij 2") });
            catalogusManagerMock.Setup(mock => mock.GeefAlleReeksen()).Returns(new List<Reeks> { new Reeks("reeks 1"), new Reeks("reeks 2") });
            catalogusManagerMock.Setup(mock => mock.GeefAlleAuteurs()).Returns(new List<Auteur> { new Auteur("A"), new Auteur("B") });
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


        // Telkens er een bestaande auteur aan de strip wordt toegevoegd via de '>'-knop dan moet deze auteur worden toegevoegd aan de GeselecteerdeAuteurs-collectie.
        [TestMethod]
        public void VoegAuteurToeAanStrip_MetGeldigeAuteur_VoegtAuteurToeAanGeselecteerdeAuteurs()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Auteur nieuweAuteur = new Auteur(naam: "Jan Janssens");
            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(nieuweAuteur);

            Assert.AreEqual(vm.GeselecteerdeAuteurs.Last(), nieuweAuteur);
        }

        // Telkens er een bestaande auteur aan de strip wordt toegevoegd via de '>'-knop dan moet deze auteur worden verwijderd uit de Auteurs-collectie.
        [TestMethod]
        public void VoegAuteurToeAanStrip_MetGeldigeAuteur_VerwijderdAuteurUitAuteurs()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Auteur nieuweAuteur = new Auteur(naam: "Jan Janssens");
            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(nieuweAuteur);

            Assert.IsFalse(vm.Auteurs.Contains(nieuweAuteur));
        }

        // Als de toe te voegen auteur gelijk is aan null dan mag deze null waarde niet toegevoegd worden aan de GeselecteerdeAuteurs-collectie.
        [TestMethod]
        public void VoegAuteurToeAanStrip_MetNullWaarde_WordtNietToegevoegdAanGeselecteerdeAuteurs()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Auteur ongeldigeAuteur = null;
            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(ongeldigeAuteur);

            Assert.IsFalse(vm.GeselecteerdeAuteurs.Contains(ongeldigeAuteur));
        }
        // Als er null wordt meegegven aan de VoegAuteurToeAanStrip-methode dan mag de GeselecteerdeAuteurs-collectie niet veranderd worden.
        [TestMethod]
        public void VoegAuteurToeAanStrip_MetNullWaarde_GeselecteerdeAuteursWordtNietGewijzigd()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Auteur geldigeAuteur = new Auteur("A");
            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(geldigeAuteur);

            Auteur ongeldigeAuteur = null;
            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(ongeldigeAuteur);

            Assert.IsFalse(vm.GeselecteerdeAuteurs.Contains(ongeldigeAuteur));

            Assert.IsTrue(vm.GeselecteerdeAuteurs[0] == geldigeAuteur);
            Assert.IsTrue(vm.GeselecteerdeAuteurs.Count == 1);

        }

        // Het moet onmogelijk zijn om twee keer dezelfde auteur toe te voegen aan de GeselecteerdeAuteurs-collectie.
        [TestMethod]
        public void VoegAuteurToeAanStrip_MetDezelfdeAuteur_WordtNietToegevoegdAanGeselecteerdeAuteurs()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Auteur nieuweAuteur = new Auteur(naam: "Jan Janssens");

            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(nieuweAuteur);
            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(nieuweAuteur);

            Assert.IsTrue(vm.GeselecteerdeAuteurs.Where(x => x == nieuweAuteur).Count() == 1);
        }

        // Als de auteur werd terug gevonden in de GeselecteerdeAuteurs-collectie dan moet deze verwijderd worden uit de collectie.
        [TestMethod]
        public void VerwijderAuteurUitStrip_WanneerAuteurWerdTerugGevonden_VerwijderdAuteurUitGeselecteerdeAuteurs()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            List<Auteur> auteurs = fakeManager.Object.GeefAlleAuteurs();

            vm.VerwijderAuteurUitGeselecteerdeAuteursCommand.Execute(auteurs[0]);

            Assert.IsFalse(vm.GeselecteerdeAuteurs.Contains(auteurs[0]));
            Assert.IsTrue(vm.Auteurs[0] == auteurs[0]);
            Assert.IsTrue(vm.Auteurs[1] == auteurs[1]);
            Assert.IsTrue(vm.Auteurs.Count == 2);
        }

        // Als de auteur werd terug gevonden in de GeselecteerdeAuteurs-collectie dan moet deze toegevoegd worden aan de Auteurs-Collectie.
        [TestMethod]
        public void VerwijderAuteurUitStrip_WanneerAuteurWerdTerugGevonden_VoegtAuteurToeAanAuteurs()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Auteur nieuweAuteur = new Auteur(naam: "Jan Janssens");

            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(nieuweAuteur);
            vm.VerwijderAuteurUitGeselecteerdeAuteursCommand.Execute(nieuweAuteur);

            Assert.IsTrue(vm.Auteurs.Contains(nieuweAuteur));
            Assert.IsTrue(vm.GeselecteerdeAuteurs.Count == 0);
        }

        // Als de auteur werd terug gevonden in de GeselecteerdeAuteurs-collectie dan moet de Auteur-property worden ingesteld op alle auteurs in alfabetische volgorde.
        [TestMethod]
        public void VerwijderAuteurUitStrip_WanneerAuteurWerdTerugGevonden_SteltAuteursPropertyIn()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Auteur a1 = new Auteur(naam: "C");
            Auteur a2 = new Auteur(naam: "AA");

            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(a1);
            vm.VoegAuteurToeAanGeselecteerdeAuteursCommand.Execute(a2);

            vm.VerwijderAuteurUitGeselecteerdeAuteursCommand.Execute(a1);
            vm.VerwijderAuteurUitGeselecteerdeAuteursCommand.Execute(a2);

            Assert.IsTrue(vm.Auteurs[0].Naam == "A");
            Assert.IsTrue(vm.Auteurs[1].Naam == "AA");
            Assert.IsTrue(vm.Auteurs[2].Naam == "B");
            Assert.IsTrue(vm.Auteurs[3].Naam == "C");
            Assert.IsTrue(vm.GeselecteerdeAuteurs.Count == 0);
        }

        // Wanneer er op de '+'-knop geklikt wordt naar de AuteursListBox dan moet er een call gemaakt worden naar de factory om een VoegAuteurToeView te instantiëren.
        [TestMethod]
        public void OpenVoegAuteurToeView_GaatAltijd_CallMakenNaarViewFactory()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            vm.OpenVoegAuteurToeViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakVoegAuteurToeView(vm), Times.Once);
        }

        // Wanneer er op de '+'-knop geklikt wordt naar de ReeksComboBox dan moet er een call gemaakt worden naar de factory om een VoegReeksToeView te instantiëren.
        [TestMethod]
        public void OpenVoegReeksToeView_GaatAltijd_CallMakenNaarViewFactory()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            vm.OpenVoegReeksToeViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakVoegReeksToeView(vm), Times.Once);
        }

        // Wanneer er op de '+'-knop geklikt wordt naar de UitgeverijComboBox dan moet er een call gemaakt worden naar de factory om een VoegUitgeverijToeView te instantiëren.
        [TestMethod]
        public void OpenVoegUitgeverijToeView_GaatAltijd_CallMakenNaarViewFactory()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            vm.OpenVoegUitgeverijToeViewCommand.Execute(null);

            fakeViewFactory.Verify(fake => fake.MaakVoegUitgeverijToeView(vm), Times.Once);
        }

        // Telkens er met de VoegReeksToeView een reeks wordt toegevoegd aan de database, dan moet de GeselecteerdeReek-property worden ingesteld met de reeks die werd meegegeven als argumentwaarde aan de VoegReeksToeAanStrip-methode.
        [TestMethod]
        public void VoegReeksToeAanStrip_GeldigeReeks_SteltGeselecteerdeReeksIn()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Reeks reeks = new Reeks(naam: "Jommeke op avontuur met de Bruinfluiten deel 1");

            vm.VoegReeksToe(reeks);

            Assert.AreEqual(expected: reeks, actual: vm.GeselecteerdeReeks);
        }

        // Telkens er met de VoegReeksToeView een reeks wordt toegevoegd aan de database, dan moet de GeselecteerdeReek-property worden ingesteld met de reeks die werd meegegeven als argumentwaarde aan de VoegReeksToeAanStrip-methode.
        [TestMethod]
        public void VoegUitgeverijToeAanStrip_GeldigeUitgeverij_SteltGeselecteerdeUitgeverijIn()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Uitgeverij uitgeverij = new Uitgeverij(naam: "uitgeverij 1");

            vm.VoegUitgeverijToe(uitgeverij);

            Assert.AreEqual(expected: uitgeverij, actual: vm.GeselecteerdeUitgeverij);
        }

        // Alle properties moeten ingesteld worden op de waarden van de strip die werd meegegeven als argument van de OntvangTeUpdatenStrip-methode.
        [TestMethod]
        public void OntvangTeUpdatenStrip_GaatAltijd_AlleStripPropertiesInstellenMetDeWaardenVanDeOntvangenStrip()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Strip strip = new Strip("striptitel", new Reeks(1, "reeks 1"), 5, new List<Auteur>() { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1"));

            vm.OntvangTeUpdatenStrip(strip);

            Assert.AreEqual(expected: strip.Titel, actual: vm.StripTitel);
            Assert.AreEqual(expected: strip.Reeks, actual: vm.GeselecteerdeReeks);
            Assert.AreEqual(expected: strip.Uitgeverij, actual: vm.GeselecteerdeUitgeverij);
            Assert.AreEqual(expected: strip.ReeksNummer.ToString(), actual: vm.ReeksNummer);
            Assert.AreEqual(expected: strip.GeefAuteurs().Count, actual: vm.GeselecteerdeAuteurs.Count);
            Assert.AreEqual(expected: strip, actual: vm.TeUpdatenStrip);
        }

        [TestMethod]
        public void VoegStripToe_CorrecteInvoer_VoegtMessageToe()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            vm.StripTitel = "strip 1";
            vm.GeselecteerdeReeks = new Reeks(1, "reeks 1");
            vm.GeselecteerdeAuteurs = new ObservableCollection<Auteur> { new Auteur(1, "auteur 1") };
            vm.ReeksNummer = "1";
            vm.GeselecteerdeUitgeverij = new Uitgeverij(1, "uitgeverij 1");

            vm.CompleteCommand.Execute(null);

            fakePopUpBox.Verify(fake => fake.ShowSuccesMessage("De strip werd toegevoegd aan de database."), Times.Once);
        }

        // Telkens er geen strip wordt meegegeven moeten de tekstevelden leeg blijven.
        [TestMethod]
        public void OntVangTeUpdatenStrip_WanneerNullWordtMeegegeven_AlleTekstVeldenMoetenLeegBlijven()
        {
            var fakePopUpBox = MaakFakePopupBox();
            var fakeViewFactory = MaakFakeViewFactory();
            var fakeManager = MaakFakeCatalogusManager();

            var vm = new VoegStripToeViewModel(catalogusManager: fakeManager.Object, viewFactory: fakeViewFactory.Object, popupBox: fakePopUpBox.Object, null);

            Strip ongeldeStrip = null;

            vm.OntvangTeUpdatenStrip(ongeldeStrip);

            Assert.AreEqual(expected: null, actual: vm.StripTitel);
            Assert.IsTrue(vm.GeselecteerdeReeks.Naam.Contains("<") && vm.GeselecteerdeReeks.Naam.Contains(">"));
            Assert.AreEqual(expected: null, actual: vm.GeselecteerdeUitgeverij);
            Assert.AreEqual(expected: "", actual: vm.ReeksNummer);
            Assert.AreEqual(expected: 0, actual: vm.GeselecteerdeAuteurs.Count);
            Assert.AreEqual(expected: null, actual: vm.TeUpdatenStrip);

        }
    }
}
