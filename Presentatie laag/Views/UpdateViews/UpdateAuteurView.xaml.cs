using Domeinlaag;
using Domeinlaag.Interfaces;
using Presentatie_laag.Interfaces;
using Presentatie_laag.ViewModels;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentatie_laag.Views
{
    /// <summary>
    /// Interaction logic for UpdateAuteurView.xaml
    /// </summary>
    public partial class UpdateAuteurView : Window
    {
        public UpdateAuteurView(ICatalogusManager manager, IPopupBox popupBox)
        {
            InitializeComponent();
            this.DataContext = new UpdateAuteurViewModel(manager, popupBox);
        }
    }
}
