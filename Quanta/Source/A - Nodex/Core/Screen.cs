﻿using Raylib_cs;

namespace Quanta;

public static class Screen
{
    public static Vector2 OriginalSize = Vector2.Zero;
    public static Vector2 Size => new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
}