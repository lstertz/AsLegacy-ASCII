using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using System;
using Console = SadConsole.Console;

namespace AsLegacy
{
    public partial class Display : DrawConsoleComponent
    {
        /// <summary>
        /// Defines the Directions aspect of a Display, 
        /// which is responsible for updating the directions 
        /// available for Player movement.
        /// </summary>
        public class Directions : DrawConsoleComponent
        {
            private static readonly Color fadedWhite = new Color(255, 255, 255, 240);

            private static readonly Cell empty = new Cell(Color.Transparent, Color.Transparent);
            private static readonly Cell up = new Cell(fadedWhite, Color.Transparent, 30);
            private static readonly Cell right = new Cell(fadedWhite, Color.Transparent, 16);
            private static readonly Cell down = new Cell(fadedWhite, Color.Transparent, 31);
            private static readonly Cell left = new Cell(fadedWhite, Color.Transparent, 17);

            /// <summary>
            /// The Cells that make up the display of the 
            /// available directions.
            /// </summary>
            public static readonly Cell[] Cells = new Cell[]
               {
                   empty, up, empty,
                   left, empty, right,
                   empty, down, empty
               };

            /// <summary>
            /// Updates the cell display by changing the 
            /// visibility of the cells to indicate available movement.
            /// </summary>
            /// <param name="console">The Console to which the 
            /// Directions has been added as a component.</param>
            /// <param name="delta">The time that has passed
            /// since the last draw update.</param>
            public override void Draw(Console console, TimeSpan delta)
            {
                int x = World.Player.Column;
                int y = World.Player.Row;

                console.Position = new Point(x - 1, y - 1);

                console.SetForeground(1, 0, World.IsPassable(y - 1, x) ? fadedWhite : Color.Transparent);//.Cells[1].Glyph = 2;// Color.Transparent;
                console.SetForeground(2, 1, World.IsPassable(y, x + 1) ? fadedWhite : Color.Transparent);
                console.SetForeground(1, 2, World.IsPassable(y + 1, x) ? fadedWhite : Color.Transparent);
                console.SetForeground(0, 1, World.IsPassable(y, x - 1) ? fadedWhite : Color.Transparent);
            }
        }
    }
}
