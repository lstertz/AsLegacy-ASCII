using SadConsole.Components;
using SadConsole.Input;
using SadConsole;
using AsLegacy.GUI.Screens;

namespace AsLegacy.Input
{
    /// <summary>
    /// Defines the Character Selection Handling Console Component to evaluate input 
    /// to either set the Player's character target or set the game's focused Character.
    /// </summary>
    public class CharacterSelectionHandling : MouseConsoleComponent
    {
        /// <inheritdoc/>
        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            handled = false;
            if (PlayScreenDisplaying.IsShowingPopup)
            {
                World.Character.Highlight(null);
                return;
            }

            int column = state.CellPosition.X;
            int row = state.CellPosition.Y;

            if (state.Mouse.LeftClicked)
                GameExecution.SelectCharacter(World.CharacterAt(row, column));
            else
                World.Character.Highlight(World.CharacterAt(row, column));
        }
    }
}
