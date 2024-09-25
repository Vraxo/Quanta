using Raylib_cs;

namespace Quanta;

public abstract class ClickableRectangle : Clickable
{
    public override bool IsMouseOver()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();

        bool isMouseOver = mousePosition.X > GlobalPosition.X - Origin.X &&
                           mousePosition.X < GlobalPosition.X + Size.X - Origin.X &&
                           mousePosition.Y > GlobalPosition.Y - Origin.Y &&
                           mousePosition.Y < GlobalPosition.Y + Size.Y - Origin.Y;

        return isMouseOver;
    }
}