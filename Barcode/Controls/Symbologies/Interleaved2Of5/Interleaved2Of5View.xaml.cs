using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfBarcode.Controls.Symbologies.Interleaved2Of5
{
    /// <summary>
    /// Interaction logic for Interleaved2Of5View.xaml
    /// </summary>
    public partial class Interleaved2Of5View : UserControl
    {
        private bool IsInDesign { get; } = DesignerProperties.GetIsInDesignMode(new DependencyObject());
        public Interleaved2Of5View()
        {
            if (IsInDesign) DataContext = new Interleaved2Of5ViewModel("1234567", true);
            InitializeComponent();
        }
    }
}
