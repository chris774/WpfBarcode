using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfBarcode;
using BarcodeApp.Example.Wpf.Base;
using Microsoft.Win32;
using Barcode = WpfBarcode.Barcode;

namespace BarcodeApp.Example.Wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
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
        
        private readonly Dictionary<Symbology, string> _defaultCode = new Dictionary<Symbology, string>();

        private string _selectedCode;
        private Symbology _selectedSymbology;
        private int? _imageHeight;
        private int? _imageWidth;
        private bool _isSaveButtonEnabled;

        public MainViewModel()
        {
            _defaultCode.Add(Symbology.Interleaved2Of5, "12345670");
            _defaultCode.Add(Symbology.Ean8, "0421009");
            _defaultCode.Add(Symbology.Ean13, "7501054530107");
            _defaultCode.Add(Symbology.Code39, "Code 39");

            SelectedSymbology = Symbology.Interleaved2Of5;
            ImageHeight = 100;
            ImageWidth = 250;
        }

        public ObservableCollection<Symbology> Symbologies { get; } = new ObservableCollection<Symbology>(Enum.GetValues(typeof(Symbology)).OfType<Symbology>());
        public string SelectedCode
        {
            get { return _selectedCode; }
            set
            {
                SetProperty(ref _selectedCode, value);
                _defaultCode[SelectedSymbology] = value;
            }
        }

        public Symbology SelectedSymbology
        {
            get { return _selectedSymbology; }
            set
            {
                SetProperty(ref _selectedSymbology, value);
                SelectedCode =_defaultCode[value];
            }
        }

        public int? ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                SetProperty(ref _imageWidth, value);
                SetIsSaveButtonEnabled();
            }
        }

        public int? ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                SetProperty(ref _imageHeight, value);
                SetIsSaveButtonEnabled();
            }
        }

        private void SetIsSaveButtonEnabled()
        {
            IsSaveButtonEnabled = ImageHeight != null && ImageWidth != null;
        }

        public bool IsSaveButtonEnabled
        {
            get { return _isSaveButtonEnabled; }
            private set { SetProperty(ref _isSaveButtonEnabled, value); }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var dialog = new SaveFileDialog();
                    dialog.AddExtension = true;
                    dialog.Filter = "Png Image (.png)|*.png|Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg";
                    if (dialog.ShowDialog() == true)
                    {
                        BitmapEncoder encoder;
                        switch (Path.GetExtension(dialog.FileName).ToLower())
                        {
                            case ".bmp":
                                encoder = new BmpBitmapEncoder();
                                break;
                            case ".jpeg":
                                encoder = new JpegBitmapEncoder();
                                break;
                            default:
                                encoder = new PngBitmapEncoder();
                                break;
                        }
                        
                        Barcode.Save(filename:dialog.FileName, 
                                     code: SelectedCode, 
                                     symbology:SelectedSymbology, 
                                     width: ImageWidth.Value, 
                                     height: ImageHeight.Value,
                                     displayCode:true,
                                     dpiX: 150,
                                     dpiY: 150,
                                     encoder:encoder);
                    }
                });
            }
        }
    }
}
