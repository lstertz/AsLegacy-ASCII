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
        /// Defines the PlayerCommands aspect of a Display, 
        /// which is responsible for updating the directions available for Player movement.
        /// </summary>
        public class PlayerCommands : DrawConsoleComponent
        {
            private static readonly Color fadedWhite = new Color(255, 255, 255, 235);

            private static readonly Cell empty = new Cell(Color.Transparent, Color.Transparent);
            private static readonly Cell center = new Cell(Color.Transparent, Color.Transparent);
            private static readonly Cell up = new Cell(fadedWhite, Color.Transparent, 30);
            private static readonly Cell right = new Cell(fadedWhite, Color.Transparent, 16);
            private static readonly Cell down = new Cell(fadedWhite, Color.Transparent, 31);
            private static readonly Cell left = new Cell(fadedWhite, Color.Transparent, 17);

            /// <summary>
            /// The Cells that make up the display of the available directions.
            /// </summary>
            public static readonly Cell[] Cells = new Cell[]
            {
                empty, up, empty,
                left, center, right,
                empty, down, empty
            };

            private int highlightCellX = -1;
            private int highlightCellY = -1;
            
            /// <summary>
            /// Specifies which PlayerCommands Display Cell should be highlighted, which 
            /// influences the foreground color of the Cell. A location outside of the 
            /// PlayerCommands Console (e.g. (-1, -1)) indicates that no Cell should be highlighted.
            /// </summary>
            /// <param name="x">The local x of the Cell.</param>
            /// <param name="y">The local y of the Cell.</param>
            public void SetCellToHighlight(int x, int y)
            {
                highlightCellX = x;
                highlightCellY = y;
            }

            /// <summary>
            /// Updates the cell display by changing the 
            /// visibility of the cells to indicate available movement.
            /// </summary>
            /// <param name="console">The Console to which the 
            /// PlayerCommands has been added as a component.</param>
            /// <param name="delta">The time that has passed
            /// since the last draw update.</param>
            public override void Draw(Console console, TimeSpan delta)
            {
                int x = World.Player.Column;
                int y = World.Player.Row;

                int centerX = MapViewPortHalfWidth;
                int centerY = MapViewPortHalfHeight;

                if (display.MapViewPort.Left == 0)
                    centerX = World.Player.Column;
                else if (display.MapViewPort.Right == World.ColumnCount)
                    centerX = World.Player.Column - display.characters.ViewPort.Left;
                if (display.characters.ViewPort.Top == 0)
                    centerY = World.Player.Row;
                else if (display.characters.ViewPort.Bottom == World.RowCount)
                    centerY = World.Player.Row - display.characters.ViewPort.Top;

                console.Position = new Point(centerX - 1, centerY - 1);

                console.SetForeground(1, 0, GetCellColor(1, 0, y - 1, x));
                console.SetForeground(2, 1, GetCellColor(2, 1, y, x + 1));
                console.SetForeground(1, 2, GetCellColor(1, 2, y + 1, x));
                console.SetForeground(0, 1, GetCellColor(0, 1, y, x - 1));
            }

            /// <summary>
            /// Determines the foreground color of a PlayerCommands Display Cell for the 
            /// specified local and global location.
            /// </summary>
            /// <param name="x">The local x location of the Cell.</param>
            /// <param name="y">The local y location of the Cell.</param>
            /// <param name="worldX">The global x location of the Cell.</param>
            /// <param name="worldY">The global y location of the Cell.</param>
            /// <returns></returns>
            private Color GetCellColor(int x, int y, int worldX, int worldY)
            {
                if (!World.IsPassable(worldX, worldY))
                    return Color.Transparent;

                if (highlightCellX == x && highlightCellY == y)
                    return Color.White;

                return fadedWhite;
            }
        }
    }
}
