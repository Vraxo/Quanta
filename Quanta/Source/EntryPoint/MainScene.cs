namespace Quanta;

public partial class MainScene : Node
{
    public override void Start()
    {
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

        base.Start();
    }
}