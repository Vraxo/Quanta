namespace Quanta;

public class NumberButton : Button
{
    public int Column = 0;
    public int Row = 0;

    private MainScene mainScene;

    public NumberButton()
    {
        Size = new(80, 64);
        Style.Roundness = 0.5f;
        Style.FontSize = 32;
    }

    public override void Start()
    {
        mainScene = GetParent<MainScene>();

        LeftClicked += OnLeftClicked;

        base.Start();
    }

    public override void Update()
    {
        UpdatePosition();
        UpdateSize();

        base.Update();
    }

    private void UpdatePosition()
    {
        float windowWidthPortion = Window.Width / 4;

        float padding = (Column - 1) * (windowWidthPortion - Size.X);

        float x = windowWidthPortion * Column - Size.X / 2 - padding / 4;
        //float y = Position.Y;
        float y = Window.Height / 6 * (Row + 1);
        Position = new(x, y);
    }

    private void UpdateSize()
    {
        float width = Window.Width / 4.8f;
        float height = Window.Height / 8;
        Size = new(width, height);
    }

    private void OnLeftClicked(object? sender, EventArgs e)
    {
        mainScene.GetNode<TextBox>("TextBox").Text += Text;
    }
}