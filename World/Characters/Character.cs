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
            protected readonly Color highlightedGlyphColor = Color.White;

            /// <summary>
            /// The glyph to visually represent the entity of the Character.
            /// </summary>
            public override int Glyph 
            { 
                get => base.Glyph;
                protected set
                {
                    base.Glyph = value;
                    characters?.GetDisplay()?.Update(Row, Column);
                }
            }

            /// <summary>
            /// The color of the glyph to visually represent the entity of the Tile.
            /// </summary>
            public override Color GlyphColor
            {
                get => base.GlyphColor;
                protected set
                {
                    base.GlyphColor = value;
                    characters?.GetDisplay()?.Update(Row, Column);
                }
            }
            private Color originalGlyphColor;

            /// <summary>
            /// The Column (x-axis) location of the Character.
            /// </summary>
            public int Column { get; private set; }

            /// <summary>
            /// The Row (y-axis) location of the Character.
            /// </summary>
            public int Row { get; private set; }

            /// <summary>
            /// Specifies whether the Character is highlighted, generally for 
            /// some kind of interaction or as being targeted. Highlighting a Character 
            /// changes its glyph's color.
            /// </summary>
            public bool Highlighted 
            {
                get => highlighted;
                set
                {
                    if (highlighted == value)
                        return;

                    highlighted = value;
                    GlyphColor = value ? highlightedGlyphColor : originalGlyphColor;
                }
            }
            private bool highlighted;

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
                originalGlyphColor = glyphColor;
                Column = column;
                Row = row;

                if (characters != null)
                    characters.ReplaceWith(row, column, this);
            }

            /// <summary>
            /// Moves the specificed Character to a new Row and Column location.
            /// This technically swaps the Character with the Character presently at 
            /// the new Row and Column.
            /// </summary>
            /// <param name="c">The Character to be moved.</param>
            /// <param name="newRow">The Row that the Character will be moved to.</param>
            /// <param name="newColumn">The Column that the Character will be moved to.</param>
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
