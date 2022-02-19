using Microsoft.Xna.Framework;
using SadConsole;
using Console = SadConsole.Console;

using AsLegacy.Global;
using SadConsole.Controls;
using AsLegacy.Configs;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines StartScreen, which serves as visual output controller for displaying 
    /// the first screen of the game.
    /// </summary>
    public class StartScreen : ControlsConsole
    {
        private const int GameModeInitialYOffset = -8;
        private const int GameModeOffsetShift = 2;

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

            ConfigurationSelection selection = GetContext<ConfigurationSelection>();
            ConfigurationSet configurations = GetContext<ConfigurationSet>();
            string[] gameConfigs = configurations.ConfigurationNames;
            for (int c = 0, count = gameConfigs.Length; c < count; c++)
            {
                int option = c;

                int width = gameConfigs[option].Length + 2;
                int yOffset = GameModeInitialYOffset + (GameModeOffsetShift * option);
                string config = gameConfigs[option];

                Button button = new(width, 1)
                {
                    Position = new Point(Width / 2 - width / 2, Height + yOffset),
                    Text = config
                };
                button.Click += (s, e) =>
                {
                    Display.ShowScreen(Display.Screen.Settings);
                    selection.ConfigurationFile.Value = configurations.ConfigurationFiles[option];
                    selection.ConfigurationName.Value = configurations.ConfigurationNames[option];
                };
                Add(button);
            }
        }
    }
}
