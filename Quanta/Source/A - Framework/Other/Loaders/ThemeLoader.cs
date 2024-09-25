using System.Text.Json;

namespace Quanta;

public class ThemeLoader
{
    public Dictionary<string, Color> Colors = new()
{
    { "Background", new(30, 30, 30, 255) },         // Darker background
    { "DefaultFill", new(45, 45, 45, 255) },        // Darker fill for default elements
    { "DefaultOutline", new(70, 70, 70, 255) },     // Outline color for default elements
    { "HoverFill", new(55, 55, 55, 255) },          // Fill color when hovered
    { "HoverOutline", new(90, 90, 90, 255) },       // Outline color when hovered
    { "Accent", new(0, 122, 204, 255) },             // Blue accent color
    { "PressedOutline", new(80, 80, 80, 255) },     // Outline color when pressed
    { "TextBoxPressedFill", new(75, 75, 75, 255) }, // Fill color for pressed text boxes
    { "SliderEmptyFill", new(90, 90, 90, 255) },    // Empty slider fill
    { "SliderFillColor", new(0, 122, 204, 255) },    // Slider fill color matching the accent
    { "Text", new(220, 220, 220, 255) }              // Light gray text for readability
};


    private static ThemeLoader? instance;

    public static ThemeLoader Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    private ThemeLoader()
    {
        //Save();
        string name = File.ReadAllText("Resources/Themes/Theme.txt");
        //Load(name);
    }

    private void Save()
    {
        var serializableColors = Colors.ToDictionary(
            kvp => kvp.Key,
            kvp => new
            {
                kvp.Value.R,
                kvp.Value.G,
                kvp.Value.B,
                kvp.Value.A
            }
        );

        var json = JsonSerializer.Serialize(serializableColors, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText("Resources/Themes/Dark.json", json);
    }

    public void Load(string fileName)
    {
        string json = File.ReadAllText($"Resources/Themes/{fileName}.json");
        var deserializedColors = JsonSerializer.Deserialize<Dictionary<string, JsonColor>>(json);

        Colors.Clear();

        foreach (var kvp in deserializedColors)
        {
            Colors[kvp.Key] = new(kvp.Value.R, kvp.Value.G, kvp.Value.B, kvp.Value.A);
        }
    }

    private class JsonColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }
}