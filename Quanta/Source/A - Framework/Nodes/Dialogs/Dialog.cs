namespace Quanta;

public partial class Dialog : Node2D
{
    public override void Start()
    {
        Origin = GetNode<ColoredRectangle>("Background").Size / 2;
        GetNode<ClickManager>("/root/ClickManager").MinLayer = ClickableLayer.DialogButtons;
        GetNode<Button>("CloseButton").LeftClicked += OnCloseButtonLeftClicked;
    }

    public override void Update()
    {
        UpdatePosition();
    }

    protected void Close()
    {
        GetNode<ClickManager>("/root/ClickManager").MinLayer = 0;
        Destroy();
    }

    private void OnCloseButtonLeftClicked(object? sender, EventArgs e)
    {
        Close();
    }

    private void UpdatePosition()
    {
        float x = Window.Width / 2;
        float y = Window.Height / 2;

        Position = new(x, y);
    }
}