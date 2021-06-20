using Domeinlaag.Model;
using Presentatie_laag.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentatie_laag.Interfaces
{
    public interface IViewFactory
    {
        void MaakZoekStripView();
        void MaakVoegAuteurToeView();
        void MaakVoegAuteurToeView(VoegStripToeViewModel voegStripToeViewModel);
        void MaakVoegReeksToeView();
        void MaakVoegReeksToeView(VoegStripToeViewModel voegStripToeViewModel);
        void MaakVoegUitgeverijToeView();
        void MaakVoegUitgeverijToeView(VoegStripToeViewModel voegStripToeViewModel);
        void MaakVoegStripToeView(Strip strip);
        void MaakUpdateAuteurView();
        void MaakUpdateUitgeverijView();
        void MaakUpdateReeksView();
    }
}


