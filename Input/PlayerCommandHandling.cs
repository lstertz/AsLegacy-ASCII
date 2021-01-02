using SadConsole;
using SadConsole.Components;
using SadConsole.Input;

using Keys = Microsoft.Xna.Framework.Input.Keys;

using AsLegacy.GUI;

namespace AsLegacy.Input
{
    /// <summary>
    /// Defines a Console Component to handle inputs (mouse and keyboard) for 
    /// a Console that represents Commands available to the Player.
    /// </summary>
    public class PlayerCommandHandling : InputConsoleComponent
    {
        /// <summary>
        /// Handles keyboard state changes.
        /// </summary>
        /// </summary>
        /// <param name="console">The Console to which this Component belongs.</param>
        /// <param name="info">The state of the keyboard.</param>
        /// <param name="handled">A bool indicating whether the input was handled.</param>
        public override void ProcessKeyboard(Console console, Keyboard info, out bool handled)
        {
            info.InitialRepeatDelay = float.PositiveInfinity;
            info.RepeatDelay = float.PositiveInfinity;

            handled = true;

            if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.W))
                World.Player.MoveInDirection(World.Character.Direction.Up, () =>
                    info.IsKeyDown(Keys.Up) || info.IsKeyDown(Keys.W));
            else if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.S))
                World.Player.MoveInDirection(World.Character.Direction.Down, () =>
                    info.IsKeyDown(Keys.Down) || info.IsKeyDown(Keys.S));
            else if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.A))
                World.Player.MoveInDirection(World.Character.Direction.Left, () =>
                    info.IsKeyDown(Keys.Left) || info.IsKeyDown(Keys.A));
            else if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.D))
                World.Player.MoveInDirection(World.Character.Direction.Right, () =>
                    info.IsKeyDown(Keys.Right) || info.IsKeyDown(Keys.D));

            if (info.IsKeyReleased(Keys.Space))
                World.Player.ToggleAttackMode();
            World.Player.EnableDefense(AltIsDown(info));
        }

        /// <summary>
        /// Specifies whether an Alt key is currently pressed down.
        /// </summary>
        /// <param name="info">The keyboard state being checked.</param>
        /// <returns>True if an Alt key (either left or right) is currently 
        /// pressed down.</returns>
        private bool AltIsDown(Keyboard info)
        {
            return info.IsKeyDown(Keys.LeftAlt) || info.IsKeyDown(Keys.RightAlt);
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

            Commands c = console as Commands;
            int x = state.CellPosition.X - 1;
            int y = state.CellPosition.Y - 1;

            if (state.Mouse.LeftClicked)
            {
                if (x == 0)
                {
                    if (y == -1) // Up
                        handled = World.Player.MoveInDirection(World.Character.Direction.Up);
                    else if (y == 1) // Down
                        handled = World.Player.MoveInDirection(World.Character.Direction.Down);
                }
                else if (y == 0)
                {
                    if (x == -1) // Left
                        handled = World.Player.MoveInDirection(World.Character.Direction.Left);
                    else if (x == 1) // Right
                        handled = World.Player.MoveInDirection(World.Character.Direction.Right);
                }

                c?.SetCellToHighlight(-1, -1);
            }
            else
                c?.SetCellToHighlight(x + 1, y + 1);
        }
    }
}
