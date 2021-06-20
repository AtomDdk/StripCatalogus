using Domeinlaag;
using Domeinlaag.Interfaces;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinkelPresentatielaag.Factories;
using WinkelPresentatielaag.Interfaces;
using WinkelPresentatielaag.ViewModels;

namespace WinkelPresentatielaag.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IPopupBox popupBox, ICatalogusManager catalogusManager, IVerkoopManager verkoopManager, IViewFactory viewFactory, bool isLevering)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(popupBox, catalogusManager, verkoopManager, viewFactory, isLevering);
        }
    }
}
