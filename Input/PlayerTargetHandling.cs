using SadConsole.Components;
using SadConsole.Input;
using SadConsole;

namespace AsLegacy.Input
{
    /// <summary>
    /// Defines the Player Target Handling Console Component to evaluate input 
    /// to set the Player's character target.
    /// </summary>
    public class PlayerTargetHandling : MouseConsoleComponent
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

            int column = state.CellPosition.X;
            int row = state.CellPosition.Y;

            if (state.Mouse.LeftClicked)
                World.Player.Target = World.CharacterAt(row, column);
            else
                World.Character.Highlight(World.CharacterAt(row, column));
        }
    }
}
