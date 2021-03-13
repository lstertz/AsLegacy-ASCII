using Microsoft.Xna.Framework;
using SadConsole;
using System;
using Console = SadConsole.Console;

using AsLegacy.Global;
using SadConsole.Controls;
using SadConsole.Input;
using Microsoft.Xna.Framework.Input;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines SettingsScreen, which serves as visual output controller for displaying 
    /// the settings of a new game.
    /// </summary>
    public class SettingsScreen : ControlsConsole
    {
        private const string CharacterNamePrompt = "Who starts this legacy?";
        private const int CharacterPromptY = 7;
        private const int CharacterNameY = CharacterPromptY + 2;

        private const string CharacterNobiliary = "of";

        private const string PlayLabel = "Play";
        private static readonly int PlayLabelWidth = PlayLabel.Length + 2;
        private const int PlayOffsetY = 4;

        private static readonly AsciiKey Backspace = AsciiKey.Get(Keys.Back, new KeyboardState());
        private static readonly AsciiKey Delete = AsciiKey.Get(Keys.Delete, new KeyboardState());

        private static ControlsConsole Screen;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static new bool IsVisible
        {
            get => Screen.IsVisible;
            set
            {
                Screen.IsVisible = value;
                Screen.IsFocused = value;
            }
        }

        /// <summary>
        /// Initializes the StartScreen.
        /// Required for any screen output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// initialized CompletionScreen's Console's parent Console.</param>
        public static void Init(Console parentConsole)
        {
            if (Screen == null)
                Screen = new SettingsScreen(parentConsole);
        }


        private TextBox _lineageField;
        private TextBox _nameField;
        private Button _play;

        /// <summary>
        /// Constructs a new StartScreen for the given Console.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// new CompletionScreen's Console's parent Console.</param>
        private SettingsScreen(Console parentConsole) :
            base(parentConsole.Width, parentConsole.Height)
        {
            ThemeColors = Colors.StandardTheme;
            parentConsole.Children.Add(this);

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
                Position = new Point(Width / 2 - 12, CharacterNameY),
                TextAlignment = HorizontalAlignment.Right,
            };
            _nameField.KeyPressed += ValidateInput;
            _nameField.IsDirtyChanged += UpdatePlayEnablement;
            Add(_nameField);

            Add(new Label(CharacterNobiliary.Length)
            {
                Alignment = HorizontalAlignment.Center,
                DisplayText = CharacterNobiliary,
                Position = new Point(Width / 2 - 1, CharacterNameY),
                TextColor = Colors.White,
                UseMouse = false
            });

            _lineageField = new TextBox(10)
            {
                IsCaretVisible = true,
                MaxLength = 9,
                Position = new Point(Width / 2 + 2, CharacterNameY)
            };
            _lineageField.KeyPressed += ValidateInput;
            _lineageField.IsDirtyChanged += UpdatePlayEnablement;
            Add(_lineageField);

            _play = new Button(PlayLabelWidth, 1)
            {
                IsEnabled = false,
                Position = new Point(Width / 2 - PlayLabelWidth / 2, Height - PlayOffsetY),
                Text = PlayLabel
            };
            _play.Click += (s, e) => AsLegacy.StartGame(
                _nameField.EditingText, _lineageField.EditingText);
            Add(_play);

            FocusedControl = _nameField;
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
        /// Updates the play Button's enabled state.
        /// </summary>
        /// <param name="sender">The event sender that triggers the update.</param>
        /// <param name="args">The event args.</param>
        private void UpdatePlayEnablement(object sender, EventArgs args)
        {
            string name = _nameField.EditingText;
            if (name == null)
                name = _nameField.Text;

            string lineage = _lineageField.EditingText;
            if (lineage == null)
                lineage = _lineageField.Text;

            _play.IsEnabled = name.Length > 0 && lineage.Length > 0;
        }
    }
}
