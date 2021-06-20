using Domeinlaag;
using Presentatie_laag.Interfaces;
using Presentatie_laag.ViewFactories;
using Presentatie_laag.ViewModels;
using Presentatie_laag.Views;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Presentatie_laag
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IViewFactory factory = new ViewFactory(new CatalogusManager(new UnitOfWork()), new PopupBox());
            factory.MaakZoekStripView();
            base.OnStartup(e);
        }
    }
}
