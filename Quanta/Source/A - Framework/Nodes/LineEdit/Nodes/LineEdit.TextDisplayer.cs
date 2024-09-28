namespace Quanta;

public partial class LineEdit
{
    private class TextDisplayer : BaseText
    {
        protected override string GetText()
        {
            return parent.Secret ?
                new string(parent.SecretCharacter, parent.Text.Length) :
                parent.Text.Substring(parent.textStartIndex, parent.Text.Length - parent.textStartIndex);
        }

        protected override bool ShouldSkipDrawing()
        {
            return false;
        }
    }
}