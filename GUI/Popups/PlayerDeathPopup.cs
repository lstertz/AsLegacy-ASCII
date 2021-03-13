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
    /// Defines the PlayerDeathPopup, which displays information and 
    /// options for the Player upon their Character's death.
    /// </summary>
    public class PlayerDeathPopup : Popup
    {
        private const string CharacterNamePrompt = "Who will continue this legacy?";
        private const int CharacterPromptY = 4;
        private const int CharacterNameY = CharacterPromptY + 2;

        private static readonly AsciiKey Backspace = AsciiKey.Get(Keys.Back, new KeyboardState());
        private static readonly AsciiKey Delete = AsciiKey.Get(Keys.Delete, new KeyboardState());

        /// <inheritdoc/>
        protected override string Title
        {
            get
            {
                if (Player.Character != null)
                    return Player.Character.Name + " has died!";
                return base.Title;
            }
        }

        private Button _createSuccessor;
        private TextBox _nameField;

        /// <summary>
        /// Constructs a new PlayerDeathPopup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public PlayerDeathPopup(int width, int height) : base("", "", width, height, false)
        {
            Add(new Label(Width)
            {
                Alignment = HorizontalAlignment.Center,
                DisplayText = CharacterNamePrompt,
                Position = new Point(0, CharacterPromptY),
                TextColor = Colors.White
            });

            _nameField = new TextBox(10)
            {
                IsCaretVisible = true,
                MaxLength = 9,
                Position = new Point(Width / 2 - 5, CharacterNameY)
            };
            _nameField.KeyPressed += ValidateInput;
            _nameField.IsDirtyChanged += UpdateCreateSuccessorEnablement;
            Add(_nameField);

            Button quit = new Button(6, 1)
            {
                Position = new Point(5,  height - 2),
                Text = "Quit"
            };
            quit.Click += (s, e) =>
            {
                IsVisible = false;
                Display.ShowScreen(Display.Screen.Start);
            };
            Add(quit);

            _createSuccessor = new Button(10, 1)
            {
                Position = new Point(width / 2 - 5, height - 2),
                Text = "Continue"
            };
            _createSuccessor.Click += (s, e) =>
            {
                IsVisible = false;
                Player.CreateSuccessor(_nameField.EditingText);
            };
            Add(_createSuccessor);

            Button observe = new Button(9, 1)
            {
                Position = new Point(width - 12, height - 2),
                IsEnabled = false,
                Text = "Observe"
            };
            observe.Click += (s, e) => IsVisible = false;
            Add(observe);

            FocusedControl = _nameField;
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            // TODO :: Update with an addendum of some kind by default, e.g. Player II.
            if (IsVisible)
              _nameField.Text = Player.Character.Name;
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

            args.IsCancelled = !char.IsLetterOrDigit(k.Character);
        }

        /// <summary>
        /// Updates the createSuccessor Button's enabled state.
        /// </summary>
        /// <param name="sender">The event sender that triggers the update.</param>
        /// <param name="args">The event args.</param>
        private void UpdateCreateSuccessorEnablement(object sender, EventArgs args)
        {
            string name = _nameField.EditingText;
            if (name == null)
                name = _nameField.Text;

            _createSuccessor.IsEnabled = name.Length > 0;
        }
    }
}
