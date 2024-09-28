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

        private const float arrowKeyDelay = 0.5f; // Delay before rapid movement starts
        private const float arrowKeySpeed = 0.05f; // Speed of movement after delay
        private float arrowKeyTimer = 0f;
        private bool arrowKeyHeld = false;
        private bool moveRight = false; // Track which direction to move

        public bool caretMovement = false;

        private int _x = 0;
        public int X
        {
            get => _x;
            set
            {
                // Ensure that after a deletion, X doesn't exceed the current visible part of the text
                int maxVisibleCharacters = parent.Text.Length - parent.textStartIndex;

                // Clamp the value to the range of visible characters
                _x = Math.Clamp(value, 0, maxVisibleCharacters);

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
            HandleArrowKeys();
        }

        private void HandleArrowKeys()
        {
            // Check if right arrow key is pressed or held
            if (Raylib.IsKeyPressed(KeyboardKey.Right))
            {
                arrowKeyHeld = true;
                arrowKeyTimer = 0f;
                moveRight = true; // Move right initially
                MoveCaretRight();
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Right) && arrowKeyHeld && moveRight)
            {
                arrowKeyTimer += Raylib.GetFrameTime();

                if (arrowKeyTimer >= arrowKeyDelay)
                {
                    // Move rapidly after delay
                    if (arrowKeyTimer % arrowKeySpeed < Raylib.GetFrameTime())
                    {
                        MoveCaretRight();
                    }
                }
            }

            // Check if left arrow key is pressed or held
            if (Raylib.IsKeyPressed(KeyboardKey.Left))
            {
                arrowKeyHeld = true;
                arrowKeyTimer = 0f;
                moveRight = false; // Move left initially
                MoveCaretLeft();
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Left) && arrowKeyHeld && !moveRight)
            {
                arrowKeyTimer += Raylib.GetFrameTime();

                if (arrowKeyTimer >= arrowKeyDelay)
                {
                    // Move rapidly after delay
                    if (arrowKeyTimer % arrowKeySpeed < Raylib.GetFrameTime())
                    {
                        MoveCaretLeft();
                    }
                }
            }

            if (Raylib.IsKeyReleased(KeyboardKey.Right) || Raylib.IsKeyReleased(KeyboardKey.Left))
            {
                arrowKeyHeld = false; // Stop moving when key is released
            }
        }

        private void MoveCaretRight()
        {
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

        private void MoveCaretLeft()
        {
            if (X <= 0 && parent.textStartIndex > 0)
            {
                parent.textStartIndex--;
            }
            else if (X > 0)
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
                float x = mouseX - parent.GlobalPosition.X + parent.Origin.X - parent.TextOrigin.X / 2;

                int characterWidth = GetCharacterWidth();

                X = (int)MathF.Floor(x / characterWidth);

                X = X > parent.Text.Length - parent.textStartIndex ?
                    parent.Text.Length - parent.textStartIndex :
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
