using Microsoft.Xna.Framework;

using SadConsole;
using SadConsole.Components;
using SadConsole.Debug;
using SadConsole.Input;
using System;
using Console = SadConsole.Console;
using Game = SadConsole.Game;

namespace AsLegacy
{
    public class Display : DrawConsoleComponent
    {
        public const int Width = 80;
        public const int Height = 25;

        private static Display display;

        public static void Init()
        {
            if (display != null)
                return;

            Game.Create(Width, Height);
            Game.OnInitialize = () =>
            {
                display = new Display();
            };

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private Console screen;
        private Console map;

        private Display()
        {
            map = new Console(40, 15);
            map.Position = new Point(0, 0);

            screen = new Console(Width, Height);
            screen.Children.Add(map);
            screen.Components.Add(this);

            Global.CurrentScreen = screen;
        }

        public override void Draw(Console console, TimeSpan delta)
        {
            screen.Fill(ColorAnsi.White, ColorAnsi.White, 0);

            map.Fill(ColorAnsi.BlueBright, ColorAnsi.Blue, 0);
            map.SetGlyph(1, 1, 1);
        }
    }
}
