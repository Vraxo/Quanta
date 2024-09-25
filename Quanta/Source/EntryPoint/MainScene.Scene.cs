namespace Quanta;

public partial class MainScene : Node
{
    public override void Build()
    {
        AddChild(new TextBox
        {
            Position = new(196, 48),
            AllowedCharacters = CharacterSet.Mathematics,
            Size = new(TextBox.DefaultSize.X, TextBox.DefaultSize.Y * 2),
            Style = new()
            {
                Roundness = 0.5f,
                FontSize = 32
            },
            OnUpdate = (textBox) =>
            {
                float x = Window.Width / 2;
                float y = Window.Height * 0.1f ;
                textBox.Position = new(x, y);
        
                float width = Window.Width * 0.75f;
                float height = Window.Height * 0.1f;
                textBox.Size = new(width, height);
            },
        });

        // 7 8 9 x

        AddChild(new GridContainer()
        {
            OnUpdate = (grid) =>
            {
                grid.Scale = new Vector2(Window.Width, Window.Height) / new Vector2(360, 480);
            }
        });

        var grid = GetNode<GridContainer>("GridContainer");

        grid.AddChild(new NumberButton() { Text = "7" });
        grid.AddChild(new NumberButton() { Text = "8" });
        grid.AddChild(new NumberButton() { Text = "8" });
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
        grid.AddChild(new NumberButton() { Text = "." });
        grid.AddChild(new NumberButton() { Text = "0" });
        grid.AddChild(new NumberButton() { Text = "=" });


        //AddChild(new NumberButton
        //{
        //    Position = new(0, 144),
        //    Text = "7",
        //    Column = 1,
        //    Row = 1,
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(0, 144),
        //    Text = "8",
        //    Column = 2,
        //    Row = 1,
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(20, 144),
        //    Text = "9",
        //    Column = 3,
        //    Row = 1
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(0, 144),
        //    Text = "x",
        //    Column = 4,
        //    Row = 1
        //});
        //
        //// 4 5 6 -
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(50, 216),
        //    Text = "4",
        //    Column = 1,
        //    Row = 2
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(140 - 5, 216),
        //    Text = "5",
        //    Column = 2,
        //    Row = 2
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(230 - 10, 216),
        //    Text = "6",
        //    Column = 3,
        //    Row = 2
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(320 - 15, 216),
        //    Text = "-",
        //    Column = 4,
        //    Row = 2
        //});
        //
        //
        //// 1 2 3 +
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(50, 288),
        //    Text = "1",
        //    Column = 1,
        //    Row = 3
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(140 - 5, 288),
        //    Text = "2",
        //    Column = 2,
        //    Row = 3
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(230 - 10, 288),
        //    Text = "3",
        //    Column = 3,
        //    Row = 3
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(320 - 15, 288),
        //    Text = "+",
        //    Column = 4,
        //    Row = 3
        //});
        //
        //// ? 0 . =
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(50, 360),
        //    Text = "?",
        //    Column = 1,
        //    Row = 4
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(140 - 5, 360),
        //    Text = "0",
        //    Column = 2,
        //    Row = 4
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(230 - 10, 360),
        //    Text = ".",
        //    Column = 3,
        //    Row = 4
        //});
        //
        //AddChild(new NumberButton
        //{
        //    Position = new(320 - 15, 360),
        //    Text = "=",
        //    Column = 4,
        //    Row = 4
        //});
    }
}