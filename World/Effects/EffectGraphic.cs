using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public partial class World
    {
        /// <summary>
        /// Defines a <see cref="Tile"/> to display the graphical results of 
        /// an <see cref="Effect"/>.
        /// </summary>
        public class EffectGraphic : Tile
        {
            /// <summary>
            /// Constructs a new <see cref="EffectGraphic"/>.
            /// </summary>
            /// <param name="glyphColor">The color of the graphic.</param>
            /// <param name="glyph">The graphc, defined as an identified glyph.</param>
            public EffectGraphic(Color glyphColor, int glyph) : 
                base(Color.Transparent, glyphColor, glyph, true) { }
        }
    }
}
