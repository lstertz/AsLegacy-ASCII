using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using System;

using Console = SadConsole.Console;
using Game = SadConsole.Game;

namespace AsLegacy
{
    public class AsLegacy : UpdateConsoleComponent
    {
        public const int Width = 80;
        public const int Height = 25;

        public static void Main(string[] args)
        {
            new AsLegacy();
        }

        private World world;

        public AsLegacy()
        {
            Game.Create(Width, Height);
            Game.OnInitialize = () =>
            {
                Console console = new Console(Width, Height);
                Global.CurrentScreen = console;
                console.Components.Add(this);

                world = new World();
                Display.Init(console, world);
            };

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        public override void Update(SadConsole.Console console, TimeSpan delta)
        {
            world.characters[2].Glyph = 1;
            world.characters[2].Background = Color.Black;
            // This is the Game Loop.
            // Update game logic here.
        }
    }
}