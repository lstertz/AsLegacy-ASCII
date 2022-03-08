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
            if (!GameExecution.HasPlayer)
                return;

            info.InitialRepeatDelay = float.PositiveInfinity;
            info.RepeatDelay = float.PositiveInfinity;

            handled = true;
            if (PlayScreenDisplaying.IsShowingPopup)
                return;

            if (info.IsKeyPressed(Keys.Up) || info.IsKeyPressed(Keys.W))
                GameExecution.Player.MoveInDirection(World.Character.Direction.Up, () =>
                    info.IsKeyDown(Keys.Up) || info.IsKeyDown(Keys.W));
            else if (info.IsKeyPressed(Keys.Down) || info.IsKeyPressed(Keys.S))
                GameExecution.Player.MoveInDirection(World.Character.Direction.Down, () =>
                    info.IsKeyDown(Keys.Down) || info.IsKeyDown(Keys.S));
            else if (info.IsKeyPressed(Keys.Left) || info.IsKeyPressed(Keys.A))
                GameExecution.Player.MoveInDirection(World.Character.Direction.Left, () =>
                    info.IsKeyDown(Keys.Left) || info.IsKeyDown(Keys.A));
            else if (info.IsKeyPressed(Keys.Right) || info.IsKeyPressed(Keys.D))
                GameExecution.Player.MoveInDirection(World.Character.Direction.Right, () =>
                    info.IsKeyDown(Keys.Right) || info.IsKeyDown(Keys.D));
            else if (info.IsKeyPressed(Keys.D1) || info.IsKeyPressed(Keys.NumPad1))
                GameExecution.Player.InitiateSkill(GameExecution.Player.EquippedSkills[0]);
            else if (info.IsKeyPressed(Keys.D2) || info.IsKeyPressed(Keys.NumPad2))
                GameExecution.Player.InitiateSkill(GameExecution.Player.EquippedSkills[1]);
            else if (info.IsKeyPressed(Keys.D3) || info.IsKeyPressed(Keys.NumPad3))
                GameExecution.Player.InitiateSkill(GameExecution.Player.EquippedSkills[2]);
            else if (info.IsKeyPressed(Keys.D4) || info.IsKeyPressed(Keys.NumPad4))
                GameExecution.Player.InitiateSkill(GameExecution.Player.EquippedSkills[3]);
            else if (info.IsKeyPressed(Keys.D5) || info.IsKeyPressed(Keys.NumPad5))
                GameExecution.Player.InitiateSkill(GameExecution.Player.EquippedSkills[4]);
            else if (info.IsKeyPressed(Keys.D6) || info.IsKeyPressed(Keys.NumPad6))
                GameExecution.Player.InitiateSkill(GameExecution.Player.EquippedSkills[5]);

            if (info.IsKeyReleased(Keys.Space))
                GameExecution.Player.ToggleAttackMode();
            GameExecution.Player.EnableDefense(AltIsDown(info));
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

            if (!GameExecution.HasPlayer || PlayScreenDisplaying.IsShowingPopup)
                return;

            Commands c = console as Commands;
            int x = state.CellPosition.X - 1;
            int y = state.CellPosition.Y - 1;

            if (state.Mouse.LeftClicked)
            {
                if (x == 0)
                {
                    if (y == -1) // Up
                        handled = GameExecution.Player.MoveInDirection(World.Character.Direction.Up);
                    else if (y == 1) // Down
                        handled = GameExecution.Player.MoveInDirection(World.Character.Direction.Down);
                }
                else if (y == 0)
                {
                    if (x == -1) // Left
                        handled = GameExecution.Player.MoveInDirection(World.Character.Direction.Left);
                    else if (x == 1) // Right
                        handled = GameExecution.Player.MoveInDirection(World.Character.Direction.Right);
                }

                c?.SetCellToHighlight(-1, -1);
            }
            else
                c?.SetCellToHighlight(x + 1, y + 1);
        }
    }
}
