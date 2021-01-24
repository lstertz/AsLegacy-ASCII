using Microsoft.Xna.Framework;
using SadConsole;
using System;
using Console = SadConsole.Console;

using AsLegacy.Global;
using SadConsole.Controls;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines CompletionScreen, which serves as visual output controller for displaying 
    /// the details of a game's completion.
    /// </summary>
    public class CompletionScreen : ControlsConsole
    {
        private const string playAgainLabel = "Play Again";
        private static readonly int playAgainWidth = playAgainLabel.Length + 2;

        private const string completionMessage = " has won the game!";
        private const int completionMessageY = 6;

        private static ControlsConsole screen;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static new bool IsVisible
        {
            get => screen.IsVisible;
            set => screen.IsVisible = value;
        }

        private readonly Label message;

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
        private CompletionScreen(Console parentConsole) : 
            base(parentConsole.Width, parentConsole.Height)
        {
            ThemeColors = Colors.StandardTheme;
            parentConsole.Children.Add(this);

            message = new Label(Width)
            {
                Alignment = HorizontalAlignment.Center,
                Position = new Point(0, completionMessageY),
                TextColor = Colors.White
            };
            Add(message);

            Ranking ranking = new Ranking(12)
            {
                Position = new Point(
                    Width / 2 - Ranking.TotalWidth / 2,
                    Height - 16)
            };
            Children.Add(ranking);

            Button playAgain = new Button(playAgainWidth, 1)
            {
                Position = new Point(Width / 2 - playAgainWidth / 2, Height - 3),
                Text = playAgainLabel
            };
            //playAgain.Click += (s, e) => CurrentPaneIndex--;
            Add(playAgain);
        }

        /// <summary>
        /// Updates the details of the CompletionScreen, primarily by updating 
        /// the completion message.
        /// </summary>
        /// <param name="delta">The time passed since the last Update call.</param>
        public override void Update(TimeSpan time)
        {
            base.Update(time);

            message.DisplayText = World.HighestRankedCharacter.Name + completionMessage;
            message.IsDirty = true;
        }
    }
}
