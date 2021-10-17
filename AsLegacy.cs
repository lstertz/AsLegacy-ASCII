using AsLegacy.Characters;
using AsLegacy.GUI;
using AsLegacy.GUI.Elements;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using SadConsole.Themes;
using System;

using Console = SadConsole.Console;
using Game = SadConsole.Game;

namespace AsLegacy
{
    /// <summary>
    /// Defines the manager of the primary game aspects, including the Player Character, 
    /// any focus Character, game system setup, and game system updating.
    /// </summary>
    public class AsLegacy : UpdateConsoleComponent
    {
        /// <summary>
        /// The legacy (point) goal of the game.
        /// </summary>
        public const int Goal = 25;

        /// <summary>
        /// The current Character being focused on by the game player.
        /// While playing, this is the World Player, when dead and in 'viewer mode' this may 
        /// be any other living Character.
        /// </summary>
        public static World.Character Focus { get => Player ?? ObservedCharacter; }
        private static World.Character ObservedCharacter = null;

        /// <summary>
        /// Specifies whether the game has a current Player Character.
        /// </summary>
        public static bool HasPlayer => Player != null;

        /// <summary>
        /// The Player Character of the game.
        /// This is null if the Player is in 'viewer mode'.
        /// </summary>
        public static Player Player => Player.Character;

        /// <summary>
        /// Entry execution point of the game.
        /// </summary>
        public static void Main()
        {
            _ = new AsLegacy();
        }

        /// <summary>
        /// Selects the provided Character, either as the target of 
        /// the Player Character or the focus Character of the game.
        /// </summary>
        /// <param name="character">The Character being selected.</param>
        public static void SelectCharacter(World.Character character)
        {
            if (Player != null)
                Player.Target = character;
            else
                ObservedCharacter = character;
        }

        /// <summary>
        /// Starts a new game with the Player Character having the 
        /// specified Character name and Lineage name.
        /// </summary>
        /// <param name="characterName">The name of the Player Character created 
        /// with the start of the game.</param>
        /// <param name="lineageName">The new of the Lineage that starts with 
        /// the new Player Character.</param>
        public static void StartGame(string characterName, string lineageName)
        {
            ObservedCharacter = null;
            Player.Reset();

            Class.Init();
            World.InitNewWorld();

            Point playerPosition = World.GetRandomPassablePosition(World.SpawnZone.Player);
            Player.Create(playerPosition.Y, playerPosition.X, characterName, lineageName);

            Display.Reset();
            Display.ShowScreen(Display.Screen.Play);
        }


        /// <summary>
        /// Constructs a new AsLegacy instance, which will initialize and update the game systems.
        /// </summary>
        public AsLegacy()
        {
            Game.Create(Display.Width, Display.Height);
            Game.OnInitialize = () =>
            {
                Library.Default.SetControlTheme(typeof(ClearingTextBox), new TextBoxTheme());
                SadConsole.Global.LoadFont("Resources/Fonts/AsLegacy.font");
                SadConsole.Global.FontDefault = SadConsole.Global.Fonts["AsLegacy"]
                    .GetFont(SadConsole.Font.FontSizes.One);
                Display.Init(this);
            };

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        /// <summary>
        /// Updates the game systems.
        /// </summary>
        /// <param name="console">The Console rendering the game.</param>
        /// <param name="delta">The time passed since the last update.</param>
        public override void Update(Console console, TimeSpan delta)
        {
            if (Display.CurrentScreen != Display.Screen.Play)
                return;

            if (World.HighestRankedCharacter.Legacy < Goal)
                World.Update(delta.Milliseconds);
            else if (Display.CurrentScreen != Display.Screen.Completion)
                Display.ShowScreen(Display.Screen.Completion);
        }
    }
}