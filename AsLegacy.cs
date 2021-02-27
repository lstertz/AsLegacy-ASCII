using SadConsole.Components;
using System;

using Console = SadConsole.Console;
using Game = SadConsole.Game;

using AsLegacy.GUI;
using AsLegacy.Characters;

namespace AsLegacy
{
    public class AsLegacy : UpdateConsoleComponent
    {
        /// <summary>
        /// The legacy (point) goal of the game.
        /// </summary>
        public const int Goal = 21;

        /// <summary>
        /// The current Character being focused on by the game player.
        /// While playing, this is the World Player, when dead and in 'viewer mode' this may 
        /// be any other living Character.
        /// </summary>
        public static World.Character Focus { get => HasPlayer ? Player : focus; }
        private static World.Character focus = null;

        public static bool HasPlayer => Player != null;

        public static Player Player => World.Player;
        private static Player player = null;

        public static void Main()
        {
            new AsLegacy();
        }

        public static void SelectCharacter(World.Character character)
        {
            World.Player.Target = character;
            return;
            if (player != null)
                player.Target = character;
            else
                focus = character;
        }

        public static void StartGame()
        {

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
            else if (Display.CurrentScreen != Display.Screens.Completion)
                Display.ShowScreen(Display.Screens.Completion);
        }
    }
}