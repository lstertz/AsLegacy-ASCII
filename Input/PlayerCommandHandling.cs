using SadConsole;
using SadConsole.Components;
using SadConsole.Input;

using Keys = Microsoft.Xna.Framework.Input.Keys;

using AsLegacy.GUI;
using AsLegacy.GUI.Screens;

namespace AsLegacy.Input
{
    /// <summary>
    /// Defines a Console Component to handle inputs (mouse and keyboard) for 
    /// a Console that represents Commands available to the Player.
    /// </summary>
    public class PlayerCommandHandling : InputConsoleComponent
    {
        /// <inheritdoc/>
        public override void ProcessKeyboard(Console console, Keyboard info, out bool handled)
        {
            handled = false;
            if (!AsLegacy.HasPlayer)
                return;

            info.InitialRepeatDelay = float.PositiveInfinity;
            info.RepeatDelay = float.PositiveInfinity;

            handled = true;
            if (PlayScreen.IsShowingPopup)
                return;

            if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.W))
                AsLegacy.Player.MoveInDirection(World.Character.Direction.Up, () =>
                    info.IsKeyDown(Keys.Up) || info.IsKeyDown(Keys.W));
            else if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.S))
                AsLegacy.Player.MoveInDirection(World.Character.Direction.Down, () =>
                    info.IsKeyDown(Keys.Down) || info.IsKeyDown(Keys.S));
            else if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.A))
                AsLegacy.Player.MoveInDirection(World.Character.Direction.Left, () =>
                    info.IsKeyDown(Keys.Left) || info.IsKeyDown(Keys.A));
            else if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.D))
                AsLegacy.Player.MoveInDirection(World.Character.Direction.Right, () =>
                    info.IsKeyDown(Keys.Right) || info.IsKeyDown(Keys.D));

            if (info.IsKeyReleased(Keys.Space))
                AsLegacy.Player.ToggleAttackMode();
            AsLegacy.Player.EnableDefense(AltIsDown(info));
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

        /// <inheritdoc/>
        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            handled = false;

            if (!AsLegacy.HasPlayer || PlayScreen.IsShowingPopup)
                return;

            Commands c = console as Commands;
            int x = state.CellPosition.X - 1;
            int y = state.CellPosition.Y - 1;

            if (state.Mouse.LeftClicked)
            {
                if (x == 0)
                {
                    if (y == -1) // Up
                        handled = AsLegacy.Player.MoveInDirection(World.Character.Direction.Up);
                    else if (y == 1) // Down
                        handled = AsLegacy.Player.MoveInDirection(World.Character.Direction.Down);
                }
                else if (y == 0)
                {
                    if (x == -1) // Left
                        handled = AsLegacy.Player.MoveInDirection(World.Character.Direction.Left);
                    else if (x == 1) // Right
                        handled = AsLegacy.Player.MoveInDirection(World.Character.Direction.Right);
                }

                c?.SetCellToHighlight(-1, -1);
            }
            else
                c?.SetCellToHighlight(x + 1, y + 1);
        }
    }
}
