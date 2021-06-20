using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using Presentatie_laag.Interfaces;
using Presentatie_laag.ViewModels;
using Presentatie_laag.Views;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentatie_laag.ViewFactories
{
    public class ViewFactory : IViewFactory
    {
        private ICatalogusManager _manager;
        private IPopupBox _popupBox;
        public ViewFactory(ICatalogusManager manager, IPopupBox popupBox)
        {
            _manager = manager;
            _popupBox = popupBox;
        }

        public void MaakZoekStripView()
        {
            ZoekStripView view = new ZoekStripView(_manager, this, _popupBox);
            view.Show();
        }


        public void MaakVoegAuteurToeView()
        {
            VoegAuteurToeView view = new VoegAuteurToeView(_manager, _popupBox);
            view.ShowDialog();
        }

        public void MaakVoegAuteurToeView(VoegStripToeViewModel voegStripToeViewModel)
        {
            VoegAuteurToeView view = new VoegAuteurToeView(_manager, _popupBox, voegStripToeViewModel);
            view.ShowDialog();
        }

        public void MaakVoegReeksToeView()
        {
            VoegReeksToeView view = new VoegReeksToeView(_manager, _popupBox);
            view.ShowDialog();
        }

        public void MaakVoegReeksToeView(VoegStripToeViewModel voegStripToeViewModel)
        {
            VoegReeksToeView view = new VoegReeksToeView(_manager, _popupBox, voegStripToeViewModel);
            view.ShowDialog();
        }

        public void MaakVoegUitgeverijToeView()
        {
            VoegUitgeverijToeView view = new VoegUitgeverijToeView(_manager, _popupBox);
            view.ShowDialog();
        }
        public void MaakVoegUitgeverijToeView(VoegStripToeViewModel voegStripToeViewModel)
        {
            VoegUitgeverijToeView view = new VoegUitgeverijToeView(_manager, _popupBox, voegStripToeViewModel);
            view.ShowDialog();
        }

        public void MaakVoegStripToeView(Strip strip)
        {
            VoegStripToeView view = new VoegStripToeView(_manager, this, _popupBox, strip);
            view.ShowDialog();
        }

        public void MaakUpdateAuteurView()
        {
            UpdateAuteurView view = new UpdateAuteurView(_manager, _popupBox);
            view.ShowDialog();
        }
        public void MaakUpdateUitgeverijView()
        {
            UpdateUitgeverijView view = new UpdateUitgeverijView(_manager, _popupBox);
            view.ShowDialog();
        }
        public void MaakUpdateReeksView()
        {
            UpdateReeksView view = new UpdateReeksView(_manager, _popupBox);
            view.ShowDialog();
        }
    }
}
