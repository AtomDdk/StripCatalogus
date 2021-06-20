using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WinkelPresentatielaag.Interfaces;
using WinkelPresentatielaag.ViewModels;
using WinkelPresentatielaag.Views;

namespace WinkelPresentatielaag.Factories
{
    public class ViewFactory : IViewFactory
    {
        private protected IVerkoopManager _verkoopManager;
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;
        public ViewFactory(IPopupBox popupBox, ICatalogusManager catalogusManager, IVerkoopManager verkoopManager)
        {
            if (popupBox == null)
                throw new NullReferenceException("PopupBox mag niet null zijn.");
            _popupBox = popupBox;
            if (catalogusManager == null)
                throw new NullReferenceException("CatalogusManager mag niet null zijn.");
            _catalogusManager = catalogusManager;
            if (verkoopManager == null)
                throw new NullReferenceException("VerkoopManager mag niet null zijn.");
            _verkoopManager = verkoopManager;
        }


        public void MaakOverzichtView(MainViewModel mainViewModel, bool isLevering)
        {
            OverzichtView view = new OverzichtView(mainViewModel, _popupBox, _verkoopManager, _catalogusManager, isLevering);
            view.ShowDialog();
        }

        public void MaakMainView(bool isLevering)
        {
            MainView view = new MainView(_popupBox, _catalogusManager, _verkoopManager, this, isLevering);
            view.ShowDialog();
        }

        public void MaakMenuView()
        {
            MenuView view = new MenuView(this);
            view.Show();
        }
    }
}


