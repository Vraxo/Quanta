using Raylib_cs;

namespace Quanta;

public static class Window
{
    public static Vector2 OriginalResolution = Vector2.Zero;
    public static Vector2 Resolution => new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
}