using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfBarcode;

namespace BarcodeApp.Example.Wpf.Converters
{
    public class SymbologyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Symbology)value)
            {
                case Symbology.Interleaved2Of5:
                    return "Interleaved 2 of 5";
                case Symbology.Ean8:
                    return "Ean 8";
                case Symbology.Ean13:
                    return "Ean 13";
                case Symbology.Code39:
                    return "Code 39";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
