using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines a Character, which is an entity that is expected to 
        /// manipulate the environment and other characters from its specific 
        /// position, as a Tile, on the map.
        /// </summary>
        public abstract class Character : Tile
        {
            public int Column { get; private set; }
            public int Row { get; private set; }

            /// <summary>
            /// Constructs a new Character.
            /// </summary>
            /// <param name="row">The row position of the new Character.</param>
            /// <param name="column">The column position of the new Character.</param>
            /// <param name="background">The color of the area behind the glyph.</param>
            /// <param name="glyphColor">The color of the glyph visually displayed to represent 
            /// the new Character.</param>
            /// <param name="glyph">The glyph visually displayed to represent 
            /// the new Character.</param>
            /// <param name="passable">Whether the new Character can be 
            /// passed through by others.</param>
            protected Character(int row, int column, 
                Color background, Color glyphColor, int glyph, bool passable) :
                base(background, glyphColor, glyph, passable)
            {
                Column = column;
                Row = row;

                if (characters != null)
                    characters.ReplaceWith(row, column, this);
            }

            protected void Move(Character c, int newRow, int newColumn)
            {
                Character swapped = characters.ReplaceWith(newRow, newColumn, c);
                characters.ReplaceWith(c.Row, c.Column, swapped);

                swapped.Row = c.Row;
                swapped.Column = c.Column;

                c.Row = newRow;
                c.Column = newColumn;
            }
        }
    }
}
