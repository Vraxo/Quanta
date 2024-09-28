namespace Quanta;

public partial class LineEdit
{
    private class TextDisplayer : BaseText
    {
        protected override string GetText()
        {
            return parent.Secret ?
                new string(parent.SecretCharacter, parent.textEndIndex - parent.textStartIndex) :
                parent.Text.Substring(parent.textStartIndex, parent.textEndIndex - parent.textStartIndex);
        }

        protected override bool ShouldSkipDrawing()
        {
            return false;
        }
    }
}