namespace Quanta;

public partial class TextBox
{
    public class PlaceholderTextDisplayer : BaseText
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