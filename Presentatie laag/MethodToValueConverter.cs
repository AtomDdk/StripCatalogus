using Domeinlaag.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Presentatie_laag.Views
{
    public sealed class MethodToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var methodName = parameter as string;
            if (value == null || methodName == null)
                return value;
            var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
            if (methodInfo == null)
            {
                return value;
            }
            else
            {
                object x = methodInfo.Invoke(value, new object[0]);

                ReadOnlyCollection<Auteur> enumerable = x as ReadOnlyCollection<Auteur>;

                string s = string.Empty;

                for (int i = 0; i < enumerable.Count-1; i++)
                {
                    Auteur item = enumerable[i];
                    Auteur a = item as Auteur;
                    s += a.Naam + " ; ";
                }
                s += enumerable[enumerable.Count - 1].Naam;
                return s;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
    }
}
