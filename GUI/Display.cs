using SadConsole.Components;

using AsLegacy.GUI.Screens;

namespace AsLegacy.GUI
{

    /// <summary>
    /// Defines the settings of the display, including what screen is currently being displayed.
    /// </summary>
    [Context]
    public class DisplayContext // TODO :: Rename to 'Display' when old Display can be removed.
    {
        /// <summary>
        /// The height of the display, in cells.
        /// </summary>
        public ReadonlyContextState<int> Height { get; init; } = 25;

        /// <summary>
        /// The width of the display, in cells.
        /// </summary>
        public ReadonlyContextState<int> Width { get; init; } = 80;

    }

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
            Console console = new(Width, Height);
            SadConsole.Global.CurrentScreen = console;
            console.Components.Add(gameManager);

            CompletionScreen.Init(console);

            ShowScreen(Screen.Start);
        }

        /// <summary>
        /// Changes the Display to show only the specified screen.
        /// </summary>
        /// <param name="screen">The screen to be shown.</param>
        public static void ShowScreen(Screen screen)
        {
            // TODO :: Add an optional fade transition or wait of some kind.

            CompletionScreen.IsVisible = screen == Screen.Completion;

            CurrentScreen = screen;
        }
    }
}
