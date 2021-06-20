using Domeinlaag.Exceptions;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Presentatie_laag.Interfaces;
using System;

namespace Presentatie_laag.ViewModels
{
    public class VoegUitgeverijToeViewModel
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IPopupBox _popupBox;
        private protected VoegStripToeViewModel _voegStripToeViewModel;
        public VoegUitgeverijToeViewModel(ICatalogusManager catalogusManager, IPopupBox popupBox)
        {
            _catalogusManager = catalogusManager;
            _popupBox = popupBox;
            VoegUitgeverijToeAanDbCommand = new RelayCommand(VoegUitgeverijToeAanDb);
        }
        public VoegUitgeverijToeViewModel(ICatalogusManager catalogusManager, IPopupBox popupBox, VoegStripToeViewModel voegStripToeViewModel) : this(catalogusManager, popupBox)
        {
            _voegStripToeViewModel = voegStripToeViewModel;
        }

        public string ToeTeVoegenUitgeverijNaam { get; set; }
        public RelayCommand VoegUitgeverijToeAanDbCommand { get; set; }

        private void VoegUitgeverijToeAanDb()
        {
            try
            {
                if (ToeTeVoegenUitgeverijNaam != null)
                    ToeTeVoegenUitgeverijNaam = ToeTeVoegenUitgeverijNaam.Trim();
                Uitgeverij toegevoegdeUitgeverij = _catalogusManager.VoegUitgeverijToe(ToeTeVoegenUitgeverijNaam);
                _popupBox.ShowSuccesMessage("De uitgeverij werd toegevoegd aan de database.");
                if (_voegStripToeViewModel != null)
                    _voegStripToeViewModel.VoegUitgeverijToe(toegevoegdeUitgeverij);
            }
            catch (DomeinException ex)
            {
                _popupBox.ShowErrorMessage(ex.Message);
            }
        }
    }
}
