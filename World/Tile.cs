using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines a Tile to represent an entity within the World.
        /// </summary>
        public abstract class Tile
        {
            /// <summary>
            /// The glyph to visually represent the entity of the Tile.
            /// </summary>
            public virtual int Glyph
            {
                get => glyph;
                protected set => glyph = value;
            }
            private int glyph;

            /// <summary>
            /// The color of the glyph to visually represent the entity of the Tile.
            /// </summary>
            public virtual Color GlyphColor
            {
                get => glyphColor;
                protected set => glyphColor = value;
            }
            private Color glyphColor;

            /// <summary>
            /// The color of the area behind the glyph.
            /// </summary>
            public virtual Color Background
            {
                get => background;
                protected set => background = value;
            }
            private Color background;

            /// <summary>
            /// Specifies whether this Tile's entity can be passed through by others.
            /// </summary>
            public bool Passable { get; private set; }

            /// <summary>
            /// Constructs a new Tile.
            /// </summary>
            /// <param name="background">The color of the area behind the glyph.</param>
            /// <param name="glyphColor">The color of the glyph.</param>
            /// <param name="glyph">The glyph to visually represent the entity 
            /// of the new Tile.</param>
            /// <param name="passable">Specifies whether the new Tile's entity can be 
            /// passed through by others.</param>
            public Tile(Color background, Color glyphColor, int glyph, bool passable)
            {
                Background = background;
                GlyphColor = glyphColor;
                Glyph = glyph;
                Passable = passable;
            }
        }
    }
}
