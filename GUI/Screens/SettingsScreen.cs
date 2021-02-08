using Microsoft.Xna.Framework;
using SadConsole;
using System;
using Console = SadConsole.Console;

using AsLegacy.Global;
using SadConsole.Controls;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines SettingsScreen, which serves as visual output controller for displaying 
    /// the settings of a new game.
    /// </summary>
    public class SettingsScreen : ControlsConsole
    {
        private const string PlayLabel = "Play";
        private readonly int playLabelWidth = PlayLabel.Length + 2;

        private const string TitleMessage = "New Game Settings";
        private const int TitleY = 2;

        private static ControlsConsole screen;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static new bool IsVisible
        {
            get => screen.IsVisible;
            set => screen.IsVisible = value;
        }

        /// <summary>
        /// Initializes the StartScreen.
        /// Required for any screen output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// initialized CompletionScreen's Console's parent Console.</param>
        public static void Init(Console parentConsole)
        {
            if (screen == null)
                screen = new SettingsScreen(parentConsole);
        }

        /// <summary>
        /// Constructs a new StartScreen for the given Console.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// new CompletionScreen's Console's parent Console.</param>
        private SettingsScreen(Console parentConsole) : 
            base(parentConsole.Width, parentConsole.Height)
        {
            ThemeColors = Colors.StandardTheme;
            parentConsole.Children.Add(this);

            Add(new Label(Width)
            {
                Alignment = HorizontalAlignment.Center,
                DisplayText = TitleMessage,
                Position = new Point(0, TitleY),
                TextColor = Colors.White
            });

            Button newGame = new Button(playLabelWidth, 1)
            {
                Position = new Point(Width / 2 - playLabelWidth / 2, Height - 4),
                Text = PlayLabel
            };
            newGame.Click += (s, e) =>
            {
                // TODO :: Process settings for the World.
                Display.ShowScreen(Display.Screens.Play);
            };
            Add(newGame);
        }
    }
}
