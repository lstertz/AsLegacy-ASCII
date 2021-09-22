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
    public abstract class Popup : ControlsConsole
    {
        /// <summary>
        /// The title of the Popup, displayed center top of its window.
        /// </summary>
        public virtual string Title { get; set; }

        /// <inheritdoc cref="CellSurface.Height"/>
        public virtual new int Height { get => base.Height; protected set => base.Height = value; }

        /// <inheritdoc cref="CellSurface.Width"/>
        public virtual new int Width { get => base.Width; protected set => base.Width = value; }

        private readonly Button _close;


        /// <summary>
        /// Constructs a new Popup.
        /// </summary>
        /// <param name="title">The title of the Popup, displayed center top of its window.</param>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public Popup(string title, int width, int height,
            bool hasCloseButton = true) : base(width, height)
        {
            ThemeColors = Colors.StandardTheme;

            if (hasCloseButton)
            {
                _close = new Button(1, 1)
                {
                    Position = new(width - 3, 1),
                    Text = "X"
                };
                _close.Click += (s, e) => OnClose();
                Add(_close);
            }

            Title = title;
        }

        /// <inheritdoc/>
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
            if (Title != null)
                Print(Width / 2 - Title.Length / 2, 1, Title, Color.White);
        }

        /// <summary>
        /// Handles the closing (through the standard close button) of the popup.
        /// </summary>
        protected virtual void OnClose()
        {
            IsVisible = false;
        }

        /// <summary>
        /// Updates the close button's position, if there is a close button.
        /// </summary>
        /// <param name="baseWidth">The width that the close button should be based upon.</param>
        protected void UpdateCloseButtonPosition(int baseWidth)
        {
            if (_close != null)
                _close.Position = new(baseWidth - 3, 1);
        }
    }
}
