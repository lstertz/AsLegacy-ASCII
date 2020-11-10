using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        public abstract class Character : Tile
        {
            public int Column { get { return col; } }
            protected int col;

            public int Row { get { return row; } }
            protected int row;

            protected Character(int col, int row, 
                Color background, Color glyphColor, int glyph, bool passable) :
                base(background, glyphColor, glyph, passable)
            {
                this.col = col;
                this.row = row;

                characters[row * columnCount + col] = this;
            }

            protected Character(Character original, 
                Color background, Color glyphColor, int glyph, bool passable) :
                base(original, passable)
            {
                col = original.col;
                row = original.row;

                Glyph = glyph;
                GlyphColor = glyphColor;
                Background = background;

                characters[row * columnCount + col] = this;
            }
        }
    }
}
