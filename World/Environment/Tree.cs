using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Path, which is an Environment Element that represents a 
    /// part of a path, as a Tile, on the World map.
    /// </summary>
    public class Tree : World.EnvironmentElement
    {
        private static int typeValue = 0;

        /// <summary>
        /// Provides the glyph for a tree based on a pseudo-random 
        /// generation using primes with the increasing typeValue.
        /// </summary>
        /// <returns>The pseudo-random glyph, either 5 or 6.</returns>
        private static int GetTreeGlyph()
        {
            return (typeValue += 31) % 13 > 7 ? 5 : 6;
        }

        /// <summary>
        /// Constructs a new Tree.
        /// </summary>
        public Tree() : 
            base(Color.Black, Color.ForestGreen, GetTreeGlyph(), true)
        { }

        /// <summary>
        /// Constructs a new Tree, to replace an existing Environment Element.
        /// </summary>
        /// <param name="row">The row position of the new Tree, 
        /// to be used to replace any existing Environment Element.</param>
        /// <param name="column">The column position of the new Tree, 
        /// to be used to replace any existing Environment Element.</param>
        public Tree(int row, int column) : 
            base(row, column, Color.Black, Color.ForestGreen, GetTreeGlyph(), true)
        {
        }
    }
}
