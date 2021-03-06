﻿using Microsoft.Xna.Framework;
using SadConsole;
using System;
using Console = SadConsole.Console;

using AsLegacy.Global;
using SadConsole.Controls;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines StartScreen, which serves as visual output controller for displaying 
    /// the first screen of the game.
    /// </summary>
    public class StartScreen : ControlsConsole
    {
        private const string ContinueLabel = "Continue";
        private readonly int ContinueLabelWidth = ContinueLabel.Length + 2;

        private const string NewGameLabel = "New Game";
        private readonly int NewGameLabelWidth = NewGameLabel.Length + 2;

        private const string TitleMessage = "As Legacy";
        private const int TitleY = 10;

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
        /// Initializes the StartScreen.
        /// Required for any screen output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// initialized CompletionScreen's Console's parent Console.</param>
        public static void Init(Console parentConsole)
        {
            if (Screen == null)
                Screen = new StartScreen(parentConsole);
        }

        /// <summary>
        /// Constructs a new StartScreen for the given Console.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// new CompletionScreen's Console's parent Console.</param>
        private StartScreen(Console parentConsole) : 
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

            Button newGame = new Button(NewGameLabelWidth, 1)
            {
                Position = new Point(Width / 2 - NewGameLabelWidth / 2, Height - 8),
                Text = NewGameLabel
            };
            newGame.Click += (s, e) => Display.ShowScreen(Display.Screen.Settings);
            Add(newGame);

            Button continueGame = new Button(ContinueLabelWidth, 1)
            {
                Position = new Point(Width / 2 - ContinueLabelWidth / 2, Height - 6),
                Text = ContinueLabel,
                IsEnabled = false
            };
            continueGame.Click += (s, e) =>
            {
                throw new NotImplementedException("The functionality to continue a game " +
                    "has not yet been implemented.");
                // TODO :: Show continue options or load the last played game.
            };
            Add(continueGame);
        }
    }
}
