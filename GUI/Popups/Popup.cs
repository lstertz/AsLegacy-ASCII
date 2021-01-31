using Microsoft.Xna.Framework;
using SadConsole;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines a Popup, a Console for displaying potentially 
    /// interactive data as a window displaying on top of other content.
    /// </summary>
    public class Popup : Console
    {
        public Popup(string title, string content, int width, int height) : base(width, height)
        {
            // TODO :: Render a frame around the window.

            Print(width / 2 - title.Length / 2, 0, title, Color.White);
            Print(0, 2, content, Color.White);
        }
    }
}
