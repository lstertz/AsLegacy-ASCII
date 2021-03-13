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
            protected static Tile HighlightedTile { get; private set; }

            /// <summary>
            /// The glyph to visually represent the entity of the Tile.
            /// </summary>
            public virtual int Glyph
            {
                get => _glyph;
                protected set => _glyph = value;
            }
            private int _glyph;

            /// <summary>
            /// The color of the glyph to visually represent the entity of the Tile.
            /// </summary>
            public virtual Color GlyphColor
            {
                get => _glyphColor;
                protected set => _glyphColor = value;
            }
            private Color _glyphColor;

            /// <summary>
            /// The color of the area behind the glyph.
            /// </summary>
            public virtual Color Background
            {
                get => _background;
                protected set => _background = value;
            }
            private Color _background;

            /// <summary>
            /// Specifies whether the Tile is highlighted, generally for 
            /// some kind of anticipated interaction.
            /// Only one Tile can be highlighted at a time.
            /// </summary>
            public virtual bool Highlighted
            {
                get => _highlighted;
                set
                {
                    if (value == _highlighted)
                        return;
                    _highlighted = value;

                    if (HighlightedTile != this && HighlightedTile != null)
                    {
                        HighlightedTile.Highlighted = false;
                        HighlightedTile = null;
                    }

                    if (value)
                        HighlightedTile = this;
                }
            }
            private bool _highlighted;

            /// <summary>
            /// Specifies whether the Tile is selected.
            /// </summary>
            public virtual bool Selected
            {
                get => _selected;
                protected set => _selected = value;
            }
            private bool _selected;

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
            protected Tile(Color background, Color glyphColor, int glyph, bool passable)
            {
                Background = background;
                GlyphColor = glyphColor;
                Glyph = glyph;
                Passable = passable;
            }
        }
    }
}
