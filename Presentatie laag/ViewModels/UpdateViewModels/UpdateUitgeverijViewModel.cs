using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Presentatie_laag.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Presentatie_laag.ViewModels
{
    public class UpdateUitgeverijViewModel : ViewModelBase
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;
        public UpdateUitgeverijViewModel(ICatalogusManager manager, IPopupBox popupBox)
        {
            _catalogusManager = manager;
            _popupBox = popupBox;
            Uitgeverijen = new ObservableCollection<Uitgeverij>(manager.GeefAlleUitgeverijen());
            UpdateUitgeverijCommand = new RelayCommand(UpdateUitgeverij);
        }

        private ObservableCollection<Uitgeverij> _uitgeverijen;
        public ObservableCollection<Uitgeverij> Uitgeverijen
        {
            get => _uitgeverijen;
            set
            {
                if (_uitgeverijen != value)
                {
                    _uitgeverijen = value;
                    RaisePropertyChanged(() => Uitgeverijen);
                }
            }
        }

        public Uitgeverij GeselecteerdeUitgeverij { get; set; }
        public string NieuweUitgeverijNaam { get; set; }
        public RelayCommand UpdateUitgeverijCommand { get; set; }

        private void UpdateUitgeverij()
        {
            if (GeselecteerdeUitgeverij == null)
                _popupBox.ShowErrorMessage("Gelieve een uitgeverij te selecteren.");
            else
            {
                if (NieuweUitgeverijNaam != null)
                    NieuweUitgeverijNaam = NieuweUitgeverijNaam.Trim();
                try
                {
                    Uitgeverij uitgeverij = new Uitgeverij(GeselecteerdeUitgeverij.Id, NieuweUitgeverijNaam);
                    _catalogusManager.UpdateUitgeverij(uitgeverij);
                    _popupBox.ShowSuccesMessage("De uitgeverij werd gewijzigd.");
                    Uitgeverijen.Remove(GeselecteerdeUitgeverij);
                    Uitgeverijen.Add(uitgeverij);
                    Uitgeverijen = new ObservableCollection<Uitgeverij>(Uitgeverijen.OrderBy(u => u.Naam));
                }
                catch (DomeinException ex)
                {
                    _popupBox.ShowErrorMessage(ex.Message);
                }
            }
        }
    }
}
