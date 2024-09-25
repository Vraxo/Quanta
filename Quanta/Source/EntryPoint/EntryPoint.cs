namespace Quanta;

public class EntryPoint
{
    [STAThread]
    public static void Main(string[] args)
    {
        WindowData windowData = new()
        {
            Title = "Quanta",
            Resolution = new(360, 480),
            //ClearColor = new(57, 57, 57, 255)
            //ClearColor = new(16, 16, 16, 255)
            ClearColor = ThemeLoader.Instance.Colors["Background"]
        };

        MainScene rootNode = new()
        {
        };

        Program program = new(windowData, rootNode, args);
        program.Run();
    }
}