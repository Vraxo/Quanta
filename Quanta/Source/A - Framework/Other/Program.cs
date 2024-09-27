using Raylib_cs;
using System.Reflection;

namespace Quanta;

public class Program(WindowData windowData, Node rootNode, string[] args)
{
    public Node RootNode = rootNode;
    public string[] Args = args;

    private readonly WindowData windowData = windowData;

    public void Run()
    {
        Initialize();
        RunLoop();
    }

    private void Initialize()
    {
        SetCurrentDirectory();

        Window.OriginalSize = windowData.Resolution;

        int width = (int)windowData.Resolution.X;
        int height = (int)windowData.Resolution.Y;

        SetWindowFlags();

        Raylib.InitWindow(width, height, windowData.Title);
        Raylib.SetWindowMinSize(width, height);
        Raylib.InitAudioDevice();

        Raylib.SetTargetFPS(60);

        Raylib.SetWindowIcon(Raylib.LoadImage("Resources/Icon/Icon.png"));
        
        //Scene scene = new(rootNode);
        //var mainScene = scene.Instantiate<MainScene>();
        //RootNode = mainScene;
        RootNode.Program = this;

        RootNode.Build();
        RootNode.Start();
    }

    private static void SetCurrentDirectory()
    {
        string assemblyLocation = Assembly.GetEntryAssembly().Location;
        Environment.CurrentDirectory = Path.GetDirectoryName(assemblyLocation);
    }

    private static void SetWindowFlags()
    {
        Raylib.SetConfigFlags(
            ConfigFlags.VSyncHint | 
            ConfigFlags.Msaa4xHint |
            ConfigFlags.HighDpiWindow |
            ConfigFlags.ResizableWindow |
            ConfigFlags.AlwaysRunWindow);
    }

    private void RunLoop()
    {
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
                Raylib.ClearBackground(windowData.ClearColor);
                RootNode.Process();
            Raylib.EndDrawing();

            PrintTree();
        }
    }

    private void PrintTree()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            //Console.ClearItems();

            Random random = new();
            int r = random.Next(1000);

            Raylib.TakeScreenshot($"Screenshot{r}.png");

            //RootNode.PrintChildren();
        }
    }
}