using System;
using System.Collections.Generic;
using System.Text;

namespace Presentatie_laag.Interfaces
{
    public interface IPopupBox
    {
        public void ShowErrorMessage(string message);
        public void ShowSuccesMessage(string message);
    }
}
