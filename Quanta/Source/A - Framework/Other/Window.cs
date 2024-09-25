using Raylib_cs;

namespace Quanta;

public static class Window
{
    public static int Width => Raylib.GetScreenWidth();
    public static int Height => Raylib.GetScreenHeight();
}