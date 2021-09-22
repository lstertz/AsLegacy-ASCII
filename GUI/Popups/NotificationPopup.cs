using SadConsole.Controls;
using System;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the NotificationPopup, which notifies the user through a message 
    /// and provides ways to dismiss the popup.
    /// </summary>
    public class NotificationPopup : DynamicContentPopup
    {
        /// <inheritdoc/>
        public override int Height 
        {
            get => base.Height + 2;
            protected set
            {
                base.Height = value;

                if (_ok != null)
                    _ok.Position = new(Width / 2 - 1, Height - 2);
            }
        }

        /// <inheritdoc/>
        public override int Width
        {
            get => base.Width;
            protected set
            {
                base.Width = value;

                if (_ok != null)
                    _ok.Position = new(Width / 2 - 1, Height - 2);
            }
        }


        private readonly Button _ok;

        /// <summary>
        /// Callback upon the dismissal (or general closing) of the popup.
        /// </summary>
        public Action OnDismissal { get; set; }

        /// <summary>
        /// Constructs a new <see cref="NotificationPopup"/>.
        /// </summary>
        /// <param name="notification">The message to be shown to the user.</param>
        /// <param name="width">The width of the popup window.</param>
        /// <param name="height">The height of the popup window.</param>
        public NotificationPopup(string title, int minWidth, int maxWidth, int maxHeight) : 
            base(title, minWidth, maxWidth, maxHeight, true, SadConsole.HorizontalAlignment.Center)
        {
            _ok = new(4, 1)
            {
                Position = new(Width / 2 - 2,  Height - 2),
                Text = "Ok"
            };
            _ok.Click += (s, e) => OnClose();
            Add(_ok);
        }

        /// <summary>
        /// Handles the closing of the popup, either through dismissal or the standard close.
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            OnDismissal();
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();
            IsFocused = IsVisible;
        }
    }
}
