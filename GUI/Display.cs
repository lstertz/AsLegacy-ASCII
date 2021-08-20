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
        public enum Screen
        {
            Start,
            Settings,
            Play,
            Completion
        }

        public static Screen CurrentScreen { get; private set; }


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

            ShowScreen(Screen.Start);
        }

        /// <summary>
        /// Resets display elements.
        /// </summary>
        public static void Reset()
        {
            PlayScreen.Reset();
        }

        /// <summary>
        /// Changes the Display to show only the specified screen.
        /// </summary>
        /// <param name="screen">The screen to be shown.</param>
        public static void ShowScreen(Screen screen)
        {
            // TODO :: Add an optional fade transition or wait of some kind.

            StartScreen.IsVisible = screen == Screen.Start;
            SettingsScreen.IsVisible = screen == Screen.Settings;
            PlayScreen.IsVisible = screen == Screen.Play;
            CompletionScreen.IsVisible = screen == Screen.Completion;
            // TODO :: Implement for the other screens.

            CurrentScreen = screen;
        }
    }
}
