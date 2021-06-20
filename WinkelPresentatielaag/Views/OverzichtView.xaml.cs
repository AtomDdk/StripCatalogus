using Domeinlaag;
using Domeinlaag.Interfaces;
using Domeinlaag.Model;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WinkelPresentatielaag.Interfaces;
using WinkelPresentatielaag.ViewModels;

namespace WinkelPresentatielaag.Views
{
    /// <summary>
    /// Interaction logic for OverViewView.xaml
    /// </summary>
    public partial class OverzichtView : Window, IClosable
    {
        public OverzichtView(MainViewModel mainViewModel, IPopupBox popupBox, IVerkoopManager verkoopManager, ICatalogusManager catalogusManager, bool isLevering)
        {
            InitializeComponent();
            this.DataContext = new OverzichtViewModel(popupBox, verkoopManager, catalogusManager, isLevering, mainViewModel);
        }
    }
}


