using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfBarcode.Controls.Symbologies;
using WpfBarcode;
using WpfBarcode.Controls.Symbologies.Code39;
using WpfBarcode.Controls.Symbologies.Ean;
using WpfBarcode.Controls.Symbologies.Interleaved2Of5;

namespace WpfBarcode
{
    /// <summary>
    /// Interaction logic for Barcode.xaml
    /// </summary>
    public partial class Barcode : UserControl, INotifyPropertyChanged
    {
        private object _barcodeData;

        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(Barcode), new PropertyMetadata(string.Empty, CodeChanged));
        
        public static readonly DependencyProperty SymbologyProperty = DependencyProperty.Register("Symbology", typeof(Symbology), typeof(Barcode), new PropertyMetadata(global::WpfBarcode.Symbology.Interleaved2Of5, SymbologyChanged));

        public static readonly DependencyProperty DisplayCodeProperty = DependencyProperty.Register("DisplayCode", typeof(bool), typeof(Barcode), new PropertyMetadata(true, DisplayCodeChanged));

        public bool DisplayCode
        {
            get { return (bool)GetValue(DisplayCodeProperty); }
            set { SetValue(DisplayCodeProperty, value); }
        }

        public Barcode()
        {
            InitializeComponent();
            SetSymbology(Symbology.Interleaved2Of5);
        }
        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        public Symbology Symbology
        {
            get { return (Symbology)GetValue(SymbologyProperty); }
            set { SetValue(SymbologyProperty, value); }
        }

        private static void CodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var barcode = d as Barcode;
            if (barcode == null) return;
            ((IBarcodeViewModel)barcode._barcodeData).Code = (string)e.NewValue;
        }

        private static void SymbologyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Barcode)?.SetSymbology((global::WpfBarcode.Symbology)e.NewValue);
        }

        private void SetSymbology(Symbology symbology)
        {
            switch (symbology)
            {
                case Symbology.Interleaved2Of5:
                    _barcodeData = new Interleaved2Of5ViewModel(Code, DisplayCode);
                    BarcodePresenter.Content = new Interleaved2Of5View() { DataContext = _barcodeData };
                    break;
                case Symbology.Ean8:
                    _barcodeData = new Ean8ViewModel(Code, DisplayCode);
                    BarcodePresenter.Content = new EanView() { DataContext = _barcodeData };
                    break;
                case Symbology.Ean13:
                    _barcodeData = new Ean13ViewModel(Code, DisplayCode);
                    BarcodePresenter.Content = new EanView() { DataContext = _barcodeData };
                    break;
                case Symbology.Code39:
                    _barcodeData = new Code39ViewModel(Code, DisplayCode);
                    BarcodePresenter.Content = new Code39View() { DataContext = _barcodeData };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void DisplayCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var barcode = d as Barcode;
            if (barcode == null) return;
            ((IBarcodeViewModel)barcode._barcodeData).DisplayCode = (bool)e.NewValue;
        }

        public static void Save(Stream stream, string code, Symbology symbology, int width, int height, bool displayCode, int dpiX, int dpiY, BitmapEncoder encoder)
        {
            var barcode = new Barcode()
            {
                Code = code,
                Symbology = symbology,
                Width = width,
                Height = height,
                DisplayCode = displayCode,
                Background = Brushes.White
            };
            barcode.Measure(new Size(width, height));
            barcode.Arrange(new Rect(new Size(width, height)));
            barcode.UpdateLayout();
            
            var bounds = VisualTreeHelper.GetDescendantBounds(barcode);
            var rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                             (int)(bounds.Height * dpiY / 96.0),
                                             dpiX,
                                             dpiY,
                                             PixelFormats.Pbgra32);

            var dv = new DrawingVisual();
            using (var ctx = dv.RenderOpen())
            {
                var vb = new VisualBrush(barcode);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);
        }

        public static void Save(string filename, string code, Symbology symbology, int width, int height, bool displayCode, int dpiX, int dpiY, BitmapEncoder encoder)
        {
            using (var stream = File.Create(filename))
            {
                Save(stream, code, symbology, width, height, displayCode, dpiX, dpiY, encoder);
            }
        }

        public static void Save(Stream stream, string code, Symbology symbology, int width, int height)
        {
            Save(stream, code, symbology, width, height, true, 150, 150, new PngBitmapEncoder());
        }

        public static void Save(string filename, string code, Symbology symbology, int width, int height)
        {
            Save(filename, code, symbology, width, height, true, 150, 150, new PngBitmapEncoder());
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(source, value)) return false;
            source = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion
    }
}
