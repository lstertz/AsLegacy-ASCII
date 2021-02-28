using Microsoft.Xna.Framework;
using SadConsole;
using System;
using Console = SadConsole.Console;

using AsLegacy.Global;
using SadConsole.Controls;
using AsLegacy.Characters;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines CompletionScreen, which serves as visual output controller for displaying 
    /// the details of a game's completion.
    /// </summary>
    public class CompletionScreen : ControlsConsole
    {
        private const string PlayAgainLabel = "Play Again";
        private static readonly int playAgainWidth = PlayAgainLabel.Length + 2;

        private const string PrefixMessage = "The Lineage of ";
        private const string CompletionMessage = " has won the game!";
        private const int CompletionMessageY = 6;

        private static ControlsConsole screen;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static new bool IsVisible
        {
            get => screen.IsVisible;
            set
            {
                screen.IsVisible = value;
                screen.IsFocused = value;
            }
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
                Position = new Point(0, CompletionMessageY),
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
                Text = PlayAgainLabel
            };
            playAgain.Click += (s, e) =>
            {
                World.Reset();
                Display.ShowScreen(Display.Screens.Settings);
            };
            Add(playAgain);
        }

        /// <summary>
        /// Updates the details of the CompletionScreen, primarily by updating 
        /// the completion message.
        /// </summary>
        /// <param name="delta">The time passed since the last Update call.</param>
        public override void Update(TimeSpan time)
        {
            if (!IsVisible)
                return;

            base.Update(time);

            string prefix = "";
            string name = World.HighestRankedCharacter.Name;
            if (World.HighestRankedCharacter is ILineal lineal)
            {
                prefix = PrefixMessage;
                name = lineal.LineageName;
            }

            message.DisplayText = prefix + name + CompletionMessage;
            message.IsDirty = true;
        }
    }
}
