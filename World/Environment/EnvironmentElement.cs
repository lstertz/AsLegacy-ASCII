using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines an Environment Element, which is an entity that represents an 
        /// environmental construct or concept, as a Tile, on the World map.
        /// </summary>
        public abstract class EnvironmentElement : Tile
        {
            /// <summary>
            /// Constructs a new Environment Element.
            /// </summary>
            /// <param name="background">The color of the area behind the glyph.</param>
            /// <param name="glyphColor">The color of the glyph visually displayed to represent 
            /// the new Environment Tile.</param>
            /// <param name="glyph">The glyph visually displayed to represent 
            /// the new Environment Element.</param>
            /// <param name="passable">Whether the new Environment Element can be 
            /// passed through by others.</param>
            protected EnvironmentElement(
                Color background, Color glyphColor, int glyph, bool passable) :
                base(background, glyphColor, glyph, passable)
            { }

            /// <summary>
            /// Constructs a new Environment Element, to replace an existing Environment Element.
            /// </summary>
            /// <param name="row">The row position of the new Environment Element, 
            /// to be used to replace any existing Environment Element.</param>
            /// <param name="column">The column position of the new Environment Element, 
            /// to be used to replace any existing Environment Element.</param>
            /// <param name="background">The color of the area behind the glyph.</param>
            /// <param name="glyphColor">The color of the glyph visually displayed to represent 
            /// the new Environment Tile.</param>
            /// <param name="glyph">The glyph visually displayed to represent 
            /// the new Environment Element.</param>
            /// <param name="passable">Whether the new Environment Element can be 
            /// passed through by others.</param>
            protected EnvironmentElement(int row, int column, 
                Color background, Color glyphColor, int glyph, bool passable) :
                base(background, glyphColor, glyph, passable)
            {
                if (Environment != null)
                    Environment.ReplaceWith(row, column, this);
            }
        }
    }
}
