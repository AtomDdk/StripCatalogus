using Domeinlaag;
using Domeinlaag.Interfaces;
using StripCatalogus___Data_Layer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WinkelPresentatielaag.Factories;
using WinkelPresentatielaag.Interfaces;
using WinkelPresentatielaag.Views;

namespace WinkelPresentatielaag
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IUnitOfWork uow = new UnitOfWork();
            IViewFactory viewFactory = new ViewFactory(new PopupBox() ,new CatalogusManager(uow), new VerkoopManager(uow));
            viewFactory.MaakMenuView();
            base.OnStartup(e);
        }
    }
}
