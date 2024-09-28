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

        public bool caretMovement = false;

        private int _x = 0;
        public int X
        {
            get => _x;
            set
            {
                // Ensure that after a deletion, X doesn't exceed the current visible part of the text
                int maxCaretPosition = Math.Max(parent.Text.Length - parent.textStartIndex, 0);

                // Ensure X is within bounds (not less than 0, and not greater than the visible text length)
                if (value > _x) // Moving to the right
                {
                    // Allow movement to the right as long as it doesn't exceed the visible text length
                    if (_x < maxCaretPosition)
                    {
                        _x = value;
                    }
                }
                else // Moving to the left
                {
                    // Allow movement to the left, ensuring it doesn't go below 0
                    if (value >= 0)
                    {
                        _x = value;
                    }
                }

                // Reset alpha to make the caret visible again
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
            // Handle left mouse click to position the caret
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                MoveIntoPosition(Raylib.GetMousePosition().X);
            }

            // Handle caret movement with arrow keys
            if (Raylib.IsKeyPressed(KeyboardKey.Right))
            {
                // If caret is at the end of visible text, scroll right
                if (X >= parent.Text.Length - parent.textStartIndex)
                {
                    if (parent.textStartIndex < parent.Text.Length)
                    {
                        parent.textStartIndex++;
                    }
                }
                else
                {
                    X++;
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Left))
            {
                // If caret is at the beginning of visible text, scroll left
                if (X <= 0 && parent.textStartIndex > 0)
                {
                    parent.textStartIndex--;
                }
                else if (X > 0)
                {
                    X--;
                }
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

                X = X > parent.Text.Length - parent.textStartIndex ?
                    parent.Text.Length - parent.textStartIndex:
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