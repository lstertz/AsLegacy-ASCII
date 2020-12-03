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
        /// Defines the Commands aspect of a Display, 
        /// which is responsible for updating the directions available for Player movement.
        /// </summary>
        public class Commands : Console
        {
            private static readonly Color fadedWhite = new Color(255, 255, 255, 235);

            private static readonly Cell empty = new Cell(Color.Transparent, Color.Transparent);
            private static readonly Cell up = new Cell(fadedWhite, Color.Transparent, 30);
            private static readonly Cell right = new Cell(fadedWhite, Color.Transparent, 16);
            private static readonly Cell down = new Cell(fadedWhite, Color.Transparent, 31);
            private static readonly Cell left = new Cell(fadedWhite, Color.Transparent, 17);

            /// <summary>
            /// The Cells that make up the display of the available directions.
            /// </summary>
            private static readonly Cell[] cells = new Cell[]
            {
                empty, up, empty,
                left, empty, right,
                empty, down, empty
            };

            private World.PresentCharacter focus;
            
            private int highlightCellX = -1;
            private int highlightCellY = -1;

            public Commands(World.PresentCharacter focus) : base(3, 3, cells)
            {
                this.focus = focus;
            }
            
            /// <summary>
            /// Specifies which Commands Display Cell should be highlighted, which 
            /// influences the foreground color of the Cell. A location outside of the 
            /// Commands Console (e.g. (-1, -1)) indicates that no Cell should be highlighted.
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
            /// <param name="delta">The time that has passed since the last draw update.</param>
            public override void Draw(TimeSpan delta)
            {
                base.Draw(delta);

                int x = focus.Column;
                int y = focus.Row;

                int centerX = MapViewPortHalfWidth;
                int centerY = MapViewPortHalfHeight;
                Rectangle viewPort = MapViewPort;

                if (viewPort.Left == 0)
                    centerX = x;
                else if (viewPort.Right == World.ColumnCount)
                    centerX = x - viewPort.Left;
                if (viewPort.Top == 0)
                    centerY = y;
                else if (viewPort.Bottom == World.RowCount)
                    centerY = y - viewPort.Top;

                Position = new Point(centerX - 1, centerY - 1);

                SetForeground(1, 0, GetDirectionColor(1, 0, y - 1, x));
                SetForeground(2, 1, GetDirectionColor(2, 1, y, x + 1));
                SetForeground(1, 2, GetDirectionColor(1, 2, y + 1, x));
                SetForeground(0, 1, GetDirectionColor(0, 1, y, x - 1));
            }

            /// <summary>
            /// Determines the foreground color of a Commands Display Cell for the 
            /// specified local and global location.
            /// </summary>
            /// <param name="x">The local x location of the Cell.</param>
            /// <param name="y">The local y location of the Cell.</param>
            /// <param name="worldX">The global x location of the Cell.</param>
            /// <param name="worldY">The global y location of the Cell.</param>
            /// <returns>The Color for the glyph of the located Cell.</returns>
            private Color GetDirectionColor(int x, int y, int worldX, int worldY)
            {
                if (!World.IsPassable(worldX, worldY))
                    return Color.Transparent;

                if (focus.ActiveMode != World.PresentCharacter.Mode.Normal)
                    return Color.Transparent;

                if (highlightCellX == x && highlightCellY == y)
                    return Color.White;

                return fadedWhite;
            }
        }
    }
}
