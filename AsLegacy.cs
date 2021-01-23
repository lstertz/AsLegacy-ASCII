using SadConsole.Components;
using System;

using Console = SadConsole.Console;
using Game = SadConsole.Game;

using AsLegacy.GUI;

namespace AsLegacy
{
    public class AsLegacy : UpdateConsoleComponent
    {
        /// <summary>
        /// The legacy (point) goal of the game.
        /// </summary>
        public const int Goal = 21;

        public static void Main()
        {
            new AsLegacy();
        }

        public AsLegacy()
        {
            Game.Create(Display.Width, Display.Height);
            Game.OnInitialize = () =>
            {
                World.Init();
                Display.Init(this);
            };

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        public override void Update(Console console, TimeSpan delta)
        {
            if (World.HighestRankedCharacter.Legacy < Goal)
                World.Update(delta.Milliseconds);
            else
            {
                // TODO :: 36 : Display the game completion screen.
                // TODO :: 36 : Create a Console that serves as the game completion screen.
            }
        }
    }
}