using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfBarcode;

namespace BarcodeApp.Example.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WpfBarcode.Barcode.Save(@"C:\Temp\Ean13.png", "750103131130", Symbology.Ean13, 100,100, true, 300, 300, new PngBitmapEncoder());
            WpfBarcode.Barcode.Save(@"C:\Temp\Ean8.png", "1234567", Symbology.Ean13, 100, 100, true, 300, 300, new PngBitmapEncoder());
            WpfBarcode.Barcode.Save(@"C:\Temp\Interleaved2Of5.png", "1234567", Symbology.Interleaved2Of5, 100, 100);
        }
    }
}
