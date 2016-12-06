using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfBarcode;
using WpfBarcode.Controls.Bar;

namespace WpfBarcode.Controls.Symbologies.Ean
{
    internal abstract class EanViewModel : ViewModel, IBarcodeViewModel
    {
        protected enum EanSymbology
        {
            Ean8,
            Ean13
        }

        private static readonly string[] OddParity = { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };
        private static readonly string[] EvenParity = { "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111" };
        private static readonly string[] RightHand = { "1110010", "1100110","1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100" };
        private static int DefaultBarHeight = 35;
        private static int TallBarHeight = 40;

        private readonly Regex _regex = new Regex("^\\d+$", RegexOptions.Compiled);
        private string _code;
        private bool _displayCode = true;
        private string _codeText;
        private BarViewModel[] _startSentinelBars;
        private BarViewModel[] _centerGuardBars;
        private BarViewModel[] _endSentinel;
        private bool _isCodeValid;
        private int? _systemDigit;
        private int _checksumDigit;

        public ObservableCollection<BarViewModel> LeftHandBars { get; } = new ObservableCollection<BarViewModel>();
        public ObservableCollection<BarViewModel> RightHandBars { get; } = new ObservableCollection<BarViewModel>();

        public bool IsCodeValid
        {
            get { return _isCodeValid; }
            set { SetProperty(ref _isCodeValid, value); }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                if (value == null)
                {
                    if (!SetProperty(ref _code, null))
                        IsCodeValid = false;
                }
                else if (!IsNumeric(value = value.Trim()) || !IsCodeLengthValid(value.Length))
                {
                    _code = string.Empty;
                    IsCodeValid = false;
                }
                else
                {
                    IsCodeValid = true;
                    if (!SetProperty(ref _code, value)) return;
                    
                    SetDigits(ref value);
                    CodeText = value;

                    LeftHandBars.Clear();
                    for (var i = 0; i < LeftHandDigits.Count; i++)
                    {
                        var encoding = GetEanSymbology() == EanSymbology.Ean8
                            ? OddParity[LeftHandDigits[i]]                                                                         //Ean8
                            : i % 2 == 1 ? EvenParity[LeftHandDigits[i]] : OddParity[LeftHandDigits[i]];                           //Ean13 
                        foreach (var b in encoding)
                            LeftHandBars.Add(new BarViewModel(b == '1' ? BarType.Solid : BarType.Clear, 1d, DefaultBarHeight));
                    }

                    RightHandBars.Clear();
                    foreach (var i in RightHandDigits)
                    {
                        var encoding = RightHand[i];
                        foreach (var b in encoding)
                            RightHandBars.Add(new BarViewModel(b == '1' ? BarType.Solid : BarType.Clear, 1d, DefaultBarHeight));
                    }
                }
            }
        }

        protected abstract bool IsCodeLengthValid(int codeLength);

        public BarViewModel[] StartSentinelBars
        {
            get
            {
                return _startSentinelBars = _startSentinelBars ?? (_startSentinelBars = BarViewModel.CreateRange(new[] { 1, 0, 1 }, new[] { 1d, 1d, 1d }, TallBarHeight));
            }
        }

        public BarViewModel[] CenterGuardBars
        {
            get
            {
                return _centerGuardBars = _centerGuardBars ?? (_centerGuardBars = BarViewModel.CreateRange(new[] { 0, 1, 0, 1, 0 }, new[] { 1d, 1d, 1d, 1d, 1d }, TallBarHeight));
            }
        }

        public BarViewModel[] EndSentinel
        {
            get
            {
                return _endSentinel = _endSentinel ?? (_endSentinel = BarViewModel.CreateRange(new[] { 1, 0, 1 }, new[] { 1d, 1d, 1d }, TallBarHeight));
            }
        }

        public int? SystemDigit
        {
            get { return _systemDigit; }
            set { SetProperty(ref _systemDigit, value); }
        }

        public int ChecksumDigit
        {
            get { return _checksumDigit; }
            set { SetProperty(ref _checksumDigit, value); }
        }

        public ObservableCollection<int> LeftHandDigits { get; } = new ObservableCollection<int>();

        public ObservableCollection<int> RightHandDigits { get; } = new ObservableCollection<int>();

        private bool IsNumeric(string code)
        {
            return long.TryParse(code, out long dummy) || _regex.IsMatch(code);
        }
        private void SetDigits(ref string code)
        {
            var midPoint = GetEanSymbology() == EanSymbology.Ean8 ? 3 : 6;
            
            LeftHandDigits.Clear();
            RightHandDigits.Clear();

            var numbers = code.Select(x => int.Parse(x.ToString())).ToArray();

            for (var i = 0; i < numbers.Length; i++)
            {
                if (i == 0 && GetEanSymbology() == EanSymbology.Ean13)
                    SystemDigit = numbers[i];
                else if (i <= midPoint)
                    LeftHandDigits.Add(numbers[i]);
                else
                    RightHandDigits.Add(numbers[i]);
            }
            if (code.Length == 12 || code.Length == 7)
            {
                var checksum = CalculateChecksum(numbers);
                code += checksum.ToString();
                RightHandDigits.Add(checksum);
            }

            if (GetEanSymbology() == EanSymbology.Ean8) SystemDigit = null;
        }

        protected abstract EanSymbology GetEanSymbology();

        private int CalculateChecksum(int[] numbers)
        {
            var odd = true;
            var total = 0;
            for (var i = numbers.Length - 1; i >= 0; i--)
            {
                total += odd ? numbers[i] * 3 : numbers[i];
                odd = !odd;
            }
            return (total = total % 10) == 0 ? 0 : 10 - total;
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
