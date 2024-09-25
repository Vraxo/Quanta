namespace Quanta;

public class NumberButton : Button
{
    public int Column = 0;
    public int Row = 0;

    private MainScene mainScene;

    public NumberButton()
    {
        Size = new(75, 60);
        Style.Roundness = 0.5f;
        Style.FontSize = 32;
    }

    public override void Start()
    {
        mainScene = GetNode<MainScene>("/root");

        LeftClicked += OnLeftClicked;

        base.Start();
    }

    private void OnLeftClicked(object? sender, EventArgs e)
    {
        mainScene.GetNode<TextBox>("TextBox").Text += Text;
    }
}