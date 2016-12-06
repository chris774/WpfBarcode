using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using WpfBarcode.Controls.Bar;

namespace WpfBarcode.Controls.Symbologies.Interleaved2Of5
{
    internal class Interleaved2Of5ViewModel : ViewModel, IBarcodeViewModel
    {
        private static readonly string[] Encoding = new[] { "NNWWN", "WNNNW", "NWNNW", "WWNNN", "NNWNW", "WNWNN", "NWWNN", "NNNWW", "WNNWN", "NWNWN" };
        private static int DefaultBarHeight = 35;
        private static readonly Regex _regex = new Regex("^\\d+$", RegexOptions.Compiled);
        private string _code;
        private bool _displayCode = true;
        private string _codeText;

        public Interleaved2Of5ViewModel()
        {
            if (IsInDesign) Code = "1234567";
        }

        public Interleaved2Of5ViewModel(string code, bool displayCode)
        {
            Code = code;
            DisplayCode = displayCode;
        }

        public ObservableCollection<BarViewModel> Bars { get; } = new ObservableCollection<BarViewModel>();

        public string Code
        {
            get { return _code; }
            set
            {
                if (value == null)
                {
                    if (!SetProperty(ref _code, null))
                        Bars.Clear();
                }
                else if (!IsNumeric(value = value.Trim()))
                {
                    _code = string.Empty;
                    Bars.Clear();
                }
                else
                {
                    if (!SetProperty(ref _code, value)) return;
                    var numbers = GetNumbers(ref value);
                    CodeText = value;

                    AddStartCode();
                    for (int i = 0; i < numbers.Count; i +=2 )
                    {
                        for (var j = 0; j < 5; j++)
                        {
                            Bars.Add(new BarViewModel(BarType.Solid, Encoding[numbers[i]][j] == 'N' ? 1 : 2.25d, DefaultBarHeight));
                            Bars.Add(new BarViewModel(BarType.Clear, Encoding[numbers[i + 1]][j] == 'N' ? 1 : 2.25d, DefaultBarHeight));
                        }
                    }

                    AddStopCode();
                }
            }
        }

        private void AddStartCode()
        {
            Bars.Clear();
            foreach (var bar in BarViewModel.CreateRange(new[] {1, 0, 1, 0}, new[] {1d, 1d, 1d, 1d}, DefaultBarHeight))
                Bars.Add(bar);
        }

        private void AddStopCode()
        {
            foreach (var bar in BarViewModel.CreateRange(new[] {1, 1, 0, 1}, new[] {1d, 1d, 1d, 1d}, DefaultBarHeight)) 
            {
                Bars.Add(bar);
            }
        }

        private bool IsNumeric(string code)
        {
            return long.TryParse(code, out long dummy) || _regex.IsMatch(code);
        }

        private List<int> GetNumbers(ref string code)
        {
            if (code.Length % 2 == 1)
            {
                var result = new List<int>();
                var total = 0;
                for (var i = 0; i < code.Length; i++)
                {
                    var number = int.Parse(code[i].ToString());
                    result.Add(number);
                    total += i % 2 == 1 ? number : number * 3;
                }
                int checksum;
                checksum = ((checksum = total % 10)) == 0 ? 0 : 10 - checksum;
                result.Add(checksum);
                code += checksum.ToString();
                return result;
            }
            
            return code.Select(x => int.Parse(x.ToString())).ToList();
        }

        public bool DisplayCode
        {
            get { return _displayCode; }
            set { SetProperty(ref _displayCode, value); }
        }

        public string CodeText
        {
            get { return _codeText; }
            private set { SetProperty(ref _codeText, value); }
        }
    }
}
