using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using WinkelPresentatielaag.Factories;
using System.Windows;
using WinkelPresentatielaag.Interfaces;

namespace WinkelPresentatielaag.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private protected bool _isLevering;
        private protected IViewFactory _viewFactory;
        private protected ICatalogusManager _catalogusManager;
        private protected IVerkoopManager _verkoopManager;
        private protected IPopupBox _popupBox;
        public MainViewModel(IPopupBox popupBox, ICatalogusManager catalogusManager, IVerkoopManager verkoopManager, IViewFactory viewFactory, bool isLevering)

        {
            if (popupBox == null)
                throw new NullReferenceException("PopupBox mag niet null zijn.");
            _popupBox = popupBox;
            if (catalogusManager == null)
                throw new NullReferenceException("CatalogusManager mag niet null zijn.");
            _catalogusManager = catalogusManager;
            if (verkoopManager == null)
                throw new NullReferenceException("VerkoopManager mag niet null zijn.");
            _verkoopManager = verkoopManager;
            if (viewFactory == null)
                throw new NullReferenceException("ViewFactory mag niet null zijn.");
            _viewFactory = viewFactory;
            _isLevering = isLevering;
            StelTekstenIn();
            RefreshAlleCollecties();
            VoegStripToeCommand = new RelayCommand<Strip>(VoegStripToe);
            VerwijderStripCommand = new RelayCommand<KeyValuePair<Strip, int>>(VerwijderStrip);
            MaakUitgeverijSelectieOngedaanCommand = new RelayCommand(MaakUitgeverijSelectieOngedaan);
            MaakReeksSelectieOngedaanCommand = new RelayCommand(MaakReeksSelectieOngedaan);
            MaakAuteurSelectieOngedaanCommand = new RelayCommand(MaakAuteurSelectieOngedaan);
            ZoekStripCommand = new RelayCommand(ZoekStrip);
            GaNaarOverzichtCommand = new RelayCommand(GaNaarOverzicht);
            ZoekStripArguments = new ZoekStripArguments();
        }

        // Relaycommands:
        public RelayCommand<Strip> VoegStripToeCommand { get; set; }
        public RelayCommand<KeyValuePair<Strip, int>> VerwijderStripCommand { get; set; }
        public RelayCommand ZoekStripCommand { get; set; }
        public RelayCommand MaakUitgeverijSelectieOngedaanCommand { get; set; }
        public RelayCommand MaakAuteurSelectieOngedaanCommand { get; set; }
        public RelayCommand MaakReeksSelectieOngedaanCommand { get; set; }
        // Collecties:
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

        private ObservableCollection<KeyValuePair<Strip, int>> _geselecteerdeStrips;
        public ObservableCollection<KeyValuePair<Strip, int>> GeselecteerdeStrips
        {
            get => _geselecteerdeStrips;
            set
            {
                if (_geselecteerdeStrips != value)
                {
                    _geselecteerdeStrips = value;
                    RaisePropertyChanged(() => GeselecteerdeStrips);
                }
            }
        }

        private ObservableCollection<Strip> _alleStrips;
        public ObservableCollection<Strip> AlleStrips
        {
            get => _alleStrips;
            set
            {
                if (_alleStrips != value)
                {
                    _alleStrips = value;
                    RaisePropertyChanged(() => AlleStrips);
                }
            }
        }
        // Properties die nodig zijn om via de X-knoppen een geselecteerd item ongedaan te kunnen maken:
        public string GezochtReeksNummer { get; set; }
        public int? IndexVanGezochteAuteur { get; set; }
        public int? IndexVanGezochteReeks { get; set; }
        public int? IndexVanGezochteUitgeverij { get; set; }
        public ZoekStripArguments ZoekStripArguments { get; set; }

        private void VoegStripToe(Strip strip)
        {
            if (strip != null)
            {
                bool zitStripAlInWinkelmand = GeselecteerdeStrips.Select(keyValuePair => keyValuePair.Key).Contains(strip);

                if (zitStripAlInWinkelmand)
                {
                    KeyValuePair<Strip, int> keyValuePair = GeselecteerdeStrips.FirstOrDefault(keyValuePair => keyValuePair.Key == strip);
                    int i = GeselecteerdeStrips.IndexOf(keyValuePair);
                    GeselecteerdeStrips[i] = new KeyValuePair<Strip, int>(keyValuePair.Key, keyValuePair.Value + 1);
                    GeselecteerdeStripMetAantal = GeselecteerdeStrips[i];
                }
                else
                {
                    GeselecteerdeStrips.Add(new KeyValuePair<Strip, int>(strip, 1));
                    GeselecteerdeStripMetAantal = GeselecteerdeStrips.Last();
                }
            }
        }

        private void VerwijderStrip(KeyValuePair<Strip, int> keyValuePair)
        {
            if (keyValuePair.Key != null)
            {
                if (keyValuePair.Key != null)
                {
                    int i = GeselecteerdeStrips.IndexOf(keyValuePair);
                    if (keyValuePair.Value > 1)
                    {
                        GeselecteerdeStrips[i] = new KeyValuePair<Strip, int>(keyValuePair.Key, keyValuePair.Value - 1);
                        GeselecteerdeStripMetAantal = GeselecteerdeStrips[i];
                    }
                    else
                    {
                        GeselecteerdeStrips.RemoveAt(i);
                        GeselecteerdeStripMetAantal = default;
                    }
                }
            }
        }

        private void MaakUitgeverijSelectieOngedaan()
        {
            IndexVanGezochteUitgeverij = -1;
            RaisePropertyChanged(() => IndexVanGezochteUitgeverij);
            ZoekStripArguments.FilterOpUitgeverij = false;
        }

        private void MaakReeksSelectieOngedaan()
        {
            IndexVanGezochteReeks = -1;
            RaisePropertyChanged(() => IndexVanGezochteReeks);
            ZoekStripArguments.FilterOpReeks = false;
        }

        private void MaakAuteurSelectieOngedaan()
        {
            IndexVanGezochteAuteur = -1;
            RaisePropertyChanged(() => IndexVanGezochteAuteur);
            ZoekStripArguments.FilterOpAuteur = false;
        }

        private void ZoekStrip()
        {
            if (IndexVanGezochteReeks == 0)
                ZoekStripArguments.Reeks = null;
            if (string.IsNullOrWhiteSpace(GezochtReeksNummer) || !int.TryParse(GezochtReeksNummer, out int gezochtReeksNr))
                ZoekStripArguments.ReeksNummer = null;
            else
                ZoekStripArguments.ReeksNummer = gezochtReeksNr;

            if (GezochtReeksNummer.Any(x => char.IsLetter(x)))
                AlleStrips = new ObservableCollection<Strip>();
            else
            {
                if (!_isLevering)
                    AlleStrips = new ObservableCollection<Strip>(_verkoopManager.ZoekStripsVoorVerkoop(ZoekStripArguments));
                else
                    AlleStrips = new ObservableCollection<Strip>(_catalogusManager.ZoekStrips(ZoekStripArguments));
            }
        }

        public RelayCommand GaNaarOverzichtCommand { get; set; }
        private void GaNaarOverzicht()
        {
            if (GeselecteerdeStrips.Count > 0)
            {
                _viewFactory.MaakOverzichtView(this, _isLevering);
            }
            else
            {
                if (_isLevering == true)
                    _popupBox.ShowErrorMessage("Uw leveringen zijn leeg.");
                else
                    _popupBox.ShowErrorMessage("Uw winkelmand is leeg.");
            }
        }

        private void StelTekstenIn()
        {
            if (_isLevering == false)
            {
                StripsLabelTekst = "Stripassortiment";
                GeselecteerdeStripsLabelTekst = "Winkelmand";
                ButtonTekst = "Ga Naar BestellingOverzicht";
            }
            else
            {
                StripsLabelTekst = "Inventaris";
                GeselecteerdeStripsLabelTekst = "Levering";
                ButtonTekst = "Ga Naar LeveringOverzicht";
            }
        }

        public void RefreshAlleCollecties()
        {
            // Opvullen van de comboboxes en listview:
            if (!_isLevering)
                AlleStrips = new ObservableCollection<Strip>(_verkoopManager.GeefAlleStripsVoorVerkoop());
            else
                AlleStrips = new ObservableCollection<Strip>(_catalogusManager.GeefAlleStrips());
            GeselecteerdeStrips = new ObservableCollection<KeyValuePair<Strip, int>>();
            Reeksen = new ObservableCollection<Reeks>(_catalogusManager.GeefAlleReeksen());
            Reeksen.Insert(0, new Reeks("<Geen Reeks>"));
            Auteurs = new ObservableCollection<Auteur>(_catalogusManager.GeefAlleAuteurs());
            Uitgeverijen = new ObservableCollection<Uitgeverij>(_catalogusManager.GeefAlleUitgeverijen());
            GeselecteerdeStripMetAantal = default;
        }

        public string StripsLabelTekst { get; set; }
        public string GeselecteerdeStripsLabelTekst { get; set; }
        public string ButtonTekst { get; set; }

        private KeyValuePair<Strip, int> _geselecteerdeStripMetAantal;
        public KeyValuePair<Strip, int> GeselecteerdeStripMetAantal
        {
            get => _geselecteerdeStripMetAantal;
            set
            {
                _geselecteerdeStripMetAantal = value;
                RaisePropertyChanged(() => GeselecteerdeStripMetAantal);
            }
        }
    }

}

