﻿using Raylib_cs;

namespace Quanta;

public class TexturedRectangle : Node2D
{
    public Texture2D Texture { get; set; } = Raylib.LoadTexture("");
    public Action<TexturedRectangle> OnUpdate = (rectangle) => { };

    public TexturedRectangle()
    {
        Size = new(32, 32);
    }

    public override void Update()
    {
        OnUpdate(this);
        Draw();
        base.Update();
    }

    public void Load(string texturePath)
    {
        Texture = Raylib.LoadTexture(texturePath);
    }

    private void Draw()
    {
        if (!(Visible && ReadyForVisibility))
        {
            return;
        }

        Rectangle source = new(0, 0, Texture.Width, Texture.Height);
        Rectangle destination = new(GlobalPosition, Size);

        Raylib.DrawTexturePro(
            Texture,
            source,
            destination,
            Origin,
            0,
            Color.White);
    }
}