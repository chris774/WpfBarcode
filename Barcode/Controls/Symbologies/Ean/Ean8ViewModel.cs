namespace WpfBarcode.Controls.Symbologies.Ean
{
    internal class Ean8ViewModel : EanViewModel
    {
        public Ean8ViewModel(string code, bool displayCode)
        {
            Code = code;
            DisplayCode = displayCode;
        }

        public Ean8ViewModel()
        {
            if (!IsInDesign) return;
            Code = "04210009";
            DisplayCode = true;
        }

        protected override bool IsCodeLengthValid(int codeLength)
        {
            return codeLength == 7 || codeLength == 8;
        }

        protected override EanSymbology GetEanSymbology()
        {
            return EanSymbology.Ean8;
        }
    }
}