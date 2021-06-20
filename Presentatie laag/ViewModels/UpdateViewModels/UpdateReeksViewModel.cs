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
    public class UpdateReeksViewModel : ViewModelBase
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;

        public UpdateReeksViewModel(ICatalogusManager manager, IPopupBox popupBox)
        {
            _catalogusManager = manager;
            _popupBox = popupBox;
            Reeksen = new ObservableCollection<Reeks>(manager.GeefAlleReeksen());
            UpdateReeksCommand = new RelayCommand(UpdateReeks);
        }

        private ObservableCollection<Reeks> _reeksen;
        public ObservableCollection<Reeks> Reeksen
        {
            get => _reeksen;
            set
            {
                if (_reeksen != value)
                {
                    _reeksen = value;
                    RaisePropertyChanged(() => Reeksen);
                }
            }
        }

        public Reeks GeselecteerdeReeks { get; set; }
        public string NieuweReeksNaam { get; set; }
        public RelayCommand UpdateReeksCommand { get; set; }

        private void UpdateReeks()
        {
            if (GeselecteerdeReeks == null)
                _popupBox.ShowErrorMessage("Gelieve een reeks te selecteren.");
            else
            {
                if (NieuweReeksNaam != null)
                    NieuweReeksNaam = NieuweReeksNaam.Trim();
                try
                {
                    Reeks reeks = new Reeks(GeselecteerdeReeks.Id, NieuweReeksNaam);
                    _catalogusManager.UpdateReeks(reeks);
                    _popupBox.ShowSuccesMessage("De reeks werd gewijzigd.");
                    Reeksen.Remove(GeselecteerdeReeks);
                    Reeksen.Add(reeks);
                    Reeksen = new ObservableCollection<Reeks>(Reeksen.OrderBy(r => r.Naam));
                }
                catch (DomeinException ex)
                {
                    _popupBox.ShowErrorMessage(ex.Message);
                }
            }
        }
    }
}
