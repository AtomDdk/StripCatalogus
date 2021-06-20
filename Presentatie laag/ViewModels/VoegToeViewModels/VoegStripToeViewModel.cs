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
    public class VoegStripToeViewModel : ViewModelBase
    {
        private protected ICatalogusManager _catalogusManager;
        private protected IViewFactory _viewFactory;
        private protected IPopupBox _popupBox;
        public VoegStripToeViewModel(ICatalogusManager catalogusManager, IViewFactory viewFactory, IPopupBox popupBox, Strip teUpdatenStrip)
        {
            _catalogusManager = catalogusManager;
            _viewFactory = viewFactory;
            _popupBox = popupBox;
            OpenVoegAuteurToeViewCommand = new RelayCommand(OpenVoegAuteurToeView);
            OpenVoegUitgeverijToeViewCommand = new RelayCommand(OpenVoegUitgeverijToeView);
            OpenVoegReeksToeViewCommand = new RelayCommand(OpenVoegReeksToeView);
            VoegAuteurToeAanGeselecteerdeAuteursCommand = new RelayCommand<Auteur>(VoegAuteurToeAanGeselecteerdeAuteurs);
            VerwijderAuteurUitGeselecteerdeAuteursCommand = new RelayCommand<Auteur>(VerwijderAuteurUitGeselecteerdeAuteurs);
            GeselecteerdeAuteurs = new ObservableCollection<Auteur>();
            CompleteCommand = new RelayCommand(Complete);
            Uitgeverijen = new ObservableCollection<Uitgeverij>(_catalogusManager.GeefAlleUitgeverijen());
            Reeks geenReeks = new Reeks("<Geen Reeks>");
            GeselecteerdeReeks = geenReeks;
            Reeksen = new ObservableCollection<Reeks>(_catalogusManager.GeefAlleReeksen());
            Reeksen.Insert(0, geenReeks);
            ReeksNummer = string.Empty;
            Auteurs = new ObservableCollection<Auteur>(_catalogusManager.GeefAlleAuteurs());
            OntvangTeUpdatenStrip(teUpdatenStrip);
        }

        private string _buttonContentText;
        public string ButtonContentText
        {
            get => _buttonContentText;
            set
            {
                if (_buttonContentText != value)
                {
                    _buttonContentText = value;
                    RaisePropertyChanged(() => ButtonContentText);
                }
            }
        }

        #region info
        /* Onderstaande property is gebind aan de titel-TextBox uit het VoegStripToeView-venster.
         * Hierin typt de gebruiker de titel in van de toe te voegen strip of van de up te daten strip.
         */
        #endregion
        private string _stripTitel;
        public string StripTitel
        {
            get => _stripTitel;
            set
            {
                if (_stripTitel != value)
                {
                    _stripTitel = value;
                    RaisePropertyChanged(() => StripTitel);
                }
            }
        }

        #region info
        /* Onderstaande property is gebind aan de reeksNummer-TextBox uit het VoegStripToeView-venster.
         * Hierin typt de gebruiker het reeksnummer in van de toe te voegen strip of van de up te daten strip.
         */
        #endregion
        private string _reeksNummer;
        public string ReeksNummer
        {
            get => _reeksNummer;
            set
            {
                if (_reeksNummer != value)
                {
                    _reeksNummer = value;
                    RaisePropertyChanged(() => ReeksNummer);
                }
            }
        }

        #region info
        /* Onderstaande property is gebind aan het geselecteerde item van de Uitgeverij-ComboBox uit het VoegStripToeView-venster.
         * Als de gebruiker een uitgeverij selecteerd uit de ComboBox, zal onderstaande property de geselecteerde uitgeverij bevatten.
         */
        #endregion
        private Uitgeverij _geselecteerdeUitgeverij;
        public Uitgeverij GeselecteerdeUitgeverij
        {
            get => _geselecteerdeUitgeverij;
            set
            {
                if (_geselecteerdeUitgeverij != value)
                {
                    _geselecteerdeUitgeverij = value;
                    RaisePropertyChanged(() => GeselecteerdeUitgeverij);
                }
            }
        }

        #region info
        /* Onderstaande property is gebind aan het geselecteerde item van de Reeks-ComboBox uit het VoegStripToeView-venster.
         * Als de gebruiker een reeks selecteerd uit de ComboBox, zal onderstaande property de geselecteerde reeks bevatten.
         */
        #endregion
        private Reeks _geselecteerdeReeks;
        public Reeks GeselecteerdeReeks
        {
            get => _geselecteerdeReeks;
            set
            {
                if (_geselecteerdeReeks != value)
                {
                    _geselecteerdeReeks = value;
                    RaisePropertyChanged(() => GeselecteerdeReeks);
                }
            }
        }

        #region info
        // Onderstaande property bevat alle uitgeverijen uit de database, en wordt gebruikt om de Uitgeverij-ComboBox op te vullen.         
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
        // Onderstaande property bevat alle reeksen uit de database, en wordt gebruikt om de Reeks-ComboBox op te vullen.         
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
        // Onderstaande property bevat alle Auteurs uit de database, en wordt gebruikt om het Auteurs-vak aan de linkerkant van het VoegStripToeView-venster op te vullen.         
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
        // Onderstaande property bevat de auteurs dat de gebruiker heeft verplaatst van het Auteurs-vak aan de linkerkant naar het Auteurs-vak aan de rechterkant van het VoegStripToeView-venster.     
        #endregion
        private ObservableCollection<Auteur> _geselecteerdeAuteurs;
        public ObservableCollection<Auteur> GeselecteerdeAuteurs
        {
            get => _geselecteerdeAuteurs;
            set
            {
                if (_geselecteerdeAuteurs != value)
                {
                    _geselecteerdeAuteurs = value;
                    RaisePropertyChanged(() => GeselecteerdeAuteurs);
                }
            }
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de '+'-knop naar de Auteurs-ComboBox in het VoegStripToeView-venster.
           De methode roept de factory aan, de factory maakt en toont een VoegAuteurToeView-venster.
           Aan de factory methode wordt dit ViewModel (this) meegegeven.
           Hierdoor kan het gemaakte VoegAuteurToeView-venster de auteur die werd toegevoegd aan de database,
           ook toevoegen aan bovenstaande GeselecteerdeAuteurs-property zodat de auteur die werd toegevoegd aan de database verschijnt in het rechter Auteur-vak van het VoegStripToeView-venster.
        */
        #endregion
        public RelayCommand OpenVoegAuteurToeViewCommand { get; set; }
        private void OpenVoegAuteurToeView()
        {
            _viewFactory.MaakVoegAuteurToeView(this);
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de '+'-knop naar de Reeks-ComboBox in het VoegStripToeView-venster.
           De methode roept de factory aan, de factory maakt en toont een VoegReeksToeView-venster.
           Aan de factory methode wordt dit ViewModel (this) meegegeven.
           Hierdoor kan het gemaakte VoegReeksToeView-venster de reeks die werd toegevoegd aan de database,
           ook toevoegen aan bovenstaande Reeksen-property zodat de reeks die werd toegevoegd aan de database verschijnt als geselecteerd item in de Reeks-Combobox van het VoegStripToeView-venster.
        */
        #endregion
        public RelayCommand OpenVoegReeksToeViewCommand { get; set; }
        private void OpenVoegReeksToeView()
        {
            _viewFactory.MaakVoegReeksToeView(this);
        }

        public void VoegReeksToe(Reeks reeks)
        {
            if (reeks != null)
            {
                Reeksen.Add(reeks);
                GeselecteerdeReeks = reeks;
                Reeksen = new ObservableCollection<Reeks>(Reeksen.OrderBy(reeks => reeks.Naam));
            }
        }

        #region info
        /* Onderstaande methode wordt uitgevoerd wanneer er geklikt wordt op de '+'-knop naar de Uitgeverij-ComboBox in het VoegStripToeView-venster.
           De methode roept de factory aan, de factory maakt en toont een VoegUitgeverijToeView-venster.
           Aan de factory methode wordt dit ViewModel (this) meegegeven.
           Hierdoor kan het gemaakte VoegUitgeverijToeView-venster de uitgeverij die werd toegevoegd aan de database,
           ook toevoegen aan bovenstaande Uitgeverijen-property zodat de uitgeverij die werd toegevoegd aan de database verschijnt als geselecteerd item in de Uitgeverij-Combobox van het VoegStripToeView-venster.
        */
        #endregion
        public RelayCommand OpenVoegUitgeverijToeViewCommand { get; set; }
        private void OpenVoegUitgeverijToeView()
        {
            _viewFactory.MaakVoegUitgeverijToeView(this);
        }

        public void VoegUitgeverijToe(Uitgeverij uitgeverij)
        {
            if (uitgeverij != null)
            {
                Uitgeverijen.Add(uitgeverij);
                GeselecteerdeUitgeverij = uitgeverij;
                Uitgeverijen = new ObservableCollection<Uitgeverij>(Uitgeverijen.OrderBy(uitgeverij => uitgeverij.Naam));
            }
        }

        #region
        // Onderstaande methode wordt uitgevoerd wanneer er op de 'Voeg Strip Toe'-knop wordt geduwt uit het VoegStripToeView-venster.
        #endregion
        public RelayCommand CompleteCommand { get; set; }
        private void Complete()
        {
            try
            {
                if (ReeksNummerBevatLetters())
                    _popupBox.ShowErrorMessage("Het reeksnr mag geen letters bevatten.");
                else
                {
                    if (TeUpdatenStrip == null)
                    {
                        VoegStripToe();
                        _popupBox.ShowSuccesMessage("De strip werd toegevoegd aan de database.");
                    }
                    else
                    {
                        UpdateStrip();
                        _popupBox.ShowSuccesMessage("De strip werd gewijzigd in de database.");
                    }
                }
            }
            catch (DomeinException ex)
            {
                _popupBox.ShowErrorMessage(ex.Message);
            }
        }

        private bool ReeksNummerBevatLetters()
        {
            return ReeksNummer.Any(x => char.IsLetter(x));
        }

        private void VoegStripToe()
        {
            int? reeksNummer = ParseReeksNummer();
            Reeks geselecteerdeReeks = ParseReeks();
            if (StripTitel != null)
                StripTitel = StripTitel.Trim();
            _catalogusManager.VoegStripToe(StripTitel, geselecteerdeReeks, reeksNummer, GeselecteerdeAuteurs.ToList(), GeselecteerdeUitgeverij);
        }

        private void UpdateStrip()
        {
            int? reeksNummer = ParseReeksNummer();
            Reeks geselecteerdeReeks = ParseReeks();
            TeUpdatenStrip.StelAuteursIn(GeselecteerdeAuteurs.ToList());
            TeUpdatenStrip.VeranderReeksEnReeksNummer(geselecteerdeReeks, reeksNummer);
            if (StripTitel != null)
                StripTitel = StripTitel.Trim();
            TeUpdatenStrip.Titel = StripTitel;
            TeUpdatenStrip.Uitgeverij = GeselecteerdeUitgeverij;
            _catalogusManager.UpdateStrip(TeUpdatenStrip);
        }

        private int? ParseReeksNummer()
        {
            int? reeksNummer = null;
            if (!string.IsNullOrWhiteSpace(ReeksNummer))
                reeksNummer = int.Parse(ReeksNummer);
            return reeksNummer;
        }

        #region info
        /* De Reeksen property bevat op index 0 steeds een Reeks-object met de naam '<Geen Reeks>'.
         * Als dat reeks object werd geselecteerd, moet dit omgezet worden naar null vooraleer het kan doorgegeven worden aan de domeinlaag.
         * Deze omzetting gebeurt in onderstaande methode.
         */
        #endregion
        private Reeks ParseReeks()
        {
            Reeks geselecteerdeReeks = GeselecteerdeReeks;
            if (GeselecteerdeReeks == Reeksen.First())
                geselecteerdeReeks = null;
            return geselecteerdeReeks;
        }

        #region
        /* Onderstaande methode wordt uitgevoerd wanneer er op de '>'-knop geduwd wordt uit het VoegStripToeView-venster. 
         * De methode verwijderd de geselecteerde auteur uit het Auteurs-vak aan de linkerkant van het scherm,
         * en voegt de geselecteerde auteur toe aan het Auteurs-vak aan de rechterkant van het scherm.
         */
        #endregion
        public RelayCommand<Auteur> VoegAuteurToeAanGeselecteerdeAuteursCommand { get; set; }
        public void VoegAuteurToeAanGeselecteerdeAuteurs(Auteur auteur)
        {
            if (auteur != null && !GeselecteerdeAuteurs.Contains(auteur))
            {
                GeselecteerdeAuteurs.Add(auteur);
                Auteurs.Remove(auteur);
                GeselecteerdeAuteurs = new ObservableCollection<Auteur>(GeselecteerdeAuteurs.OrderBy(auteur => auteur.Naam));
            }
        }

        #region
        /* Onderstaande methode wordt uitgevoerd wanneer er op de '<'-knop geduwd wordt uit het VoegStripToeView-venster. 
         * De methode verwijderd de geselecteerde auteur uit het Auteurs-vak aan de rechterkant van het scherm,
         * en voegt de geselecteerde auteur toe aan het Auteurs-vak aan de linkerkant van het scherm.
         */
        #endregion
        public RelayCommand<Auteur> VerwijderAuteurUitGeselecteerdeAuteursCommand { get; set; }
        private void VerwijderAuteurUitGeselecteerdeAuteurs(Auteur auteur)
        {
            if (GeselecteerdeAuteurs.Contains(auteur))
            {
                GeselecteerdeAuteurs.Remove(auteur);
                Auteurs.Add(auteur);
                Auteurs = new ObservableCollection<Auteur>(Auteurs.OrderBy(auteur => auteur.Naam));
            }
        }

        public Strip TeUpdatenStrip { get; set; }
        public void OntvangTeUpdatenStrip(Strip strip)
        {
            if (strip != null)
            {
                if (strip.Reeks == null)
                    GeselecteerdeReeks = Reeksen[0];
                else
                    GeselecteerdeReeks = strip.Reeks;
                ButtonContentText = "Update Strip";
                TeUpdatenStrip = strip;
                StripTitel = strip.Titel;
                GeselecteerdeUitgeverij = strip.Uitgeverij;
                ReeksNummer = strip.ReeksNummer.ToString();
                GeselecteerdeAuteurs = new ObservableCollection<Auteur>(strip.GeefAuteurs());
                Auteurs = new ObservableCollection<Auteur>(Auteurs.Where(p => !GeselecteerdeAuteurs.Any(p2 => p2.Id == p.Id)));
            }
            else
            {
                ButtonContentText = "Voeg Strip Toe";
            }
        }
    }
}
