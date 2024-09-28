using Raylib_cs;

namespace Quanta;

public partial class LineEdit : ClickableRectangle
{
    #region [ - - - Properties & Fields - - - ]

    public static readonly Vector2 DefaultSize = new(300, 25);

    public string      Text                 { get; set; } = "";
    public string      DefaultText          { get; set; } = "";
    public string      PlaceholderText      { get; set; } = "";
    public Vector2     TextOrigin           { get; set; } = new(8, 0);
    public int         MaxCharacters        { get; set; } = int.MaxValue;
    public int         MinCharacters        { get; set; } = 0;
    public List<char>  AllowedCharacters    { get; set; } = [];
    public ButtonStyle Style                { get; set; } = new();
    public bool        Selected             { get; set; } = false;
    public bool        Editable             { get; set; } = true;
    public bool        RevertToDefaultText  { get; set; } = true;
    public bool        UseDefaultText { get; set; } = true;
    public bool        Secret               { get; set; } = false;
    public char        SecretCharacter      { get; set; } = '*';

    protected int textStartIndex = 0;
    private string visibleText => Text.Substring(textStartIndex, Text.Length - textStartIndex);

    public Action<LineEdit> OnUpdate = (textBox) => { };

    public event EventHandler?         FirstCharacterEntered;
    public event EventHandler?         Cleared;
    public event EventHandler<string>? TextChanged;
    public event EventHandler<string>? Confirmed;

    private const int   minAscii       = 32;
    private const int   maxAscii       = 125;
    private const float backspaceDelay = 0.5f; 
    private const float backspaceSpeed = 0.05f;

    private Caret caret;
    private float        backspaceTimer = 0f;
    private bool         backspaceHeld  = false;

    #endregion

    public LineEdit()
    {
        Size = DefaultSize;
    }

    public override void Start()
    {
        Style.Pressed.FillColor = ThemeLoader.Instance.Colors["TextBoxPressedFill"];
        Style.Pressed.OutlineThickness = 1;
        Style.Pressed.OutlineColor = ThemeLoader.Instance.Colors["Accent"];

        caret = GetNode<Caret>("Caret");

        base.Start();
    }

    public override void Update()
    {
        OnUpdate(this);
        HandleInput();
        PasteText();
        UpdateVisibleText();
        base.Update();
    }

    public void Insert(string input)
    {
        if (!Editable)
        {
            return;
        }

        InsertTextAtCaret(input);
    }

    private void HandleInput()
    {
        if (!Editable)
        {
            return;
        }

        HandleClicks();

        if (!Selected)
        {
            return;
        }

        GetTypedCharacters();
        HandleBackspace();
        Confirm();
    }

    private void HandleClicks()
    {
        if (IsMouseOver())
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left) && OnTopLeft)
            {
                Selected = true;
                Style.Current = Style.Pressed;
            }
        }
        else
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                Selected = false;
                Style.Current = Style.Default;
            }
        }
    }

    private void GetTypedCharacters()
    {
        int key = Raylib.GetCharPressed();

        while (key > 0)
        {
            InsertCharacter(key);
            key = Raylib.GetCharPressed();
        }
    }

    private void InsertCharacter(int key)
    {
        bool isKeyInRange = key >= minAscii && key <= maxAscii;
        bool isSpaceLeft = Text.Length < MaxCharacters;

        if (isKeyInRange && isSpaceLeft)
        {
            if (AllowedCharacters.Count > 0 && !AllowedCharacters.Contains((char)key))
            {
                return;
            }

            if (UseDefaultText && Text == DefaultText)
            {
                Text = "";
            }

            int insertPosition = Math.Clamp(caret.X + textStartIndex, 0, Text.Length);

            Text = Text.Insert(insertPosition, ((char)key).ToString());
            caret.X++;

            // Adjust textStartIndex if necessary to keep the caret in view
            if (caret.X >= GetVisibleCharacterCount())
            {
                textStartIndex++;
            }

            TextChanged?.Invoke(this, Text);

            if (Text.Length == 1)
            {
                FirstCharacterEntered?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void InsertTextAtCaret(string text)
    {
        bool isSpaceLeft = Text.Length + text.Length <= MaxCharacters;

        if (isSpaceLeft)
        {
            if (UseDefaultText && Text == DefaultText)
            {
                Text = "";
            }

            if (caret.X < 0 || caret.X > Text.Length)
            {
                caret.X = Text.Length;
            }

            Text = Text.Insert(caret.X, text);
            caret.X += text.Length;
            TextChanged?.Invoke(this, Text);

            if (Text.Length == text.Length)
            {
                FirstCharacterEntered?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void HandleBackspace()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Backspace))
        {
            backspaceHeld = true;
            backspaceTimer = 0f;
            DeleteLastCharacter();
        }
        else if (Raylib.IsKeyDown(KeyboardKey.Backspace) && backspaceHeld)
        {
            backspaceTimer += Raylib.GetFrameTime();

            if (backspaceTimer >= backspaceDelay)
            {
                if (backspaceTimer % backspaceSpeed < Raylib.GetFrameTime())
                {
                    DeleteLastCharacter();
                }
            }
        }

        if (Raylib.IsKeyReleased(KeyboardKey.Backspace))
        {
            backspaceHeld = false;
        }
    }

    //private void DeleteLastCharacter()
    //{
    //    int textLengthBeforeDeletion = Text.Length;
    //
    //    if (Text.Length > 0 && caret.X > 0)
    //    {
    //        Console.WriteLine(caret.X - 1);
    //        Console.WriteLine(textStartIndex);
    //        Console.WriteLine(caret.X - 1 + textStartIndex);
    //        Console.WriteLine(Text.Length);
    //        Text = Text.Remove(caret.X - 1 + textStartIndex, 1);
    //        caret.X--;
    //    }
    //
    //    RevertTextToDefaultIfEmpty();
    //
    //    TextChanged?.Invoke(this, Text);
    //
    //    if (Text.Length == 0 && textLengthBeforeDeletion != 0)
    //    {
    //        Cleared?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    private void DeleteLastCharacter()
    {
        int textLengthBeforeDeletion = Text.Length;

        if (Text.Length > 0 && caret.X > 0)
        {
            int deleteIndex = Math.Clamp(caret.X + textStartIndex - 1, 0, Text.Length - 1);

            if (deleteIndex >= 0 && deleteIndex < Text.Length)
            {
                Text = Text.Remove(deleteIndex, 1);

                if (textStartIndex > 0)
                {
                    textStartIndex--;
                }
                else
                {
                    caret.X--;
                }
            }
        }

        RevertTextToDefaultIfEmpty();

        TextChanged?.Invoke(this, Text);

        if (Text.Length == 0 && textLengthBeforeDeletion != 0)
        {
            Cleared?.Invoke(this, EventArgs.Empty);
        }
    }


    private void PasteText()
    {
        bool pressedLeftControl = Raylib.IsKeyDown(KeyboardKey.LeftControl);
        bool pressedV = Raylib.IsKeyPressed(KeyboardKey.V);

        if (pressedLeftControl && pressedV)
        {
            char[] clipboardContent = [.. Raylib.GetClipboardText_()];

            foreach (char character in clipboardContent)
            {
                InsertCharacter(character);
            }
        }
    }

    private void Confirm()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Enter))
        {
            Selected = false;
            Style.Current = Style.Default;
            Confirmed?.Invoke(this, Text);
        }
    }

    private void RevertTextToDefaultIfEmpty()
    {
        if (Text.Length == 0)
        {
            Text = DefaultText;
        }
    }

    //

    private void UpdateVisibleText()
    {
        float textWidth = GetTextWidth();
        float visibleWidth = Size.X - TextOrigin.X; // Available space for text

        // Check if the text exceeds the available visible space
        if (textWidth > visibleWidth)
        {
            float overflowWidth = textWidth - visibleWidth; // How much text is outside the visible area
            float oneCharacterWidth = GetOneCharacterWidth();

            // Calculate how many characters are off-screen
            int overflowCharacters = (int)Math.Ceiling(overflowWidth / oneCharacterWidth);

            // Only adjust textStartIndex if the caret goes out of visible range
            if (caret.X + textStartIndex >= Text.Length) // Scrolled past the end
            {
                textStartIndex = Math.Max(0, Text.Length - overflowCharacters);
            }
            else if (caret.X + textStartIndex < 0) // Scrolled too far left
            {
                textStartIndex = 0;
            }
        }
        else
        {
            // Reset textStartIndex if the text fits the visible area
            textStartIndex = 0;
        }
    }

    private float GetTextWidth()
    {
        float textWidth = Raylib.MeasureTextEx(
            Style.Current.Font,
            Text,
            Style.Current.FontSize,
            Style.Current.TextSpacing).X;

        return textWidth;
    }

    private float GetOneCharacterWidth()
    {
        float oneCharacterWidth = Raylib.MeasureTextEx(
            Style.Current.Font,
            ".",
            Style.Current.FontSize,
            Style.Current.TextSpacing).X;

        return oneCharacterWidth;
    }

    private int GetVisibleCharacterCount()
    {
        float visibleWidth = Size.X - TextOrigin.X; // Space for text
        float oneCharacterWidth = GetOneCharacterWidth();

        return (int)Math.Floor(visibleWidth / oneCharacterWidth); // How many characters fit in the visible space
    }
}