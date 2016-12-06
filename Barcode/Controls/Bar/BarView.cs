using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfBarcode;

namespace WpfBarcode.Controls.Bar
{
    internal class BarView : FrameworkElement
    {
        private Rect _rectangle;

        public BarView()
        {
            SnapsToDevicePixels = true;
            SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _rectangle = new System.Windows.Rect(0,0, finalSize.Width, finalSize.Height);
            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var vm = DataContext as BarViewModel;
            if (vm == null)
                drawingContext.DrawRectangle(Brushes.Transparent, null, _rectangle);
            else
                drawingContext.DrawRectangle(vm.Type == BarType.Clear ? Brushes.White : Brushes.Black,
                                             null,
                                             _rectangle);
        }
    }
}
