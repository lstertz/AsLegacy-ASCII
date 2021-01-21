﻿using SadConsole.Components;
using System;

using Console = SadConsole.Console;
using Game = SadConsole.Game;

namespace AsLegacy
{
    public class AsLegacy : UpdateConsoleComponent
    {
        /// <summary>
        /// The legacy (point) goal of the game.
        /// </summary>
        public const int Goal = 21;

        public const int Width = 80;
        public const int Height = 25;

        public static void Main()
        {
            new AsLegacy();
        }

        public AsLegacy()
        {
            Game.Create(Width, Height);
            Game.OnInitialize = () =>
            {
                Console console = new Console(Width, Height);
                SadConsole.Global.CurrentScreen = console;
                console.Components.Add(this);

                World.Init();
                Display.Init(console);
            };

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        public override void Update(Console console, TimeSpan delta)
        {
            World.Update(delta.Milliseconds);
        }
    }
}