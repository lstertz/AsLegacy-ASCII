using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;

namespace AsLegacy.GUI.Panes
{
    /// <summary>
    /// Defines a Pane, a Console for displaying potentially 
    /// interactive data.
    /// </summary>
    public class Pane : Console
    {
        public Pane(string title, string content, int width, int height) : base(width, height)
        {
            Print(width / 2 - title.Length / 2, 0, title, Color.White);
            Print(0, 2, content, Color.White);
        }

        /// <summary>
        /// Overrides mouse processing to ignore mouse input for Panes.
        /// </summary>
        /// <param name="state">The current state of the mouse.</param>
        /// <returns>False, so processing can fall to other consoles.</returns>
        public override bool ProcessMouse(MouseConsoleState state)
        {
            return false;
        }
    }
}
