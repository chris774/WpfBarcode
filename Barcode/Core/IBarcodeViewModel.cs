using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBarcode.Controls.Bar;

namespace WpfBarcode
{
    internal interface IBarcodeViewModel
    {
        string Code { get; set; }

        string CodeText { get; }

        bool DisplayCode { get; set; }

    }
}
