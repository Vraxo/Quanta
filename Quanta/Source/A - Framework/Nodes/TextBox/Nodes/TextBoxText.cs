namespace Quanta;

public class TextBoxText : TextBoxBaseText
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