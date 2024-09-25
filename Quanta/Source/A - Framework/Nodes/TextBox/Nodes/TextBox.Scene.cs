namespace Quanta;

public partial class TextBox : ClickableRectangle
{
    public override void Build()
    {
        AddChild(new TextBoxShape());

        AddChild(new TextBoxText());

        AddChild(new TextBoxPlaceholderText());

        AddChild(new TextBoxCaret(), "Caret");
    }
}