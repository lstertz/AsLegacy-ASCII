using AsLegacy.Characters;
using AsLegacy.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the SuccessorPopup, which displays information and 
    /// options for the Player to define details about a successor.
    /// </summary>
    public class SuccessorPopup : Popup
    {
        private static readonly AsciiKey Backspace = AsciiKey.Get(Keys.Back, new KeyboardState());
        private static readonly AsciiKey Delete = AsciiKey.Get(Keys.Delete, new KeyboardState());

        /// <summary>
        /// The name of the successor.
        /// </summary>
        public string SuccessorName { get; set; }

        /// <inheritdoc/>
        protected override string Title
        {
            get
            {
                return "About Successor"; // TODO :: Update as prompt for successor details.
            }
        }

        private readonly Button _ok;

        /// <summary>
        /// Constructs a new <see cref="SuccessorPopup"/>.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public SuccessorPopup(int width, int height) : base("", width, height, false)
        {
            // TODO :: Add available points and passives.

            _ok = new Button(2, 1)
            {
                Position = new Point(width / 2 - 5, height - 2),
                Text = "Ok"
            };
            _ok.Click += (s, e) =>
            {
                IsVisible = false;
                Player.CreateSuccessor(SuccessorName);
            };
            Add(_ok);
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            IsFocused = IsVisible;
        }

        /// <summary>
        /// Validates the input key press.
        /// </summary>
        /// <param name="sender">The sender of the key input.</param>
        /// <param name="args">The args detailing the key input and whether it is valid.</param>
        private void ValidateInput(object sender, TextBox.KeyPressEventArgs args)
        {
            AsciiKey k = args.Key;
            if (k == Backspace || k == Delete)
                return;

            args.IsCancelled = !char.IsDigit(k.Character);
        }

        /// <summary>
        /// Updates the ok Button's enabled state.
        /// </summary>
        /// <param name="sender">The event sender that triggers the update.</param>
        /// <param name="args">The event args.</param>
        private void UpdateCreateSuccessorEnablement(object sender, EventArgs args)
        {
            // TODO :: Validate all points have been spent.
            _ok.IsEnabled = true;// name.Length > 0;
        }
    }
}
