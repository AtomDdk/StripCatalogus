using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Text;
using WinkelPresentatielaag.Interfaces;
using WinkelPresentatielaag.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Domeinlaag.Exceptions;

namespace WinkelPresentatielaagTests
{
    [TestClass]
    public class OverzichtViewModelTests
    {
        private protected Mock<ICatalogusManager> _fakeCatalogusManager;
        private protected Mock<IPopupBox> _fakePopupBox;
        private protected Mock<IVerkoopManager> _fakeVerkoopManager;
        private ObservableCollection<KeyValuePair<Strip, int>> _stripsEnAantallen;

        private protected Mock<IViewFactory> _fakeViewFactory;
        private MainViewModel _mainViewModel;

        public OverzichtViewModelTests()
        {
            _fakeCatalogusManager = new Mock<ICatalogusManager>();
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleReeksen()).Returns(new List<Reeks>());
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleAuteurs()).Returns(new List<Auteur>());
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleUitgeverijen()).Returns(new List<Uitgeverij>());
            _fakeCatalogusManager.Setup(fake => fake.GeefAlleStrips()).Returns(new List<Strip>());

            _fakeViewFactory = new Mock<IViewFactory>();
            _fakePopupBox = new Mock<IPopupBox>();
            _fakeVerkoopManager = new Mock<IVerkoopManager>();
            _mainViewModel = new MainViewModel(_fakePopupBox.Object, _fakeCatalogusManager.Object, _fakeVerkoopManager.Object, _fakeViewFactory.Object, true);

            _stripsEnAantallen = new ObservableCollection<KeyValuePair<Strip, int>>() {
                new KeyValuePair<Strip, int>(new Strip(1, "strip 1", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 1),
                new KeyValuePair<Strip, int>(new Strip(2, "strip 2", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 5),
                new KeyValuePair<Strip, int>(new Strip(3, "strip 3", new Reeks(1, "reeks 1"), 1, new List<Auteur> { new Auteur(1, "auteur 1") }, new Uitgeverij(1, "uitgeverij 1")), 5)
            };

            _mainViewModel.GeselecteerdeStrips = _stripsEnAantallen;

        }

        [TestMethod]
        public void OverZichtViewModel_WanneerCatalogusManagerGelijkIsAanNull_GooitException()
        {
            ICatalogusManager invalidCatalogusManager = null;

            var ex = Assert.ThrowsException<NullReferenceException>(() => new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, invalidCatalogusManager, true, _mainViewModel));

            Assert.IsTrue(ex.Message.ToLower().Contains("catalogusmanager"));
        }

        [TestMethod]
        public void OverZichtViewModel_WanneerPopupBoxGelijkIsAanNull_GooitException()
        {
            IPopupBox invalidPopupBox = null;

            var ex = Assert.ThrowsException<NullReferenceException>(() => new OverzichtViewModel(invalidPopupBox, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, true, _mainViewModel));

            Assert.IsTrue(ex.Message.ToLower().Contains("popupbox"));
        }

        [TestMethod]
        public void OverZichtViewModel_WanneerVerkoopManagerGelijkIsAanNull_GooitException()
        {
            IPopupBox invalidPopupBox = null;

            var ex = Assert.ThrowsException<NullReferenceException>(() => new OverzichtViewModel(invalidPopupBox, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, true, _mainViewModel));

            Assert.IsTrue(ex.Message.ToLower().Contains("popupbox"));
        }

        [TestMethod]
        public void OverzichtViewModel_GaatAltijd_StripsEnAantallenInstellen()
        {

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, true, _mainViewModel);


            Assert.IsTrue(vm.StripsEnAantallen.SequenceEqual(_stripsEnAantallen));
        }

        [TestMethod]
        public void IncrementAantal_MetGeldigeStrip_VerhoogtAantalMet1()
        {

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, true, _mainViewModel);

            vm.IncrementAantalCommand.Execute(_stripsEnAantallen.First());

            Assert.AreEqual(expected: 2, actual: _stripsEnAantallen.First().Value);
            Assert.AreEqual(expected: 5, actual: _stripsEnAantallen[1].Value);
            Assert.AreEqual(expected: 5, actual: _stripsEnAantallen.Last().Value);
        }

        [TestMethod]
        public void DecrementAantal_MetGeldigeStrip_VerlaagtAantalMet1()
        {

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, true, _mainViewModel);

            vm.DecrementAantalCommand.Execute(_stripsEnAantallen.Last());

            Assert.AreEqual(expected: 1, actual: _stripsEnAantallen.First().Value);
            Assert.AreEqual(expected: 5, actual: _stripsEnAantallen[1].Value);
            Assert.AreEqual(expected: 4, actual: _stripsEnAantallen.Last().Value);
        }

        [TestMethod]
        public void OverzichtViewModel_WanneerIsLeveringGelijkIsAanTrue_SteltButtonTekstPropertyIn()
        {
            bool isLevering = true;

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            Assert.AreEqual(expected: "Bevestig Levering", actual: vm.ButtonTekst);
        }

        [TestMethod]
        public void OverzichtViewModel_WanneerIsLeveringGelijkIsAanFalse_SteltButtonTekstPropertyIn()
        {
            bool isLevering = false;

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            Assert.AreEqual(expected: "Bevestig Bestelling", actual: vm.ButtonTekst);
        }

        [TestMethod]
        public void Complete_WanneerStripsEnAantallenLeegIs_EnIsLeveringGelijkIsAanTrue_CalledShowErrorMessageMethodeVanPopupBox()
        {

            var stripsEnAantallenDatLeegIs = new ObservableCollection<KeyValuePair<Strip, int>>();

            _mainViewModel.GeselecteerdeStrips = stripsEnAantallenDatLeegIs;

            bool isLevering = true;
            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);

            vm.CompleteCommand.Execute(null);

            _fakePopupBox.Verify(fake => fake.ShowErrorMessage("Er kan geen lege levering geplaatst worden."), Times.Once);
        }

        [TestMethod]
        public void Complete_WanneerStripsEnAantallenLeegIs_EnIsLeveringGelijkIsAanFalse_CalledShowErrorMessageMethodeVanPopupBox()
        {
            var stripsEnAantallenDatLeegIs = new ObservableCollection<KeyValuePair<Strip, int>>();

            _mainViewModel.GeselecteerdeStrips = stripsEnAantallenDatLeegIs;

            bool isLevering = false;
            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);

            vm.CompleteCommand.Execute(null);

            _fakePopupBox.Verify(fake => fake.ShowErrorMessage("Er kan geen lege bestelling geplaatst worden."), Times.Once);
        }

        [TestMethod]
        public void Complete_WanneerDomeinlaagExceptionGooit_CalledShowErrorMessageMethodeVanPopupBox()
        {
            _fakeVerkoopManager.Setup(fake => fake.VoegLeveringToe(It.IsAny<Dictionary<Strip, int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws(new DomeinException("error message dat domeinlaag werd opgeworpen"));

            bool isLevering = true;

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            vm.CompleteCommand.Execute(null);

            _fakePopupBox.Verify(fake => fake.ShowErrorMessage("error message dat domeinlaag werd opgeworpen"), Times.Once);
        }

        [TestMethod]
        public void Complete_WanneerDomeinlaagGeenExceptionGooitEnIsLeveringGelijkIsAanTrue_CalledShowSuccessMessageMethodeVanPopupBox()
        {
            bool isLevering = true;

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            vm.CompleteCommand.Execute(null);

            _fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De levering werd in de database opgeslagen."), Times.Once);
        }

        [TestMethod]
        public void Complete_WanneerDomeinlaagGeenExceptionGooitEnIsLeveringGelijkIsAanFalse_CalledShowSuccessMessageMethodeVanPopupBox()
        {
            bool isLevering = false;

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            vm.CompleteCommand.Execute(null);

            _fakePopupBox.Verify(fake => fake.ShowSuccesMessage("De bestelling werd in de database opgeslagen."), Times.Once);
        }

        [TestMethod]
        public void ZijnStripsEnAantallenLeeg_WanneerStripsEnAantallenLeegIs_En_WanneerIsLeveringGelijkIsAanFalse_CalledShowErrorMessageVanPopupBox()
        {
            var stripsEnAantallenDatLeegIs = new ObservableCollection<KeyValuePair<Strip, int>>();

            _mainViewModel.GeselecteerdeStrips = stripsEnAantallenDatLeegIs;

            bool isLevering = false;
            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            vm.ZijnStripsEnAantallenLeeg();

            _fakePopupBox.Verify(fake => fake.ShowErrorMessage("Er kan geen lege bestelling geplaatst worden."), Times.Once);
        }

        [TestMethod]
        public void ZijnStripsEnAantallenLeeg_WanneerStripsEnAantallenLeegIs_En_WanneerIsLeveringGelijkIsAanFalse_RetourneertTrue()
        {
            var stripsEnAantallenDatLeegIs = new ObservableCollection<KeyValuePair<Strip, int>>();

            _mainViewModel.GeselecteerdeStrips = stripsEnAantallenDatLeegIs;

            bool isLevering = false;
            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            bool result = vm.ZijnStripsEnAantallenLeeg();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ZijnStripsEnAantallenLeeg_WanneerStripsEnAantallenLeegIs_En_WanneerIsLeveringGelijkIsAanTrue_CalledShowErrorMessageVanPopupBox()
        {
            var stripsEnAantallenDatLeegIs = new ObservableCollection<KeyValuePair<Strip, int>>();

            _mainViewModel.GeselecteerdeStrips = stripsEnAantallenDatLeegIs;

            bool isLevering = false;
            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            vm.ZijnStripsEnAantallenLeeg();

            _fakePopupBox.Verify(fake => fake.ShowErrorMessage("Er kan geen lege bestelling geplaatst worden."), Times.Once);
        }

        [TestMethod]
        public void ZijnStripsEnAantallenLeeg_WanneerStripsEnAantallenLeegIs_En_WanneerIsLeveringGelijkIsAanTrue_RetourneertTrue()
        {
            var stripsEnAantallenDatLeegIs = new ObservableCollection<KeyValuePair<Strip, int>>();

            _mainViewModel.GeselecteerdeStrips = stripsEnAantallenDatLeegIs;

            bool isLevering = false;
            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);


            vm.ZijnStripsEnAantallenLeeg();

            bool result = vm.ZijnStripsEnAantallenLeeg();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ZijnStripsEnAantallenLeeg_WanneerStripsEnAantallenLeegGroterIsDan1_RetourneertFalse()
        {
            bool isLevering = false;

            OverzichtViewModel vm = new OverzichtViewModel(_fakePopupBox.Object, _fakeVerkoopManager.Object, _fakeCatalogusManager.Object, isLevering, _mainViewModel);

            vm.ZijnStripsEnAantallenLeeg();

            bool result = vm.ZijnStripsEnAantallenLeeg();
            Assert.IsFalse(result);
        }
    }
}




