using Domeinlaag.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinkelPresentatielaag.ViewModels;


namespace WinkelPresentatielaag.Interfaces
{
    public interface IViewFactory
    {
        void MaakMainView(bool isLevering);
        void MaakOverzichtView(MainViewModel mainViewModel, bool isLevering);
        void MaakMenuView();
    }
}







