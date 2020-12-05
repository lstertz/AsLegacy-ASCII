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
            /// The currently highlighted Tile, or null, or no Tile is currently highlighted.
            /// </summary>
            private Tile highlightedTile = null;

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
            /// Specifies whether the Tile is highlighted, generally for 
            /// some kind of anticipated interaction.
            /// Only one Tile can be highlighted at a time.
            /// </summary>
            public virtual bool Highlighted
            {
                get => highlighted;
                set
                {
                    if (value && highlightedTile == this)
                        return;

                    if (highlightedTile != null)
                    {
                        highlightedTile.Highlighted = false;
                        highlightedTile = null;
                    }

                    if (value)
                        highlightedTile = this;
                    highlighted = value;
                }
            }
            private bool highlighted;

            /// <summary>
            /// Specifies whether the Tile is selected.
            /// </summary>
            public virtual bool Selected
            {
                get => selected;
                protected set => selected = value;
            }
            private bool selected;

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
