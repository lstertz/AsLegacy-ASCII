using AsLegacy.Characters;
using Microsoft.Xna.Framework;
using SadConsole.Controls;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the PlayerDeathPopup, which displays information and 
    /// options for the Player upon their Character's death.
    /// </summary>
    public class PlayerDeathPopup : Popup
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
        /// Constructs a new PlayerDeathPopup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public PlayerDeathPopup(int width, int height) : base("", "", width, height, false)
        {
            Button quit = new Button(6, 1)
            {
                Position = new Point(5,  height - 2),
                Text = "Quit"
            };
            quit.Click += (s, e) =>
            {
                IsVisible = false;
                Display.ShowScreen(Display.Screens.Start);
            };
            Add(quit);

            Button createSuccessor = new Button(10, 1)
            {
                Position = new Point(width / 2 - 5, height - 2),
                Text = "Continue"
            };
            createSuccessor.Click += (s, e) =>
            {
                IsVisible = false;
                Player.CreateSuccessor(Player.Character.Name);
            };
            Add(createSuccessor);
            // TODO :: 55 : Add Textbox for Successor's name.

            Button observe = new Button(9, 1)
            {
                Position = new Point(width - 12, height - 2),
                IsEnabled = false,
                Text = "Observe"
            };
            observe.Click += (s, e) => IsVisible = false;
            Add(observe);
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            base.Invalidate();

            // TODO :: 55 : Prompt Player for Successor Name.
        }
    }
}
