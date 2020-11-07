using Microsoft.Xna.Framework;

using SadConsole;
using Console = SadConsole.Console;
using Game = SadConsole.Game;

namespace AsLegacy
{
    class Display
    {
        public const int Width = 80;
        public const int Height = 25;

        private static Console screen;
        private static Console map;

        public static void Init()
        {
            Game.Create(Width, Height);
            Game.OnInitialize = () =>
            {
                map = new Console(40, 15);
                map.Position = new Point(0, 0);

                screen = Global.CurrentScreen;
                screen.Children.Add(map);

                Draw();
            };

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        public static void Draw()
        {
            screen.Fill(ColorAnsi.White, ColorAnsi.White, 0);

            map.Fill(ColorAnsi.BlueBright, ColorAnsi.Blue, 0);
            map.SetGlyph(1, 1, 1);

        }
    }
}
