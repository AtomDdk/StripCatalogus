using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domeinlaag.Interfaces;
using WinkelPresentatielaag.Factories;
using WinkelPresentatielaag.ViewModels;
using System;
using Domeinlaag.Model;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Domeinlaag;
using WinkelPresentatielaag.Interfaces;

namespace WinkelPresentatielaagTests
{
    [TestClass]
    public class MainViewModelTests
    {
        private protected Mock<ICatalogusManager> _fakeCatalogusManager;
        private protected Mock<IViewFactory> _fakeViewFactory;
        private protected Mock<IPopupBox> _fakePopupBox;
        private protected Mock<IVerkoopManager> _fakeVerkoopManager;

        public MainViewModelTests()
        {
            _fakeCatalogusManager = new Mock<ICatalogusManager>();
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleReeksen()).Returns(new List<Reeks>());
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleAuteurs()).Returns(new List<Auteur>());
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(new List<Uitgeverij>());
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleStrips()).Returns(new List<Strip>());
            _fakeVerkoopManager = new Mock<IVerkoopManager>();
            _fakeVerkoopManager.Setup(fake => fake.GeefAlleStripsVoorVerkoop()).Returns(new List<Strip>());
            _fakeVerkoopManager.Setup(fake => fake.ZoekStripsVoorVerkoop(It.IsAny<ZoekStripArguments>())).Returns(new List<Strip>());
            _fakePopupBox = new Mock<IPopupBox>();
            _fakeViewFactory = new Mock<IViewFactory>();
        }

        [TestMethod]
        public void MainViewModel_WanneerCatalogusManagerGelijkIsAanNull_GooitException()
        {
            ICatalogusManager catalogusManager = null;

            var ex = Assert.ThrowsException<NullReferenceException>(() => new MainViewModel(_fakePopupBox.Object, catalogusManager, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true));

            Assert.IsTrue(ex.Message.ToLower().Contains("manager"));
        }

        [TestMethod]
        public void MainViewModel_WanneerViewFactoryGelijkIsAanNull_GooitException()
        {
            IViewFactory viewFactory = null;

            var ex = Assert.ThrowsException<NullReferenceException>(() => new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, viewFactory, true));

            Assert.IsTrue(ex.Message.ToLower().Contains("viewfactory"));
        }

        [TestMethod]
        public void AlleStrips_BevatAltijd_AlleStripsUitDeDomeinlaag()
        {
            List<Strip> alleStripsUitDomeinlaag = new List<Strip> {
                new Strip(1, "strip 1", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")),
                new Strip(1, "strip 2", new Reeks("reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")),
                new Strip(1, "strip 3", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1"))
            };


            _fakeCatalogusManager.Setup(fake => fake.GeefAlleStrips()).Returns(alleStripsUitDomeinlaag);

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);


            Assert.IsTrue(vm.AlleStrips.SequenceEqual(alleStripsUitDomeinlaag));
            _fakeCatalogusManager.Verify(fake => fake.GeefAlleStrips(), Times.Once);
        }

        [TestMethod]
        public void Uitgeverijen_BevatAltijd_AlleUitgeverijenUitDeDomeinlaag()
        {
            List<Uitgeverij> alleUitgeverijenUitDomeinlaag = new List<Uitgeverij> {
                new Uitgeverij("uitgeverij 1"),
                new Uitgeverij("uitgeverij 2"),
                new Uitgeverij("uitgeverij 3")
            };

            _fakeCatalogusManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(alleUitgeverijenUitDomeinlaag);

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            Assert.IsTrue(vm.Uitgeverijen.SequenceEqual(alleUitgeverijenUitDomeinlaag));
            _fakeCatalogusManager.Verify(fake => fake.GeefAlleUitgeverijen(), Times.Once);
        }

        [TestMethod]
        public void VoegStripToe_MetGeldigeStrip_VoegtStripToeAanGeselecteerdeStrips()
        {
            List<Strip> alleStripsUitDomeinlaag = new List<Strip> {
                new Strip(1, "strip 1", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")),
                new Strip(1, "strip 2", new Reeks("reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")),
                new Strip(1, "strip 3", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1"))
             };

            _fakeCatalogusManager.Setup(fake => fake.GeefAlleStrips()).Returns(alleStripsUitDomeinlaag);

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            Strip geldigeStrip = alleStripsUitDomeinlaag.First();
            vm.VoegStripToeCommand.Execute(geldigeStrip);

            Assert.AreEqual(expected: new KeyValuePair<Strip, int>(geldigeStrip, 1), actual: vm.GeselecteerdeStrips.First());
            Assert.IsTrue(vm.GeselecteerdeStrips.Count == 1);
            Assert.IsTrue(vm.AlleStrips.Count == 3);
            Assert.IsTrue(vm.AlleStrips.SequenceEqual(alleStripsUitDomeinlaag));
        }

        [TestMethod]
        public void VerwijderStrip_MetGeldigeStrip_VerwijdertStripUitGeselecteerdeStrips()
        {
            ObservableCollection<KeyValuePair<Strip, int>> geselecteerdeStrips = new ObservableCollection<KeyValuePair<Strip, int>> {
                new KeyValuePair<Strip, int>(new Strip(1, "strip 1", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 1),
                new KeyValuePair<Strip, int>(new Strip(2, "strip 2", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 2),
                new KeyValuePair<Strip, int>(new Strip(3, "strip 3", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 5)
            };


            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            vm.GeselecteerdeStrips = geselecteerdeStrips;

            var teVerwijderenStripMetAantal = geselecteerdeStrips.First();

            vm.VerwijderStripCommand.Execute(teVerwijderenStripMetAantal);

            Assert.IsFalse(vm.GeselecteerdeStrips.Contains(teVerwijderenStripMetAantal));
            Assert.AreEqual(expected: new KeyValuePair<Strip, int>(new Strip(2, "strip 2", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 2), actual: vm.GeselecteerdeStrips[0]);
            Assert.AreEqual(expected: new KeyValuePair<Strip, int>(new Strip(3, "strip 3", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 5), actual: vm.GeselecteerdeStrips[1]);
            Assert.IsTrue(vm.GeselecteerdeStrips.Count == 2);
        }

        [TestMethod]
        public void MaakUitgeverijSelectieOngedaan_SteltAltijd_IndexVanGezochteUitgeverijInOpMin1()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            vm.MaakUitgeverijSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: vm.IndexVanGezochteUitgeverij, -1);
        }

        [TestMethod]
        public void MaakUitgeverijSelectieOngedaan_SteltAltijd_FilterOpUitgeverijInOpFalse()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            vm.MaakUitgeverijSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: vm.ZoekStripArguments.FilterOpReeks, false);
        }

        [TestMethod]
        public void MaakAuteurSelectieOngedaan_SteltAltijd_IndexVanGezochteAuteurInOpMin1()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);


            vm.MaakAuteurSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: vm.IndexVanGezochteAuteur, -1);
        }

        [TestMethod]
        public void MaakAuteurSelectieOngedaan_SteltAltijd_FilterOpAuteurInOpFalse()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);
            vm.MaakAuteurSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: vm.ZoekStripArguments.FilterOpAuteur, false);
        }

        [TestMethod]
        public void MaakReeksSelectieOngedaan_SteltAltijd_IndexVanGezochteReeksInOpMin1()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            vm.MaakReeksSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: vm.IndexVanGezochteReeks, -1);
        }

        [TestMethod]
        public void MaakReeksSelectieOngedaan_SteltAltijd_FilterOpReeksInOpFalse()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            vm.MaakReeksSelectieOngedaanCommand.Execute(null);

            Assert.AreEqual(expected: vm.ZoekStripArguments.FilterOpReeks, false);
        }

        [TestMethod]
        public void MainViewModel_WanneerIsLeveringGelijkIsAanFalse_SteltTekstenCorrectIn()
        {
            bool isLevering = false;

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, isLevering);

            Assert.AreEqual(expected: "Stripassortiment", actual: vm.StripsLabelTekst);
            Assert.AreEqual(expected: "Winkelmand", actual: vm.GeselecteerdeStripsLabelTekst);
            Assert.AreEqual(expected: "Ga Naar BestellingOverzicht", actual: vm.ButtonTekst);
        }

        [TestMethod]
        public void MainViewModel_WanneerIsLeveringGelijkIsAanTrue_SteltTekstenCorrectIn()
        {
            bool isLevering = true;

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, isLevering);


            Assert.AreEqual(expected: "Inventaris", actual: vm.StripsLabelTekst);
            Assert.AreEqual(expected: "Levering", actual: vm.GeselecteerdeStripsLabelTekst);
            Assert.AreEqual(expected: "Ga Naar LeveringOverzicht", actual: vm.ButtonTekst);
        }

        [TestMethod]
        public void ZoekStrip_GebruikerZoektOpReeksNummer_ZoekStripArgsWordtIngesteld()
        {
            _fakeCatalogusManager.Setup(fake => fake.ZoekStrips(It.IsAny<ZoekStripArguments>())).Returns(new List<Strip>());

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, false);

            vm.GezochtReeksNummer = "5";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 5, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrip_GebruikerZoektOpReeksNummerDatLettersBevat_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, false);

            vm.GezochtReeksNummer = "a5c";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrip_GebruikerZoektOpReeksNummerDatAlleenMaarUitLettersBestaat_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, false);

            vm.GezochtReeksNummer = "abc";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrip_GebruikerZoektOpReeksNummerDatEnkelSpatiesBevat_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {
            _fakeCatalogusManager.Setup(fake => fake.ZoekStrips(It.IsAny<ZoekStripArguments>())).Returns(new List<Strip>());

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, false);

            vm.GezochtReeksNummer = "    ";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void ZoekStrip_GebruikerZoektOpReeksNummerDatBestaatUitEenLegeString_ReeksNummerPropertyVanZoekStripArgsWordtIngesteldOpNul()
        {
            _fakeCatalogusManager.Setup(fake => fake.ZoekStrips(It.IsAny<ZoekStripArguments>())).Returns(new List<Strip>());

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, false);

            vm.GezochtReeksNummer = "";

            vm.ZoekStripCommand.Execute(null);

            Assert.AreEqual(expected: 0, actual: vm.ZoekStripArguments.ReeksNummer);
        }

        [TestMethod]
        public void GaNaarOverzicht_WanneerGeselecteerdeStripsGelijkIsNulStripsBevatEnIsLeveringGelijkIsAanTrue_CalledShowErrorMessage()
        {
            bool isLevering = true;

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, isLevering);

            vm.GaNaarOverzichtCommand.Execute(null);

            _fakePopupBox.Verify(fake => fake.ShowErrorMessage("Uw leveringen zijn leeg."));
        }

        [TestMethod]
        public void GaNaarOverzicht_WanneerGeselecteerdeStripsGelijkIsNulStripsBevatEnIsLeveringGelijkIsAanFalse_CalledShowErrorMessage()
        {
            bool isLevering = false;

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, isLevering);


            vm.GaNaarOverzichtCommand.Execute(null);

            _fakePopupBox.Verify(fake => fake.ShowErrorMessage("Uw winkelmand is leeg."));
        }

        [TestMethod]
        public void GaNaarOverzicht_WanneerGeselecteerdeStripsGroterIsDan0_CalledViewFactory()
        {
            ObservableCollection<KeyValuePair<Strip, int>> geselecteerdeStrips = new ObservableCollection<KeyValuePair<Strip, int>> {
                new KeyValuePair<Strip, int>(new Strip(1, "strip 1", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 1),
                new KeyValuePair<Strip, int>(new Strip(2, "strip 2", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 2),
                new KeyValuePair<Strip, int>(new Strip(3, "strip 3", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 5)
            };
            bool isLevering = false;

            MainViewModel vm = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, isLevering);

            vm.GeselecteerdeStrips = geselecteerdeStrips;

            vm.GaNaarOverzichtCommand.Execute(null);


            _fakeViewFactory.Verify(fake => fake.MaakOverzichtView(vm, isLevering), Times.Once);

        }


























    }
}
