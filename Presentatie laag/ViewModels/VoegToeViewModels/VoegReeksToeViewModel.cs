using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Presentatie_laag.Interfaces;
using System;

namespace Presentatie_laag.ViewModels
{
    public class VoegReeksToeViewModel
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;
        private protected VoegStripToeViewModel _voegStripToeViewModel;
        public VoegReeksToeViewModel(ICatalogusManager catalogusManager, IPopupBox popupBox)
        {
            _catalogusManager = catalogusManager;
            _popupBox = popupBox;
            VoegReeksToeAanDbCommand = new RelayCommand(VoegReeksToeAanDb);
        }
        public VoegReeksToeViewModel(ICatalogusManager catalogusManager, IPopupBox popupBox, VoegStripToeViewModel voegStripToeViewModel) : this(catalogusManager, popupBox)
        {
            _voegStripToeViewModel = voegStripToeViewModel;
        }

        public string ToeTeVoegenReeksNaam { get; set; }
        public RelayCommand VoegReeksToeAanDbCommand { get; set; }

        private void VoegReeksToeAanDb()
        {
            try
            {
                if (ToeTeVoegenReeksNaam != null)
                    ToeTeVoegenReeksNaam = ToeTeVoegenReeksNaam.Trim();
                Reeks toegevoegdeReeks = _catalogusManager.VoegReeksToe(ToeTeVoegenReeksNaam);
                _popupBox.ShowSuccesMessage("De reeks werd toegevoegd aan de database.");
                if (_voegStripToeViewModel != null)
                    _voegStripToeViewModel.VoegReeksToe(toegevoegdeReeks);
            }
            catch (DomeinException ex)
            {
                _popupBox.ShowErrorMessage(ex.Message);
            }
        }
    }
}
