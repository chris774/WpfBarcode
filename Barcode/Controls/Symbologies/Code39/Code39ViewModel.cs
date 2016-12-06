using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfBarcode.Controls.Bar;

namespace WpfBarcode.Controls.Symbologies.Code39
{
    internal struct Code39Encoding
    {
        public const int EmptyValue= -1;

        public int Value;

        public string Encoding;

        public Code39Encoding(int value, string encoding)
        {
            Value = value;
            Encoding = encoding;
        }
    }

    internal class Code39ViewModel : ViewModel, IBarcodeViewModel
    {
        private static int DefaultBarHeight = 50;

        #region Encoding
        private static readonly Dictionary<char, Code39Encoding> Encoding = new Dictionary<char, Code39Encoding>
        {
            {'0', new Code39Encoding(0,  "101001101101")},   // Character 0	
            {'1', new Code39Encoding(1,  "110100101011")},   // Character 1	
            {'2', new Code39Encoding(2,  "101100101011")},   // Character 2	
            {'3', new Code39Encoding(3,  "110110010101")},   // Character 3	
            {'4', new Code39Encoding(4,  "101001101011")},   // Character 4	
            {'5', new Code39Encoding(5,  "110100110101")},   // Character 5	
            {'6', new Code39Encoding(6,  "101100110101")},   // Character 6	
            {'7', new Code39Encoding(7,  "101001011011")},   // Character 7	
            {'8', new Code39Encoding(8,  "110100101101")},   // Character 8	
            {'9', new Code39Encoding(9,  "101100101101")},   // Character 9	
            {'A', new Code39Encoding(10, "110101001011")},   // Character A	
            {'B', new Code39Encoding(11, "101101001011")},   // Character B	
            {'C', new Code39Encoding(12, "110110100101")},   // Character C	
            {'D', new Code39Encoding(13, "101011001011")},   // Character D	
            {'E', new Code39Encoding(14, "110101100101")},   // Character E	
            {'F', new Code39Encoding(15, "101101100101")},   // Character F	
            {'G', new Code39Encoding(16, "101010011011")},   // Character G	
            {'H', new Code39Encoding(17, "110101001101")},   // Character H	
            {'I', new Code39Encoding(18, "101101001101")},   // Character I	
            {'J', new Code39Encoding(19, "101011001101")},   // Character J	
            {'K', new Code39Encoding(20, "110101010011")},   // Character K	
            {'L', new Code39Encoding(21, "101101010011")},   // Character L	
            {'M', new Code39Encoding(22, "110110101001")},   // Character M
            {'N', new Code39Encoding(23, "101011010011")},   // Character N
            {'O', new Code39Encoding(24, "110101101001")},   // Character O
            {'P', new Code39Encoding(25, "101101101001")},   // Character P
            {'Q', new Code39Encoding(26, "101010110011")},   // Character Q
            {'R', new Code39Encoding(27, "110101011001")},   // Character R
            {'S', new Code39Encoding(28, "101101011001")},   // Character S
            {'T', new Code39Encoding(29, "101011011001")},   // Character T
            {'U', new Code39Encoding(30, "110010101011")},   // Character U
            {'V', new Code39Encoding(31, "100110101011")},   // Character V
            {'W', new Code39Encoding(32, "110011010101")},   // Character W
            {'X', new Code39Encoding(33, "100101101011")},   // Character X
            {'Y', new Code39Encoding(34, "110010110101")},   // Character Y
            {'Z', new Code39Encoding(35, "100110110101")},   // Character Z
            {'-', new Code39Encoding(36, "100101011011")},   // Character -
            {'.', new Code39Encoding(37, "110010101101")},   // Character .
            {' ', new Code39Encoding(38, "100110101101")},   // Character SPACE
            {'$', new Code39Encoding(39, "100100100101")},   // Character $
            {'/', new Code39Encoding(40, "100100101001")},   // Character /
            {'+', new Code39Encoding(41, "100101001001")},   // Character +
            {'%', new Code39Encoding(42, "101001001001")},   // Character %
            {'*', new Code39Encoding('*',"100101101101")}    // Character *
        };

        #endregion

        private string _code;
        private bool _displayCode;
        private readonly Regex _regex = new Regex(@"^\*|\*$", RegexOptions.Compiled);
        private string _codeText;

        public ObservableCollection<BarViewModel> Bars { get; } = new ObservableCollection<BarViewModel>();

        public Code39ViewModel(string code, bool displayCode)
        {
            Code = code;
            DisplayCode = displayCode;
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (!(value ?? "").ToUpper().All(x => Encoding.ContainsKey(x)))
                {
                    SetProperty(ref _code, string.Empty);
                    CodeText = string.Empty;
                    Bars.Clear();
                }
                if (SetProperty(ref _code, value?.ToUpper()))
                {
                    GenerateBars(_code);
                    CodeText = _regex.Replace(_code, "");
                }
            }
        }

        private void GenerateBars(string code)
        {
            if (!code.StartsWith("*")) code = '*' + code;
            if (!code.EndsWith("*")) code += '*';

            Bars.Clear();
            foreach (var item in code.Select(x => Encoding[x]))
            {
                foreach (char e in item.Encoding)
                {
                    Bars.Add(new BarViewModel(e == '1' ? BarType.Solid : BarType.Clear, 1, DefaultBarHeight));
                }
                Bars.Add(new BarViewModel(BarType.Clear, 1, DefaultBarHeight));
            }
        }

        public string CodeText
        {
            get { return _codeText; }
            private set { SetProperty(ref _codeText, value); }
        }

        public bool DisplayCode
        {
            get { return _displayCode; }
            set { SetProperty(ref _displayCode, value); }
        }

        private string Checksum(string code)
        {
            var sum = code.Select(x => Encoding[x].Value).Sum();
            return string.Empty;
        }
    }
}
