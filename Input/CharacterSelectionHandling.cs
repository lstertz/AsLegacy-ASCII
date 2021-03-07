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
        /// <summary>
        /// Handles mouse state changes.
        /// </summary>
        /// <param name="console">The Console to which this Component belongs.</param>
        /// <param name="state">The state of the mouse.</param>
        /// <param name="handled">A bool indicating whether the input was handled.</param>
        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            handled = false;
            if (PlayScreen.IsShowingPopup)
            {
                World.Character.Highlight(null);
                return;
            }

            int column = state.CellPosition.X;
            int row = state.CellPosition.Y;

            if (state.Mouse.LeftClicked)
                AsLegacy.SelectCharacter(World.CharacterAt(row, column));
            else
                World.Character.Highlight(World.CharacterAt(row, column));
        }
    }
}
