using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBarcode;

namespace WpfBarcode.Controls.Bar
{
    internal enum BarType
    {
        Solid = 1,
        Clear = 0
    }
    internal class BarViewModel : ViewModel
    {
        private double _height;
        private double _width;
        private BarType _type;

        public BarType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        public BarViewModel(BarType type, double width, double height)
        {
            Type = type;
            Width = width;
            Height = height;
        }

        public static BarViewModel[] CreateRange(BarType[] types, double[] widths, int height)
        {
            if (types.Length != widths.Length)
                throw new Exception("Lengths do not match");
            return Enumerable.Range(0, widths.Length)
                .Select(x => new BarViewModel(types[x], widths[x], height))
                .ToArray();
        }

        public static BarViewModel[] CreateRange(int[] types, double[] widths, int height)
        {
            var barTypes = types.Select(x => x == 1 ? BarType.Solid : BarType.Clear).ToArray();
            return CreateRange(barTypes, widths, height);
        }
    }
}
