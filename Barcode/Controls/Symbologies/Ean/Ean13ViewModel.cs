namespace WpfBarcode.Controls.Symbologies.Ean
{
    internal class Ean13ViewModel : EanViewModel
    {
        public Ean13ViewModel(string code, bool displayCode)
        {
            Code = code;
            DisplayCode = displayCode;
        }
        public Ean13ViewModel()
        {
            if (!IsInDesign) return;
            Code = "042100005264";
            DisplayCode = true;
        }
        protected override EanSymbology GetEanSymbology()
        {
            return EanSymbology.Ean13;
        }

        protected override bool IsCodeLengthValid(int codeLength)
        {
            return codeLength == 12 || codeLength == 13;
        }
    }
}