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
    public class UpdateAuteurViewModel : ViewModelBase
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;
        public UpdateAuteurViewModel(ICatalogusManager manager, IPopupBox popupBox)
        {
            _catalogusManager = manager;
            _popupBox = popupBox;
            Auteurs = new ObservableCollection<Auteur>(manager.GeefAlleAuteurs());
            UpdateAuteurCommand = new RelayCommand(UpdateAuteur);
        }

        private ObservableCollection<Auteur> _auteurs;
        public ObservableCollection<Auteur> Auteurs
        {
            get => _auteurs;
            set
            {
                if (_auteurs != value)
                {
                    _auteurs = value;
                    RaisePropertyChanged(() => Auteurs);
                }
            }
        }

        public Auteur GeselecteerdeAuteur { get; set; }
        public string NieuweAuteurNaam { get; set; }
        public RelayCommand UpdateAuteurCommand { get; set; }

        private void UpdateAuteur()
        {
            if (GeselecteerdeAuteur == null)
                _popupBox.ShowErrorMessage("Gelieve een auteur te selecteren.");
            else
            {
                if (NieuweAuteurNaam != null)
                    NieuweAuteurNaam = NieuweAuteurNaam.Trim();
                try
                {
                    Auteur auteur = new Auteur(GeselecteerdeAuteur.Id, NieuweAuteurNaam);
                    _catalogusManager.UpdateAuteur(auteur);
                    _popupBox.ShowSuccesMessage("De auteur werd gewijzigd.");
                    Auteurs.Remove(GeselecteerdeAuteur);
                    Auteurs.Add(auteur);
                    Auteurs = new ObservableCollection<Auteur>(Auteurs.OrderBy(a => a.Naam));
                }
                catch (DomeinException ex)
                {
                    _popupBox.ShowErrorMessage(ex.Message);
                }
            }
        }
    }
}
