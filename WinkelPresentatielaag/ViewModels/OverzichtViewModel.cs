using Domeinlaag;
using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WinkelPresentatielaag.Interfaces;

namespace WinkelPresentatielaag.ViewModels
{
    public class OverzichtViewModel
    {
        private protected MainViewModel _mainViewModel;
        private protected IVerkoopManager _verkoopManager;
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;
        public OverzichtViewModel(IPopupBox popupBox, IVerkoopManager verkoopManager, ICatalogusManager catalogusManager, bool isLevering, MainViewModel mainViewModel)
        {
            if (popupBox == null)
                throw new NullReferenceException("PopupBox mag niet null zijn.");
            _popupBox = popupBox;
            if (verkoopManager == null)
                throw new NullReferenceException("VerkoopManager mag niet null zijn.");
            _verkoopManager = verkoopManager;
            if (catalogusManager == null)
                throw new NullReferenceException("CatalogusManager mag niet null zijn.");
            _catalogusManager = catalogusManager;
            if (mainViewModel == null)
                throw new NullReferenceException("MainViewModel mag niet null zijn.");
            _mainViewModel = mainViewModel;
            StripsEnAantallen = mainViewModel.GeselecteerdeStrips;
            IsLevering = isLevering;
            IncrementAantalCommand = new RelayCommand<KeyValuePair<Strip, int>>(IncrementAantal);
            DecrementAantalCommand = new RelayCommand<KeyValuePair<Strip, int>>(DecrementAantal);
            CompleteCommand = new RelayCommand<IClosable>(Complete);
            StelButtonTekstIn();
            BestelDatum = DateTime.Now;
            LeverDatum = DateTime.Now;
        }

        public bool IsLevering { get; set; }
        public ObservableCollection<KeyValuePair<Strip, int>> StripsEnAantallen { get; set; }
        public RelayCommand<KeyValuePair<Strip, int>> IncrementAantalCommand { get; set; }
        private void IncrementAantal(KeyValuePair<Strip, int> keyValuePair)
        {
            if (keyValuePair.Key != null)
            {
                int i = StripsEnAantallen.IndexOf(keyValuePair);
                StripsEnAantallen[i] = new KeyValuePair<Strip, int>(keyValuePair.Key, keyValuePair.Value + 1);
                _mainViewModel.GeselecteerdeStripMetAantal = StripsEnAantallen[i];
            }
        }

        public RelayCommand<KeyValuePair<Strip, int>> DecrementAantalCommand { get; set; }
        private void DecrementAantal(KeyValuePair<Strip, int> keyValuePair)
        {
            if (keyValuePair.Key != null)
            {
                int i = StripsEnAantallen.IndexOf(keyValuePair);
                if (keyValuePair.Value > 1)
                {
                    StripsEnAantallen[i] = new KeyValuePair<Strip, int>(keyValuePair.Key, keyValuePair.Value - 1);
                    _mainViewModel.GeselecteerdeStripMetAantal = StripsEnAantallen[i];
                }
                else
                {
                    StripsEnAantallen.RemoveAt(i);
                    _mainViewModel.GeselecteerdeStripMetAantal = default;
                }
            }
        }

        public string ButtonTekst { get; set; }
        public RelayCommand<IClosable> CompleteCommand { get; set; }
        public DateTime BestelDatum { get; set; }
        public DateTime LeverDatum { get; set; }

        public bool ZijnStripsEnAantallenLeeg()
        {
            if (StripsEnAantallen.Count < 1)
            {
                if (IsLevering)
                    _popupBox.ShowErrorMessage("Er kan geen lege levering geplaatst worden.");
                else
                    _popupBox.ShowErrorMessage("Er kan geen lege bestelling geplaatst worden.");
                return true;
            }
            return false;
        }

        private void Complete(IClosable view)
        {
            if (!ZijnStripsEnAantallenLeeg())
            {
                try
                {
                    Dictionary<Strip, int> stripsEnAantallen = StripsEnAantallen.ToDictionary(pair => pair.Key, pair => pair.Value);

                    if (IsLevering)
                    {
                        _verkoopManager.VoegLeveringToe(stripsEnAantallen, BestelDatum, LeverDatum);
                        _popupBox.ShowSuccesMessage("De levering werd in de database opgeslagen.");
                        _mainViewModel.RefreshAlleCollecties();
                        if (view != null)
                            view.Close();
                    }
                    else
                    {
                        _verkoopManager.VoegBestellingToe(stripsEnAantallen);
                        _popupBox.ShowSuccesMessage("De bestelling werd in de database opgeslagen.");
                        _mainViewModel.RefreshAlleCollecties();
                        if (view != null)
                            view.Close();
                    }
                }
                catch (DomeinException ex)
                {
                    _popupBox.ShowErrorMessage(ex.Message);
                }
            }
        }

        private void StelButtonTekstIn()
        {
            if (IsLevering == true)
                ButtonTekst = "Bevestig Levering";
            else
                ButtonTekst = "Bevestig Bestelling";
        }
    }
}
