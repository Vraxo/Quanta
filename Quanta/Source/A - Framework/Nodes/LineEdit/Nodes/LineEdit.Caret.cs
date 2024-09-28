using Raylib_cs;

namespace Quanta;

public partial class LineEdit
{
    private class Caret : Node2D
    {
        public float MaxTime = 0.5F;

        private const int minTime = 0;
        private const byte minAlpha = 0;
        private const byte maxAlpha = 255;
        private float timer = 0;
        private byte alpha = 255;
        private LineEdit parent;

        private int _x = 0;
        public int X
        {
            get => _x;

            set
            {
                // Constrain caret position to be between textStartIndex and the end of the visible text.
                if (value >= parent.textStartIndex && value <= parent.Text.Length)
                {
                    _x = value;
                }
                else if (value < parent.textStartIndex)
                {
                    _x = parent.textStartIndex;
                }
                else if (value > parent.Text.Length)
                {
                    _x = parent.Text.Length;
                }

                alpha = maxAlpha;
            }
        }

        public override void Start()
        {
            parent = GetParent<LineEdit>();
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
                X = parent.textStartIndex;
            }
            else
            {
                float x = mouseX - parent.GlobalPosition.X + parent.Origin.X - parent.TextOrigin.X / 2;

                int characterWidth = GetCharacterWidth();

                X = (int)MathF.Floor(x / characterWidth) + parent.textStartIndex;

                // Ensure the caret stays within bounds
                X = Math.Clamp(X, parent.textStartIndex, parent.Text.Length);
            }
        }

        private Vector2 GetPosition()
        {
            int width = GetWidth();
            int height = GetHeight();

            // Adjust the caret position based on textStartIndex
            int x = (int)(GlobalPosition.X - parent.Origin.X + parent.TextOrigin.X + (X - parent.textStartIndex) * width - width / 2) + X;
            int y = (int)(GlobalPosition.Y + parent.Size.Y / 2 - height / 2 - parent.Origin.Y);

            return new Vector2(x, y);
        }

        private int GetWidth()
        {
            Font font = parent.Style.Current.Font;
            float fontSize = parent.Style.Current.FontSize;

            return (int)Raylib.MeasureTextEx(font, "|", fontSize, 1).X;
        }

        private int GetHeight()
        {
            Font font = parent.Style.Current.Font;
            float fontSize = parent.Style.Current.FontSize;

            return (int)Raylib.MeasureTextEx(font, parent.Text, fontSize, 1).Y;
        }

        private int GetCharacterWidth()
        {
            if (parent.Text.Length == 0)
            {
                return 0;
            }

            float textWidth = Raylib.MeasureTextEx(
                                  parent.Style.Current.Font,
                                  parent.Text.Substring(parent.textStartIndex),  // Measure visible text
                                  parent.Style.Current.FontSize,
                                  1).X;

            return (int)MathF.Ceiling(textWidth / (parent.Text.Length - parent.textStartIndex));
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