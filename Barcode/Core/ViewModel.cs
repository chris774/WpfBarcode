using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfBarcode
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T source, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(source, value)) return false;
            source = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected bool IsInDesign { get; } = DesignerProperties.GetIsInDesignMode(new DependencyObject());
    }
}
