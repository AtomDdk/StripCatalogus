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
using WinkelPresentatielaag.Factories;
using WinkelPresentatielaag.Interfaces;
using WinkelPresentatielaag.ViewModels;

namespace WinkelPresentatielaag.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : Window
    {
        public MenuView(IViewFactory viewFactory)
        {
            InitializeComponent();
            this.DataContext = new MenuViewModel(viewFactory);
        }
    }
}


