using SadConsole.Components;
using Console = SadConsole.Console;

using AsLegacy.GUI.Screens;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines Display, which serves as the primary visual output controller.
    /// </summary>
    public class Display
    {
        public const int Width = 80;
        public const int Height = 25;

        /// <summary>
        /// The screens available for the Display to show.
        /// </summary>
        public enum Screens
        {
            Start,
            Settings,
            Play,
            Completion
        }


        public static Screens CurrentScreen { get; private set; }

        /// <summary>
        /// Initializes the Display.
        /// Required for any display output to be rendered, or screens to be created.
        /// </summary>
        /// <param name="gameManager">The UpdateConsoleComponent whose Update 
        /// directs the processes of the game's progression.</param>
        public static void Init(UpdateConsoleComponent gameManager)
        {
            Console console = new Console(Width, Height);
            SadConsole.Global.CurrentScreen = console;
            console.Components.Add(gameManager);

            StartScreen.Init(console);
            SettingsScreen.Init(console);
            PlayScreen.Init(console);
            CompletionScreen.Init(console);
            // TODO :: Implement other screens.

            ShowScreen(Screens.Start);
        }

        /// <summary>
        /// Changes the Display to show only the specified screen.
        /// </summary>
        /// <param name="screen">The screen to be shown.</param>
        public static void ShowScreen(Screens screen)
        {
            // TODO :: Add an optional fade transition or wait of some kind.

            StartScreen.IsVisible = screen == Screens.Start;
            SettingsScreen.IsVisible = screen == Screens.Settings;
            PlayScreen.IsVisible = screen == Screens.Play;
            CompletionScreen.IsVisible = screen == Screens.Completion;
            // TODO :: Implement for the other screens.

            CurrentScreen = screen;
        }
    }
}
