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
                float y = Window.Size.Y * 0.05f;
                label.Position = new(x, y);
              }
        }, "LastExpressionLabel");

        AddChild(new MathTextBox
        {
            Position = new(196, 48),
            Text = "General Kenobi!",
            //AllowedCharacters = CharacterSet.Mathematics,
            Size = new(LineEdit.DefaultSize.X, LineEdit.DefaultSize.Y * 2),
            Style = new()
            {
                Roundness = 0.5f,
                FontSize = 32
            },
            OnUpdate = (textBox) =>
            {
                textBox.Position = Window.Size * new Vector2(0.5f, 0.15f);
                textBox.Size = Window.Size * new Vector2(0.75f, 0.1f);
            },
        });

        AddChild(new GridContainer()
        {
            OnUpdate = (grid) =>
            {
                grid.Scale = Window.Size / Window.OriginalSize;
            }
        });

        var grid = GetNode<GridContainer>("GridContainer");

        grid.AddChild(new NumberButton() { Text = "(" });
        grid.AddChild(new NumberButton() { Text = ")" });
        grid.AddChild(new NumberButton() { Text = "★" });
        grid.AddChild(new NumberButton() { Text = "÷" });

        grid.AddChild(new NumberButton() { Text = "7" });
        grid.AddChild(new NumberButton() { Text = "8" });
        grid.AddChild(new NumberButton() { Text = "9" });
        grid.AddChild(new NumberButton() { Text = "x" });
        
        grid.AddChild(new NumberButton() { Text = "4" });
        grid.AddChild(new NumberButton() { Text = "5" });
        grid.AddChild(new NumberButton() { Text = "6" });
        grid.AddChild(new NumberButton() { Text = "-" });
        
        grid.AddChild(new NumberButton() { Text = "1" });
        grid.AddChild(new NumberButton() { Text = "2" });
        grid.AddChild(new NumberButton() { Text = "3" });
        grid.AddChild(new NumberButton() { Text = "+" });
        
        grid.AddChild(new NumberButton() { Text = "^" });
        grid.AddChild(new NumberButton() { Text = "0" });
        grid.AddChild(new NumberButton() { Text = "." });
        grid.AddChild(new NumberButton() 
        {  
            Text = "=", 
            IsDigit = false,
            Style = new()
            {
                FillColor = ThemeLoader.Instance.Colors["Accent"],
                Pressed = new()
                {
                    FillColor = ThemeLoader.Instance.Colors["AccentDarker"]
                },
                Hover = new()
                {
                    FillColor = ThemeLoader.Instance.Colors["AccentLighter"]
                },
                Roundness = 0.5f,
                FontSize = 32,
            }
        });
    }
}