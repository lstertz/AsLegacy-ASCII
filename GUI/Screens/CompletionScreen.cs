using Microsoft.Xna.Framework;
using SadConsole;
using System;

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
        private static readonly int PlayAgainWidth = PlayAgainLabel.Length + 2;

        private const string PrefixMessage = "The Lineage of ";
        private const string CompletionMessage = " has won the game!";
        private const int CompletionMessageY = 6;

        private static ControlsConsole Screen;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static new bool IsVisible
        {
            get => Screen.IsVisible;
            set
            {
                Screen.IsVisible = value;
                Screen.IsFocused = value;
            }
        }

        /// <summary>
        /// Initializes the CompletionScreen.
        /// Required for any screen output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// initialized CompletionScreen's Console's parent Console.</param>
        public static void Init(Console parentConsole)
        {
            if (Screen == null)
                Screen = new CompletionScreen(parentConsole);
        }

        private readonly Label _message;

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

            _message = new Label(Width)
            {
                Alignment = HorizontalAlignment.Center,
                Position = new Point(0, CompletionMessageY),
                TextColor = Colors.White
            };
            Add(_message);

            Ranking ranking = new Ranking(12)
            {
                Position = new Point(
                    Width / 2 - Ranking.TotalWidth / 2,
                    Height - 16)
            };
            Children.Add(ranking);

            Button playAgain = new Button(PlayAgainWidth, 1)
            {
                Position = new Point(Width / 2 - PlayAgainWidth / 2, Height - 3),
                Text = PlayAgainLabel
            };
            playAgain.Click += (s, e) => Display.ShowScreen(Display.Screen.Settings);
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

            _message.DisplayText = prefix + name + CompletionMessage;
            _message.IsDirty = true;
        }
    }
}
