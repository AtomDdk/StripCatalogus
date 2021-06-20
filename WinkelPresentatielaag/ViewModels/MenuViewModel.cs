using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using WinkelPresentatielaag.Factories;
using WinkelPresentatielaag.Interfaces;

namespace WinkelPresentatielaag.ViewModels
{
    public class MenuViewModel
    {
        private protected IViewFactory _viewFactory;
        public MenuViewModel(IViewFactory viewFactory)
        {
            if (viewFactory == null)
                throw new ArgumentNullException("ViewFactory mag niet null zijn");
            _viewFactory = viewFactory;
            OpenMainViewAlsLeveringViewCommand = new RelayCommand(OpenMainViewAlsLeveringView);
            OpenMainViewAlsBestellingViewCommand = new RelayCommand(OpenMainViewAlsBestellingView);
        }

        public RelayCommand OpenMainViewAlsLeveringViewCommand { get; set; }
        public RelayCommand OpenMainViewAlsBestellingViewCommand { get; set; }

        private void OpenMainViewAlsLeveringView()
        {
            _viewFactory.MaakMainView(isLevering: true);
        }

        private void OpenMainViewAlsBestellingView()
        {
            _viewFactory.MaakMainView(isLevering: false);
        }
    }
}

