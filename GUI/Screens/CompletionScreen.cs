using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using System;
using System.Collections.Generic;
using Console = SadConsole.Console;

using AsLegacy.GUI.HUDs;
using AsLegacy.Input;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines CompletionScreen, which serves as visual output controller for displaying 
    /// the details of a game's completion.
    /// </summary>
    public class CompletionScreen : DrawConsoleComponent
    {
        private const string completionMessage = " has won the game!";
        private const int messageY = 6;

        private static CompletionScreen screen;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static bool IsVisible
        {
            get => screen.console.IsVisible;
            set => screen.console.IsVisible = value;
        }

        private readonly Console console;


        /// <summary>
        /// Initializes the CompletionScreen.
        /// Required for any screen output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// initialized CompletionScreen's Console's parent Console.</param>
        public static void Init(Console parentConsole)
        {
            if (screen == null)
                screen = new CompletionScreen(parentConsole);
        }

        /// <summary>
        /// Constructs a new CompletionScreen for the given Console.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// new CompletionScreen's Console's parent Console.</param>
        private CompletionScreen(Console parentConsole)
        {
            console = new Console(Display.Width, Display.Height);
            parentConsole.Children.Add(console);

            Ranking ranking = new Ranking(12)
            {
                Position = new Point(
                    Display.Width / 2 - Ranking.TotalWidth / 2, 
                    Display.Height - 13)
            };
            console.Children.Add(ranking);

            console.Components.Add(this);
        }

        /// <summary>
        /// Updates the rendering of the CompletionScreen, primarily by updating 
        /// the completion message.
        /// </summary>
        /// <param name="console">The Console of the CompletionScreen.</param>
        /// <param name="delta">The time passed since the last Draw call.</param>
        public override void Draw(Console console, TimeSpan delta)
        {
            string message = World.HighestRankedCharacter.Name + completionMessage;

            console.Clear(0, Display.Height / 2, Display.Width);
            console.Print(Display.Width / 2 - message.Length / 2, messageY, message);
        }
    }
}
