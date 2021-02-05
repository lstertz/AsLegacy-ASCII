using AsLegacy.Global;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines a Popup, a Console for displaying potentially 
    /// interactive data as a window displaying on top of other content.
    /// </summary>
    public class Popup : ControlsConsole
    {
        private string title;
        private string content;

        public Popup(string title, string content, int width, int height) : base(width, height)
        {
            ThemeColors = Colors.StandardTheme;

            Button close = new Button(1, 1)
            {
                Position = new Point(width - 3, 1),
                Text = "X"
            };
            close.Click += (s, e) => IsVisible = false;
            Add(close);

            this.title = title;
            this.content = content;
        }

        protected override void Invalidate()
        {
            base.Invalidate();

            SetGlyph(0, 0, 201, Colors.White, Colors.Black);
            Fill(new Rectangle(1, 0, Width - 2, 1), Colors.White, Colors.Black, 205);
            SetGlyph(Width - 1, 0, 187, Colors.White, Colors.Black);
            Fill(new Rectangle(Width - 1, 1, 1, Height - 2), Colors.White, Colors.Black, 186);
            SetGlyph(Width - 1, Height - 1, 188, Colors.White, Colors.Black);
            Fill(new Rectangle(1, Height - 1, Width - 2, 1), Colors.White, Colors.Black, 205);
            SetGlyph(0, Height - 1, 200, Colors.White, Colors.Black);
            Fill(new Rectangle(0, 1, 1, Height - 2), Colors.White, Colors.Black, 186);

            Fill(new Rectangle(1, 1, Width - 2, Height - 2), Colors.White, Colors.Black, 0);
            if (title != null)
                Print(Width / 2 - title.Length / 2, 1, title, Color.White);
            if (content != null)
                Print(2, 2, content, Color.White);
        }
    }
}
