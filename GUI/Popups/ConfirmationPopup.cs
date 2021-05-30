using AsLegacy.Global;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using System;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the ConfirmationPopup, which asks the user to confirm whether 
    /// they want to complete an action.
    /// </summary>
    public class ConfirmationPopup : Popup
    {
        /// <summary>
        /// The action to be performed when the user confirms. 
        /// </summary>
        public Action OnConfirmation { get; set; } = null;

        /// <summary>
        /// The action to be performed when the user rejects.
        /// </summary>
        public Action OnRejection { get; set; } = null;

        /// <summary>
        /// The prompt displayed to the user.
        /// </summary>
        public string Prompt 
        { 
            get
            {
                return _prompt.DisplayText;
            }
            set
            {
                _prompt.DisplayText = value;
                _prompt.IsDirty = true;
            }
        }
        private Label _prompt;

        /// <summary>
        /// Constructs a new <see cref="ConfirmationPopup"/>.
        /// </summary>
        /// <param name="title">The title of the popup.</param>
        /// <param name="prompt">The text to be shown the confirmation prompt.</param>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public ConfirmationPopup(string title, string prompt, int width, int height) : 
            base(title, width, height, false)
        {
            _prompt = new(Width)
            {
                Alignment = HorizontalAlignment.Center,
                DisplayText = prompt,
                Position = new(0, height / 2),
                TextColor = Colors.White
            };
            Add(_prompt);

            Button yes = new(5, 1)
            {
                Position = new(width / 3 - 2,  height - 2),
                Text = "Yes"
            };
            yes.Click += (s, e) => Confirm();
            Add(yes);

            Button no = new(4, 1)
            {
                Position = new(2 * width / 3 - 2, height - 2),
                Text = "No"
            };
            no.Click += (s, e) => Reject();
            Add(no);
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();
            IsFocused = IsVisible;
        }

        /// <summary>
        /// Performs operations for confirmation of the prompt.
        /// </summary>
        private void Confirm()
        {
            IsVisible = false;
            OnConfirmation?.Invoke();
        }

        /// <summary>
        /// Performs operations for rejection of the prompt.
        /// </summary>
        private void Reject()
        {
            IsVisible = false;
            OnRejection?.Invoke();
        }
    }
}
