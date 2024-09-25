using Raylib_cs;

namespace Quanta;

public partial class TextBox : ClickableRectangle
{
    public static readonly Vector2 DefaultSize = new(300, 25);

    public string Text { get; set; } = "";
    public string DefaultText { get; set; } = "";
    public string PlaceholderText { get; set; } = "";
    public Vector2 TextOrigin { get; set; } = new(8, 0);
    public int MaxCharacters { get; set; } = int.MaxValue;
    public int MinCharacters { get; set; } = 0;
    public List<char> AllowedCharacters { get; set; } = [];
    public ButtonStyle Style { get; set; } = new();
    public bool Selected { get; set; } = false;
    public bool Editable { get; set; } = true;
    public bool RevertToDefaultText { get; set; } = true;
    public bool TemporaryDefaultText { get; set; } = true;
    public bool Secret { get; set; } = false;
    public char SecretCharacter { get; set; } = '*';
    public Action<TextBox> OnUpdate = (textBox) => { };

    public event EventHandler? FirstCharacterEntered;
    public event EventHandler? Cleared;
    public event EventHandler<string>? TextChanged;
    public event EventHandler<string>? Confirmed;

    private const int minAscii = 32;
    private const int maxAscii = 125;
    private const float backspaceDelay = 0.5f; 
    private const float backspaceSpeed = 0.05f;

    private TextBoxCaret caret;
    private float backspaceTimer = 0f;
    private bool backspaceHeld = false;

    public TextBox()
    {
        Size = DefaultSize;
    }

    public override void Start()
    {
        Style.Pressed.FillColor = ThemeLoader.Instance.Colors["TextBoxPressedFill"];
        Style.Pressed.OutlineThickness = 1;
        Style.Pressed.OutlineColor = ThemeLoader.Instance.Colors["Accent"];

        caret = GetNode<TextBoxCaret>("Caret");

        base.Start();
    }

    public override void Update()
    {
        OnUpdate(this);
        HandleInput();
        PasteText();
        base.Update();
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
            if (AllowedCharacters.Count > 0)
            {
                if (!AllowedCharacters.Contains((char)key))
                {
                    return;
                }
            }

            if (TemporaryDefaultText && Text == DefaultText)
            {
                Text = "";
            }

            if (caret.X < 0 || caret.X > Text.Length)
            {
                caret.X = Text.Length;
            }

            Text = Text.Insert(caret.X, ((char)key).ToString());
            caret.X++;
            TextChanged?.Invoke(this, Text);

            if (Text.Length == 1)
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

    private void DeleteLastCharacter()
    {
        int textLengthBeforeDeletion = Text.Length;

        if (Text.Length > 0 && caret.X > 0)
        {
            Text = Text.Remove(caret.X - 1, 1);
            caret.X--;
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
        if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyPressed(KeyboardKey.V))
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
}