using AsLegacy.Characters;
using AsLegacy.Global;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the PlayerDeath Popup, which displays information and 
    /// options for the Player upon their Character's death.
    /// </summary>
    public class PlayerDeath : Popup
    {
        /// <inheritdoc/>
        protected override string title
        {
            get
            {
                if (Player.Character != null)
                    return Player.Character.Name + " has died!";
                return base.title;
            }
        }

        /// <summary>
        /// Constructs a new PlayerDeath Popup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public PlayerDeath(int width, int height) : base("", "", width, height, false)
        {
            /*
            Button close = new Button(1, 1)
            {
                Position = new Point(width - 3, 1),
                Text = "X"
            };
            close.Click += (s, e) => IsVisible = false;
            Add(close);
            */

            // TODO :: Add Continue, Observe, and Quit buttons.

            // TODO :: 55 : Add Textbox for Successor's name.
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            base.Invalidate();

            // TODO :: 55 : Prompt Player for Successor Name.
        }
    }
}
