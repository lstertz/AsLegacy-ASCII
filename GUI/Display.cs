using Console = SadConsole.Console;

using AsLegacy.GUI.Screens;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines Display, which serves as the primary visual output controller.
    /// </summary>
    public class Display
    {
        public enum Screens
        {
            Menu,
            Start,
            Play,
            Completion
        }


        /// <summary>
        /// Initializes the Display.
        /// Required for any display output to be rendered, or screens to be created.
        /// </summary>
        /// <param name="console">The Console to become the 
        /// initialized Display Screens' Console.</param>
        public static void Init(Console console)
        {
            PlayScreen.Init(console);
            // TODO :: Implement other screens.

            ShowScreen(Screens.Play); // TODO :: Start on Menu.
        }

        public static void ShowScreen(Screens screen)
        {
            PlayScreen.IsVisible = screen == Screens.Play;
            // TODO :: Implement for the other screens.
        }
    }
}
