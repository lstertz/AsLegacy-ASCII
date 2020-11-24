using SadConsole;
using SadConsole.Components;
using SadConsole.Input;

using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Console Component to handle inputs (mouse and keyboard) for 
    /// a Console that represents Directions available to the Player.
    /// </summary>
    public class DirectionHandling : InputConsoleComponent
    {
        // TODO :: Refactor to account for handling other keyboard input for 
        //          the remaining Player controls (mode changes, skill activation, etc.).

        /// <summary>
        /// Handles keyboard state changes.
        /// </summary>
        /// </summary>
        /// <param name="console">The Console to which this Component belongs.</param>
        /// <param name="info">The state of the keyboard.</param>
        /// <param name="handled">A bool indicating whether the input was handled.</param>
        public override void ProcessKeyboard(Console console, Keyboard info, out bool handled)
        {
            handled = true;

            if (info.IsKeyReleased(Keys.Up))
                World.Player.PerformInDirection(World.PresentCharacter.Direction.Up);
            else if (info.IsKeyReleased(Keys.Down))
                World.Player.PerformInDirection(World.PresentCharacter.Direction.Down);
            else if (info.IsKeyReleased(Keys.Left))
                World.Player.PerformInDirection(World.PresentCharacter.Direction.Left);
            else if (info.IsKeyReleased(Keys.Right))
                World.Player.PerformInDirection(World.PresentCharacter.Direction.Right);
        }

        /// <summary>
        /// Handles mouse state changes.
        /// </summary>
        /// <param name="console">The Console to which this Component belongs.</param>
        /// <param name="state">The state of the mouse.</param>
        /// <param name="handled">A bool indicating whether the input was handled.</param>
        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            handled = false;

            int x = state.CellPosition.X - 1;
            int y = state.CellPosition.Y - 1;

            if (state.Mouse.LeftClicked)
            {
                if (x == 0)
                {
                    if (y == -1) // Up
                        World.Player.PerformInDirection(World.PresentCharacter.Direction.Up);
                    else if (y == 1) // Down
                        World.Player.PerformInDirection(World.PresentCharacter.Direction.Down);
                    y -= y;
                }
                else if (y == 0)
                {
                    if (x == -1) // Left
                        World.Player.PerformInDirection(World.PresentCharacter.Direction.Left);
                    else if (x == 1) // Right
                        World.Player.PerformInDirection(World.PresentCharacter.Direction.Right);
                    x -= x;
                }
            }

            Display.Directions directions = console
                .GetComponent<Display.Directions>() as Display.Directions;
            directions.SetCellToHighlight(x + 1, y + 1);
        }
    }
}
