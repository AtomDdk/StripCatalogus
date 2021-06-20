using Domeinlaag;
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
    public class ZoekStripViewModel : ViewModelBase
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IViewFactory _viewFactory;
        private protected IPopupBox _popupBox;
        public ZoekStripViewModel(ICatalogusManager catalogusManager, IViewFactory viewFactory, IPopupBox popupBox)
        {
            _catalogusManager = catalogusManager;
            _viewFactory = viewFactory;
            _popupBox = popupBox;
            OpenUpdateStripViewCommand = new RelayCommand(OpenUpdateStripView);
            OpenVoegStripToeViewCommand = new RelayCommand(OpenVoegStripToeView);
            MaakAuteurSelectieOngedaanCommand = new RelayCommand(MaakAuteurSelectieOngedaan);
            MaakReeksSelectieOngedaanCommand = new RelayCommand(MaakReeksSelectieOngedaan);
            MaakUitgeverijSelectieOngedaanCommand = new RelayCommand(MaakUitgeverijSelectieOngedaan);
            OpenUpdateAuteurViewCommand = new RelayCommand(OpenUpdateAuteurView);
            OpenUpdateUitgeverijViewCommand = new RelayCommand(OpenUpdateUitgeverijView);
            OpenUpdateReeksViewCommand = new RelayCommand(OpenUpdateReeksView);
            OpenVoegReeksToeViewCommand = new RelayCommand(OpenVoegReeksToeView);
            OpenVoegUitgeverijToeViewCommand = new RelayCommand(OpenVoegUitgeverijToeView);
            OpenVoegAuteurToeViewCommand = new RelayCommand(OpenVoegAuteurToeView);
            ZoekStripCommand = new RelayCommand(ZoekStrip);
            Strips = new ObservableCollection<Strip>(_catalogusManager.GeefAlleStrips());
            Uitgeverijen = new ObservableCollection<Uitgeverij>(_catalogusManager.GeefAlleUitgeverijen());
            Auteurs = new ObservableCollection<Auteur>(_catalogusManager.GeefAlleAuteurs());
            Reeksen = new ObservableCollection<Reeks>(_catalogusManager.GeefAlleReeksen());
            Reeksen.Insert(0, new Reeks("<Geen Reeks>"));
            ZoekStripArguments = new ZoekStripArguments();
        }

        public string GezochtReeksNummer { get; set; }
        public int? IndexVanGezochteAuteur { get; set; }
        public int? IndexVanGezochteReeks { get; set; }
        public int? IndexVanGezochteUitgeverij { get; set; }
        public Strip GeselecteerdeStrip { get; set; }

        #region info
        // Bevat alle strips uit de database, hiermee wordt de DataGrid in het ZoekStripView-venster opgevuld.
        #endregion
        private ObservableCollection<Strip> _strips;
        public ObservableCollection<Strip> Strips
        {
            get => _strips;
            set
            {
                if (_strips != value)
                {
                    _strips = value;
                    RaisePropertyChanged(() => Strips);
                }
            }
        }
        #region info
        // Bevat alle reeksen uit de database, hiermee wordt de Reeks-ComboBox in het ZoekStripView-venster opgevuld.
        #endregion
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

        #region info
        // Bevat alle uitgeverijen uit de database, hiermee wordt de Uitgeverij-ComboBox in het ZoekStripView-venster opgevuld.
        #endregion
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

        #region info
        // Bevat alle auteurs uit de database, hiermee wordt de Auteurs-ComboBox in het ZoekStripView-venster opgevuld.
        #endregion
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
        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Update Geselecteerde Strip'-knop in het ZoekStripView-venster.
           De methode roept de factory aan om een nieuw VoegStripToeView-venster te maken en te tonen op het scherm. De geselecteerde strip wordt aan dat nieuwe venster doorgegeven.
           De RefreshStripLijst-methode wordt aangeroepen zodra het UpdateStripView-venster sluit, zodat het ZoekStripView-venster de updated strips bevat.
        */
        #endregion
        public RelayCommand OpenUpdateStripViewCommand { get; set; }
        private void OpenUpdateStripView()
        {
            if (GeselecteerdeStrip == null)
            {
                _popupBox.ShowErrorMessage("Gelieve eerst een strip te selecteren.");
            }
            else
            {
                Strip s = GeselecteerdeStrip;
                GeselecteerdeStrip = null;
                RaisePropertyChanged(() => GeselecteerdeStrip);
                _viewFactory.MaakVoegStripToeView(s);
                RefreshAlleCollecties();
            }
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Voeg Strip Toe'-knop in het ZoekStripView-venster.
           De methode roept de factory aan, de factory maakt en toont een VoegStripToeView-venster.
           De RefreshStripLijst-methode wordt aangeroepen zodra het VoegStripToeView-venster sluit, zodat het ZoekStripView-venster de toegevoegde strips bevat.
        */
        #endregion
        public RelayCommand OpenVoegStripToeViewCommand { get; set; }
        private void OpenVoegStripToeView()
        {
            _viewFactory.MaakVoegStripToeView(null);
            RefreshAlleCollecties();
        }

        public ZoekStripArguments ZoekStripArguments { get; set; }

        #region info
        // Onderstaande methode wordt uitgevoerd wanneer er op de ZoekStrip-knop in het ZoekStripView-venster wordt geduwd.
        #endregion
        public RelayCommand ZoekStripCommand { get; set; }
        private void ZoekStrip()
        {
            if (IndexVanGezochteReeks == 0)
                ZoekStripArguments.Reeks = null;
            if (string.IsNullOrWhiteSpace(GezochtReeksNummer) || !int.TryParse(GezochtReeksNummer, out int gezochtReeksNr))
                ZoekStripArguments.ReeksNummer = null;
            else
                ZoekStripArguments.ReeksNummer = gezochtReeksNr;

            if (GezochtReeksNummer.Any(x => char.IsLetter(x)))
                Strips = new ObservableCollection<Strip>();
            else
            {
                try
                {
                    Strips = new ObservableCollection<Strip>(_catalogusManager.ZoekStrips(ZoekStripArguments));
                }
                catch (DomeinException ex)
                {
                    _popupBox.ShowErrorMessage(ex.Message);
                }
            }
        }

        #region info
        // Onderstaande Refresh-methodes worden gebruikt om alle collecties uit de db opnieuw op te vragen.
        #endregion
        public void RefreshAlleCollecties()
        {
            RefreshStrips();
            RefreshReeksen();
            RefreshUitgeverijen();
            RefreshAuteurs();
        }

        public void RefreshReeksen()
        {
            Reeksen = new ObservableCollection<Reeks>(_catalogusManager.GeefAlleReeksen());
            Reeksen.Insert(0, new Reeks("<Geen Reeks>"));
        }

        public void RefreshUitgeverijen()
        { 
            Uitgeverijen = new ObservableCollection<Uitgeverij>(_catalogusManager.GeefAlleUitgeverijen());
        }

        public void RefreshAuteurs()
        { 
            Auteurs = new ObservableCollection<Auteur>(_catalogusManager.GeefAlleAuteurs());
        }

        public void RefreshStrips()
        { 
            Strips = new ObservableCollection<Strip>(_catalogusManager.GeefAlleStrips());
        }

        #region info
        // Onderstaande methode wordt uitgevoerd wanneer er op de X-knop naast de Uitgeverij-ComboBox wordt geduwd.
        // De methode zorgt ervoor dat de geselecteerde uitgeverij in de Uitgeverij-ComboBox niet meer geselecteerd is.
        #endregion
        public RelayCommand MaakUitgeverijSelectieOngedaanCommand { get; set; }
        private void MaakUitgeverijSelectieOngedaan()
        {
            IndexVanGezochteUitgeverij = -1;
            RaisePropertyChanged(() => IndexVanGezochteUitgeverij);
            ZoekStripArguments.FilterOpUitgeverij = false;
        }

        #region info
        // Onderstaande methode wordt uitgevoerd wanneer er op de X-knop naast de Reeks-ComboBox wordt geduwd.
        // De methode zorgt ervoor dat de geselecteerde reeks in de Reeks-ComboBox niet meer geselecteerd is.
        #endregion
        public RelayCommand MaakReeksSelectieOngedaanCommand { get; set; }
        private void MaakReeksSelectieOngedaan()
        {
            IndexVanGezochteReeks = -1;
            RaisePropertyChanged(() => IndexVanGezochteReeks);
            ZoekStripArguments.FilterOpReeks = false;
        }

        #region info
        // Onderstaande methode wordt uitgevoerd wanneer er op de X-knop naast de Auteur-ComboBox wordt geduwd.
        // De methode zorgt ervoor dat de geselecteerde auteur in de Auteur-ComboBox niet meer geselecteerd is.
        #endregion
        public RelayCommand MaakAuteurSelectieOngedaanCommand { get; set; }
        private void MaakAuteurSelectieOngedaan()
        {
            IndexVanGezochteAuteur = -1;
            RaisePropertyChanged(() => IndexVanGezochteAuteur);
            ZoekStripArguments.FilterOpAuteur = false;
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Update Auteur'-knop in het ZoekStripView-venster.
           De methode roept de factory aan, de factory maakt en toont een UpdateAuteurView-venster.
           De RefreshStripLijst-methode wordt aangeroepen zodra het UpdateAuteurView-venster sluit, zodat het ZoekStripView-venster de updated auteurs bevat.
        */
        #endregion
        public RelayCommand OpenUpdateAuteurViewCommand { get; set; }
        private void OpenUpdateAuteurView()
        {
            _viewFactory.MaakUpdateAuteurView();
            RefreshAuteurs();
            RefreshStrips();
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Update Uitgeverij'-knop in het ZoekStripView-venster.
           De methode roept de factory aan, de factory maakt en toont een UpdateUitgeverijView-venster.
           De RefreshStripLijst-methode wordt aangeroepen zodra het UpdateUitgeverijView-venster sluit, zodat het ZoekStripView-venster de updated uitgeverijen bevat.
        */
        #endregion
        public RelayCommand OpenUpdateUitgeverijViewCommand { get; set; }
        private void OpenUpdateUitgeverijView()
        {
            _viewFactory.MaakUpdateUitgeverijView();
            RefreshUitgeverijen();
            RefreshStrips();
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Update Reeks'-knop in het ZoekStripView-venster.
           De methode roept de factory aan, de factory maakt en toont een UpdateReeksView-venster.
           De RefreshStripLijst-methode wordt aangeroepen zodra het UpdateReeksView-venster sluit, zodat het ZoekStripView-venster de updated reeksen bevat.
        */
        #endregion
        public RelayCommand OpenUpdateReeksViewCommand { get; set; }
        private void OpenUpdateReeksView()
        {
            _viewFactory.MaakUpdateReeksView();
            RefreshReeksen();
            RefreshStrips();
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Voeg Reeks Toe'-knop in het ZoekStripView-venster.
           De methode roept de factory aan, de factory maakt en toont een VoegReeksToeView-venster.
           De RefreshStripLijst-methode wordt aangeroepen zodra het VoegReeksToeView-venster sluit, zodat het ZoekStripView-venster de toegevoegde reeksen bevat.
        */
        #endregion
        public RelayCommand OpenVoegReeksToeViewCommand { get; set; }
        private void OpenVoegReeksToeView()
        {
            _viewFactory.MaakVoegReeksToeView();
            RefreshReeksen();
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Voeg Uitgeverij Toe'-knop in het ZoekStripView-venster.
           De methode roept de factory aan, de factory maakt en toont een VoegUitgeverijToeView-venster.
           De RefreshStripLijst-methode wordt aangeroepen zodra het VoegUitgeverijToeView-venster sluit, zodat het ZoekStripView-venster de toegevoegde uitgeverijen bevat.
        */
        #endregion
        public RelayCommand OpenVoegUitgeverijToeViewCommand { get; set; }
        private void OpenVoegUitgeverijToeView()
        {
            _viewFactory.MaakVoegUitgeverijToeView();
            RefreshUitgeverijen();
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de 'Voeg Auteur Toe'-knop in het ZoekStripView-venster.
           De methode roept de factory aan, de factory maakt en toont een VoegAuteurToeView-venster.
           De RefreshStripLijst-methode wordt aangeroepen zodra het VoegAuteurToeView-venster sluit, zodat het ZoekStripView-venster de toegevoegde auteurs bevat.
        */
        #endregion
        public RelayCommand OpenVoegAuteurToeViewCommand { get; set; }
        private void OpenVoegAuteurToeView()
        {
            _viewFactory.MaakVoegAuteurToeView();
            RefreshAuteurs();
        }
    }
}
