using Microsoft.Xna.Framework;
using SadConsole;
using System;

using AsLegacy.Global;
using SadConsole.Controls;
using SadConsole.Input;
using Microsoft.Xna.Framework.Input;
using AsLegacy.Progression;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Handles the initialization and visibility of the settings screen.
    /// </summary>
    [Behavior]
    [Dependency<ConsoleCollection>(Binding.Unique, Fulfillment.Existing, Consoles)]
    [Dependency<DisplayContext>(Binding.Unique, Fulfillment.Existing, Display)]
    [Dependency<GameState>(Binding.Unique, Fulfillment.Existing, State)]
    public class SetupScreenDisplaying : ControlsConsole
    {
        private const string Consoles = "consoles";
        private const string Display = "display";
        private const string State = "state";

        private const string CharacterNamePrompt = "Who starts this legacy?";
        private const int CharacterPromptY = 7;
        private const int CharacterNameY = CharacterPromptY + 2;

        private const string CharacterNobiliary = "of";

        private const string PlayLabel = "Play";
        private static readonly int PlayLabelWidth = PlayLabel.Length + 2;
        private const int PlayOffsetY = 4;

        private static readonly AsciiKey Backspace = AsciiKey.Get(Keys.Back, new KeyboardState());
        private static readonly AsciiKey Delete = AsciiKey.Get(Keys.Delete, new KeyboardState());


        private TextBox _lineageField;
        private TextBox _nameField;
        private Button _play;


        private SetupScreenDisplaying() :
            // Workaround for dependencies not injected to constructors.
            base(GetContext<DisplayContext>().Width, GetContext<DisplayContext>().Height)
        {
            // Workaround for dependencies not being injected to constructors.
            ConsoleCollection consoles = GetContext<ConsoleCollection>();

            ThemeColors = Colors.StandardTheme;
            consoles.ScreenConsoles.Add(this);

            SetUpNameField();
            SetUpNobiliaryLabel();
            SetUpLineageField();
            SetUpPlayButton();

            FocusedControl = _nameField;
        }
        
        /// <summary>
        /// Sets up the lineage field for the player to specify their characters' lineage.
        /// </summary>
        private void SetUpLineageField()
        {
            _lineageField = new TextBox(10)
            {
                IsCaretVisible = true,
                MaxLength = 9,
                Position = new Point(Width / 2 + 2, CharacterNameY)
            };
            _lineageField.KeyPressed += ValidateInput;
            _lineageField.IsDirtyChanged += UpdatePlayEnablement;
            Add(_lineageField);
        }

        /// <summary>
        /// Sets up the name field for the player to specify their starting character's name.
        /// </summary>
        private void SetUpNameField()
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
                Position = new Point(Width / 2 - 12, CharacterNameY),
                TextAlignment = HorizontalAlignment.Right,
            };
            _nameField.KeyPressed += ValidateInput;
            _nameField.IsDirtyChanged += UpdatePlayEnablement;
            Add(_nameField);
        }

        /// <summary>
        /// Sets up the nobiliary label, which displays the nobiliary between the 
        /// character name and the lineage.
        /// </summary>
        private void SetUpNobiliaryLabel()
        {
            Add(new Label(CharacterNobiliary.Length)
            {
                Alignment = HorizontalAlignment.Center,
                DisplayText = CharacterNobiliary,
                Position = new Point(Width / 2 - 1, CharacterNameY),
                TextColor = Colors.White,
                UseMouse = false
            });
        }

        /// <summary>
        /// Sets up the play button, which finalizes the settings and initiates the 
        /// setting up and playing of the game.
        /// </summary>
        private void SetUpPlayButton()
        {
            _play = new Button(PlayLabelWidth, 1)
            {
                IsEnabled = false,
                Position = new Point(Width / 2 - PlayLabelWidth / 2, Height - PlayOffsetY),
                Text = PlayLabel
            };
            _play.Click += (s, e) => Contextualize(new GameSetUpMessage()
            {
                Lineage = _lineageField.EditingText,
                Name = _nameField.EditingText
            });
            Add(_play);
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


        /// <summary>
        /// Updates whether this console is visible based on the current stage of the game.
        /// </summary>
        /// <remarks>
        /// The console will gain focus if it becomes visible and lose focus if it is hidden.
        /// </remarks>
        [Operation]
        [OnChange(State, nameof(GameState.CurrentStage))]
        public void UpdateVisibility(GameState state)
        {
            bool isVisible = state.CurrentStage == GameStageMap.Stage.Setup;

            IsVisible = isVisible;
            IsFocused = isVisible;
        }
    }
}
