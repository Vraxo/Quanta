using Raylib_cs;

namespace Quanta;

public partial class TextBox
{
    private class Caret : Node2D
    {
        public float MaxTime = 0.5F;

        private const int minTime = 0;
        private const byte minAlpha = 0;
        private const byte maxAlpha = 255;
        private float timer = 0;
        private byte alpha = 255;
        private TextBox parent;

        private int _x = 0;
        public int X
        {
            get => _x;

            set
            {
                if (value > _x)
                {
                    if (_x < parent.Text.Length)
                    {
                        _x = value;
                    }
                }
                else
                {
                    if (value >= 0)
                    {
                        _x = value;
                    }
                }

                alpha = maxAlpha;
            }
        }

        public override void Start()
        {
            parent = GetParent<TextBox>();
        }

        public override void Update()
        {
            if (!parent.Selected)
            {
                return;
            }

            HandleInput();
            Draw();
            UpdateAlpha();
            base.Update();
        }

        private void Draw()
        {
            Raylib.DrawTextEx(
                parent.Style.Current.Font,
                "|",
                GetPosition(),
                parent.Style.Current.FontSize,
                1,
                GetColor());
        }

        private void HandleInput()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                MoveIntoPosition(Raylib.GetMousePosition().X);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Right))
            {
                X++;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Left))
            {
                X--;
            }
        }

        public void MoveIntoPosition(float mouseX)
        {
            if (parent.Text.Length == 0)
            {
                X = 0;
            }
            else
            {
                //float x = mouseX - mainScene.GlobalPosition.X + mainScene.Origin.X;

                float x = mouseX - parent.GlobalPosition.X + parent.Origin.X - parent.TextOrigin.X / 2;

                int characterWidth = GetCharacterWidth();

                X = (int)MathF.Floor(x / characterWidth);

                X = X > parent.Text.Length ?
                    parent.Text.Length :
                    X;
            }
        }

        private Vector2 GetPosition()
        {
            int width = GetWidth();
            int height = GetHeight();

            int x = (int)(GlobalPosition.X - parent.Origin.X + parent.TextOrigin.X + X * width - width / 2) + X;
            int y = (int)(GlobalPosition.Y + parent.Size.Y / 2 - height / 2 - parent.Origin.Y);

            return new(x, y);
        }

        private int GetWidth()
        {
            Font font = parent.Style.Current.Font;
            float fontSize = parent.Style.Current.FontSize;

            int width = (int)Raylib.MeasureTextEx(font, "|", fontSize, 1).X;

            return width;
        }

        private int GetHeight()
        {
            Font font = parent.Style.Current.Font;
            float fontSize = parent.Style.Current.FontSize;

            int fontHeight = (int)(Raylib.MeasureTextEx(font, parent.Text, fontSize, 1).Y);

            return fontHeight;
        }

        private int GetCharacterWidth()
        {
            float textWidth = Raylib.MeasureTextEx(
                                  parent.Style.Current.Font,
                                  parent.Text,
                                  parent.Style.Current.FontSize,
                                  1).X;

            int characterWidth = (int)MathF.Ceiling(textWidth) / parent.Text.Length;

            return characterWidth;
        }

        private Color GetColor()
        {
            Color color = parent.Style.Current.TextColor;
            color.A = alpha;

            return color;
        }

        private void UpdateAlpha()
        {
            if (timer > MaxTime)
            {
                alpha = alpha == maxAlpha ? minAlpha : maxAlpha;
                timer = minTime;
            }

            timer += Raylib.GetFrameTime();
        }
    }
}