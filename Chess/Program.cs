using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Engine.Board;

public static class Program
{
    private static void Main()
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            Size = new Vector2i(1024, 768),
            Title = "Chess",
            // This is needed to run on macos
            Flags = ContextFlags.ForwardCompatible,
        };

        using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
        {
            window.Run();
        }
    }
}

