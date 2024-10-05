namespace Quanta;

public partial class MainScene : Node
{
    public override void Build()
    {
        AddChild(new Label
        {
            FontSize = 24,
            OnUpdate = (label) =>
            {
                var mathTextBox = GetNode<MathTextBox>("MathTextBox");

                float x = mathTextBox.Position.X - mathTextBox.Size.X / 2;
                float y = Screen.Size.Y * 0.05f;
                label.Position = new(x, y);
              }
        }, "LastExpressionLabel");

        AddChild(new MathTextBox
        {
            Position = new(196, 48),
            AllowedCharacters = CharacterSet.Mathematics,
            Size = new(LineEdit.DefaultSize.X, LineEdit.DefaultSize.Y * 2),
            Style = new()
            {
                Roundness = 0.5f,
                FontSize = 32
            },
            OnUpdate = (textBox) =>
            {
                textBox.Position = Screen.Size * new Vector2(0.5f, 0.15f);
                textBox.Size = Screen.Size * new Vector2(0.75f, 0.1f);
            },
        });

        AddChild(new GridContainer()
        {
            OnUpdate = (grid) =>
            {
                grid.Scale = Screen.Size / Screen.OriginalSize;
            }
        });
    }
}