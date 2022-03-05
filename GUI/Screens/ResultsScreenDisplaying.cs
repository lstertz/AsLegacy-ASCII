using Microsoft.Xna.Framework;
using SadConsole;
using System;

using AsLegacy.Global;
using SadConsole.Controls;
using AsLegacy.Characters;
using AsLegacy.Progression;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Handles the initialization and visibility of the results screen, which displays 
    /// the details of a game's completion.
    /// </summary>
    [Behavior]
    [Dependency<ConsoleCollection>(Binding.Unique, Fulfillment.Existing, Consoles)]
    [Dependency<DisplayContext>(Binding.Unique, Fulfillment.Existing, Display)]
    [Dependency<GameState>(Binding.Unique, Fulfillment.Existing, State)]
    public class ResultsScreenDisplaying : ControlsConsole
    {
        private const string Consoles = "consoles";
        private const string Display = "display";
        private const string State = "state";

        private const string PlayAgainLabel = "Play Again";
        private static readonly int PlayAgainWidth = PlayAgainLabel.Length + 2;

        private const string PrefixMessage = "The Lineage of ";
        private const string CompletionMessage = " has won the game!";
        private const int CompletionMessageY = 6;


        private Label _message;


        private ResultsScreenDisplaying() :
            // Workaround for dependencies not injected to constructors.
            base(GetContext<DisplayContext>().Width, GetContext<DisplayContext>().Height)
        {
            // Workarounds for dependencies not being injected to constructors.
            ConsoleCollection consoles = GetContext<ConsoleCollection>();
            GameState state = GetContext<GameState>();

            ThemeColors = Colors.StandardTheme;
            consoles.ScreenConsoles.Add(this);

            SetUpMessageLabel();
            SetUpRanking();
            SetUpPlayAgainButton();

            IsVisible = state.CurrentStage == GameStageMap.Stage.Results;
        }

        /// <summary>
        /// Updates the details of the CompletionScreen, primarily by updating 
        /// the completion message.
        /// </summary>
        /// <param name="delta">The time passed since the last Update call.</param>
        public override void Update(TimeSpan time)
        {
            if (!IsVisible)
                return;

            base.Update(time);

            string prefix = "";
            string name = World.HighestRankedCharacter.Name;
            if (World.HighestRankedCharacter is ILineal lineal)
            {
                prefix = PrefixMessage;
                name = lineal.LineageName;
            }

            _message.DisplayText = prefix + name + CompletionMessage;
            _message.IsDirty = true;
        }


        /// <summary>
        /// Sets up the results message label.
        /// </summary>
        private void SetUpMessageLabel()
        {
            _message = new(Width)
            {
                Alignment = HorizontalAlignment.Center,
                Position = new(0, CompletionMessageY),
                TextColor = Colors.White
            };
            Add(_message);
        }

        /// <summary>
        /// Sets up the play again button.
        /// </summary>
        private void SetUpPlayAgainButton()
        {
            Button playAgain = new(PlayAgainWidth, 1)
            {
                Position = new(Width / 2 - PlayAgainWidth / 2, Height - 3),
                Text = PlayAgainLabel
            };
            playAgain.Click += (s, e) => Contextualize(new GameRestartMessage());
            Add(playAgain);
        }

        /// <summary>
        /// Sets up the ranking displayed to show the results.
        /// </summary>
        private void SetUpRanking()
        {
            Ranking ranking = new(12)
            {
                Position = new(
                    Width / 2 - Ranking.TotalWidth / 2,
                    Height - 16)
            };
            Children.Add(ranking);
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
            bool isVisible = state.CurrentStage == GameStageMap.Stage.Results;

            IsVisible = isVisible;
            IsFocused = isVisible;
        }
    }
}
