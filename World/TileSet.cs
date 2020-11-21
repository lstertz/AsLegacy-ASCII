using SadConsole;
using System;
using Console = SadConsole.Console;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines a TileSet, which represents a collection of the specified type of Tiles.
        /// </summary>
        /// <typeparam name="T">The type of Tiles that will belong to the TileSet.</typeparam>
        public class TileSet<T> where T : Tile
        {
            /// <summary>
            /// Defines the Display of a TileSet, which encapsulates any structure 
            /// that is used to render the Tiles of the TileSet.
            /// </summary>
            public class Display
            {
                /// <summary>
                /// Implicitly converts a Display to an array of SadConsole Cells.
                /// </summary>
                /// <param name="display">The Display to be converted.</param>
                public static implicit operator Cell[](Display display)
                {
                    return display.cells;
                }

                private Cell[] cells;
                private Console console;
                private TileSet<T> tileSet;

                /// <summary>
                /// Constructs a new Display for the provided TileSet.
                /// </summary>
                /// <param name="tileSet">The TileSet that defines the underlying 
                /// data for the Display.</param>
                public Display(TileSet<T> tileSet)
                {
                    cells = new Cell[rowCount * columnCount];
                    for (int c = 0, count = rowCount * columnCount; c < count; c++)
                        cells[c] = new Cell();

                    this.tileSet = tileSet;
                }

                public void Bind(Console console)
                {
                    this.console = console;
                }

                /// <summary>
                /// Updates the Display's rendered data structure for the Tile at 
                /// the given row and column position.
                /// </summary>
                /// <param name="row">The row of the Tile whose display representation 
                /// is to be updated.</param>
                /// <param name="column">The column of the Tile whose display representation 
                /// is to be updated.</param>
                public void Update(int row, int column)
                {
                    Cell cell = cells[row * columnCount + column];
                    T tile = tileSet.tiles[row, column];

                    if (console != null)
                    {
                        console.SetForeground(column, row, tile.GlyphColor);
                        console.SetBackground(column, row, tile.Background);
                        console.SetGlyph(column, row, tile.Glyph);
                    }
                    else
                    {
                        cell.Foreground = tile.GlyphColor;
                        cell.Background = tile.Background;
                        cell.Glyph = tile.Glyph;
                    }
                }
            }

            private Display display;
            private T[,] tiles;

            /// <summary>
            /// Constructs a new TileSet, with Tiles created through the 
            /// given tile constructor.
            /// </summary>
            /// <param name="tileConstructor">A Func, used as a constructor 
            /// of the new TileSet's Tiles.</param>
            public TileSet(Func<int, int, T> tileConstructor)
            {
                display = new Display(this);
                tiles = new T[rowCount, columnCount];

                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < columnCount; col++)
                    {
                        tiles[row, col] = tileConstructor(row, col);
                        display.Update(row, col);
                    }
                }
            }

            public T Get(int row, int column)
            {
                if (row < 0 || column < 0 || rowCount <= row || columnCount <= column)
                    return null;
                return tiles[row, column];
            }

            /// <summary>
            /// Specifies whether the tile at the provided position is passable.
            /// </summary>
            /// <param name="row">The row of the tile.</param>
            /// <param name="column">The column of the tile.</param>
            /// <returns>True if the tile exists and is passable, false otherwise.</returns>
            public bool IsPassable(int row, int column)
            {
                if (row < 0 || column < 0 || rowCount <= row || columnCount <= column)
                    return false;
                return tiles[row, column].Passable;
            }

            /// <summary>
            /// Replaces the current Tile at the specified row and column 
            /// with the provided new Tile.
            /// </summary>
            /// <param name="row">The row of the Tile to be replaced.</param>
            /// <param name="column">The column of the Tile to be replaced.</param>
            /// <param name="newTile">The Tile replacing the original Tile.</param>
            /// <returns>The replaced Tile.</returns>
            public T ReplaceWith(int row, int column, T newTile)
            {
                T replaced = tiles[row, column];

                tiles[row, column] = newTile;
                display.Update(row, column);

                return replaced;
            }

            /// <summary>
            /// Provides the Display of this TileSet.
            /// </summary>
            /// <returns>The Display of this TileSet.</returns>
            public Display GetDisplay()
            {
                return display;
            }
        }
    }
}
