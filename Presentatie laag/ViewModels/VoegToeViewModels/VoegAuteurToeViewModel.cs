using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Presentatie_laag.Interfaces;
using System;

namespace Presentatie_laag.ViewModels
{
    public class VoegAuteurToeViewModel
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;
        private protected VoegStripToeViewModel _voegStripToeViewModel;
        public VoegAuteurToeViewModel(ICatalogusManager catalogusManager, IPopupBox popupBox)
        {
            VoegAuteurToeAanDbCommand = new RelayCommand(VoegAuteurToeAanDb);
            _catalogusManager = catalogusManager;
            _popupBox = popupBox;
        }
        public VoegAuteurToeViewModel(ICatalogusManager catalogusManager, IPopupBox popupBox, VoegStripToeViewModel voegStripToeViewModel) : this(catalogusManager, popupBox)
        {
            _voegStripToeViewModel = voegStripToeViewModel;
        }

        public string ToeTeVoegenAuteurNaam { get; set; }
        public RelayCommand VoegAuteurToeAanDbCommand { get; set; }

        private void VoegAuteurToeAanDb()
        {
            try
            {
                if (ToeTeVoegenAuteurNaam != null)
                    ToeTeVoegenAuteurNaam = ToeTeVoegenAuteurNaam.Trim();
                Auteur toegevoegdeAuteur = _catalogusManager.VoegAuteurToe(ToeTeVoegenAuteurNaam);
                _popupBox.ShowSuccesMessage("De auteur werd toegevoegd aan de database.");
                if (_voegStripToeViewModel != null)
                    _voegStripToeViewModel.VoegAuteurToeAanGeselecteerdeAuteurs(toegevoegdeAuteur);
            }
            catch (DomeinException ex)
            {
                _popupBox.ShowErrorMessage(ex.Message);
            }
        }
    }
}


