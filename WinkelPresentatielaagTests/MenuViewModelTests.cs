using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WinkelPresentatielaag.Interfaces;
using WinkelPresentatielaag.ViewModels;

namespace WinkelPresentatielaagTests
{
    [TestClass]
    public class MenuViewModelTests
    {
        private protected Mock<IViewFactory> _fakeViewFactory;
        public MenuViewModelTests()
        {
            _fakeViewFactory = new Mock<IViewFactory>();
        }

        [TestMethod]
        public void MenuViewModel_WanneerViewFactoryGelijkIsAanNull_GooitException()
        {
            IViewFactory invalidViewFactory = null;

            var ex = Assert.ThrowsException<ArgumentNullException>(() => new MenuViewModel(invalidViewFactory));
            Assert.IsTrue(ex.Message.ToLower().Contains("viewfactory"));
        }

        [TestMethod]
        public void OpenMainViewAlsLeveringView_GaatAltijd_ViewFactoryCallen()
        {
            MenuViewModel vm = new MenuViewModel(_fakeViewFactory.Object);
            vm.OpenMainViewAlsLeveringViewCommand.Execute(null);

            _fakeViewFactory.Verify(fake => fake.MaakMainView(true), Times.Once);
        }

        [TestMethod]
        public void OpenMainViewAlsBestellingView_GaatAltijd_ViewFactoryCallen()
        {
            MenuViewModel vm = new MenuViewModel(_fakeViewFactory.Object);
            vm.OpenMainViewAlsBestellingViewCommand.Execute(null);

            _fakeViewFactory.Verify(fake => fake.MaakMainView(false), Times.Once);
        }
    }
}

