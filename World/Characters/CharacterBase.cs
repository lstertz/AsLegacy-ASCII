using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines a CharacterBase, which is an entity that is expected to 
        /// manipulate the environment and other characters from its specific 
        /// position, as a Tile, on the map.
        /// </summary>
        public abstract class CharacterBase : Tile
        {
            /// <summary>
            /// The glyph to visually represent the entity of the CharacterBase.
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
            /// <summary>
            /// Specifies whether the CharacterBase is highlighted, generally for 
            /// some kind of anticipated interaction.
            /// Highlighting a CharacterBase changes its glyph's color.
            /// Only one Tile can be highlighted at a time.
            /// </summary>
            public override bool Highlighted 
            { 
                get => base.Highlighted;
                set
                {
                    base.Highlighted = value;
                    characters?.GetDisplay()?.Update(Row, Column);
                }
            }

            /// <summary>
            /// Specifies whether the CharacterBase is selected, generally for being targeted. 
            /// Selecting a CharacterBase changes its glyph's color.
            /// </summary>
            public override bool Selected
            {
                get => base.Selected;
                protected set
                {
                    base.Selected = value;
                    characters?.GetDisplay()?.Update(Row, Column);
                }
            }

            /// <summary>
            /// The Column (x-axis) location of the CharacterBase.
            /// </summary>
            public int Column { get; private set; }

            /// <summary>
            /// The Row (y-axis) location of the CharacterBase.
            /// </summary>
            public int Row { get; private set; }

            /// <summary>
            /// Constructs a new CharacterBase.
            /// </summary>
            /// <param name="row">The row position of the new CharacterBase.</param>
            /// <param name="column">The column position of the new CharacterBase.</param>
            /// <param name="background">The color of the area behind the glyph.</param>
            /// <param name="glyphColor">The color of the glyph visually displayed to represent 
            /// the new CharacterBase.</param>
            /// <param name="glyph">The glyph visually displayed to represent 
            /// the new CharacterBase.</param>
            /// <param name="passable">Whether the new CharacterBase can be 
            /// passed through by others.</param>
            protected CharacterBase(int row, int column, 
                Color background, Color glyphColor, int glyph, bool passable) :
                base(background, glyphColor, glyph, passable)
            {
                Column = column;
                Row = row;

                if (characters != null)
                    characters.ReplaceWith(row, column, this);
            }

            /// <summary>
            /// Moves the specificed CharacterBase to a new Row and Column location.
            /// This technically swaps the CharacterBase with the CharacterBase presently at 
            /// the new Row and Column.
            /// </summary>
            /// <param name="c">The CharacterBase to be moved.</param>
            /// <param name="newRow">The Row that the CharacterBase will be moved to.</param>
            /// <param name="newColumn">The Column that the CharacterBase will be moved to.</param>
            protected void Move(CharacterBase c, int newRow, int newColumn)
            {
                CharacterBase swapped = characters.ReplaceWith(newRow, newColumn, c);
                characters.ReplaceWith(c.Row, c.Column, swapped);

                swapped.Row = c.Row;
                swapped.Column = c.Column;

                c.Row = newRow;
                c.Column = newColumn;
            }

            /// <summary>
            /// Properly updates the selected Character by deselecting the 
            /// specified last selected Character.
            /// </summary>
            /// <param name="lastSelected">The last Character known to have been selected.</param>
            /// <param name="toBeSelected">The new Character to be selected.</param>
            protected void UpdateSelected(CharacterBase lastSelected,
                CharacterBase toBeSelected)
            {
                if (lastSelected != null)
                    lastSelected.Selected = false;

                if (toBeSelected != null)
                    toBeSelected.Selected = true;
            }
        }
    }
}
