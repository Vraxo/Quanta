namespace Quanta;

public partial class TextBox
{
    private class TextDisplayer : BaseText
    {
        protected override string GetText()
        {
            return parent.Secret ?
                new string(parent.SecretCharacter, parent.Text.Length) :
                parent.Text;
        }

        protected override bool ShouldSkipDrawing()
        {
            return false;
        }
    }
}