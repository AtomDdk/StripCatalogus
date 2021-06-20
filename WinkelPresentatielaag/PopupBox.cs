using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WinkelPresentatielaag.Interfaces;

namespace WinkelPresentatielaag
{
    public class PopupBox : IPopupBox
    {
        public PopupBox()
        {

        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowSuccesMessage(string message)
        {
            MessageBox.Show(message, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
