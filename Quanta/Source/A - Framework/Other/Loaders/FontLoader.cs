using Raylib_cs;

namespace Quanta;

public class FontLoader
{
    public Dictionary<string, Font> Fonts = [];

    private static FontLoader? instance;

    public static FontLoader Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    private FontLoader()
    {
        //LoadDefaultFont();
        Load("Resources/Fonts/RobotoMono.ttf", "RobotoMono 32", 32);
    }

    public void Load(string path, string name, int size)
    {
        Font font = Raylib.LoadFontEx(path, size, null, 0);
        Fonts.Add(name, font);

        Texture2D texture = Fonts[name].Texture;
        var filter = TextureFilter.Bilinear;
        Raylib.SetTextureFilter(texture, filter);
    }

    private void LoadDefaultFont()
    {
        string name = "RobotoMono 32";
        string path = "Resources/Fonts/RobotoMono.ttf";
        int size = 32;

        Font font = Raylib.LoadFontEx(path, size, null, 0);
        Fonts.Add(name, font);

        Texture2D texture = Fonts[name].Texture;
        var filter = TextureFilter.Bilinear;
        Raylib.SetTextureFilter(texture, filter);
    }
}