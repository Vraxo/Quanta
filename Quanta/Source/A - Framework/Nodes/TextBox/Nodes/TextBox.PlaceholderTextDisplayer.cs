namespace Quanta;

public partial class TextBox
{
    private class PlaceholderTextDisplayer : BaseText
    {
        protected override string GetText()
        {
            return parent.PlaceholderText;
        }

        protected override bool ShouldSkipDrawing()
        {
            return parent.Text.Length > 0;
        }
    }
}