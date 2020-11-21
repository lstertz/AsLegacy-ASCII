using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using SadConsole.Input;

namespace AsLegacy
{
    public class DirectionHandling : InputConsoleComponent
    {
        public override void ProcessKeyboard(Console console, Keyboard info, out bool handled)
        {
            throw new System.NotImplementedException();
        }

        public override void ProcessMouse(Console console, MouseConsoleState state, out bool handled)
        {
            handled = false;
            Display.Directions directions = console
                .GetComponent<Display.Directions>() as Display.Directions;

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

            directions.SetCellToHighlight(x + 1, y + 1);
        }
    }
}
