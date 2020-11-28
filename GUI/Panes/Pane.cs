using Microsoft.Xna.Framework;
using SadConsole;

namespace AsLegacy
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
    }
}
