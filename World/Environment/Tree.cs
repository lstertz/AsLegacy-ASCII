using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines a Path, which is an Environment Element that represents a 
        /// part of a path, as a Tile, on the World map.
        /// </summary>
        public class Tree : EnvironmentElement
        {
            /// <summary>
            /// Constructs a new Tree.
            /// </summary>
            public Tree() :  // TODO :: Randomize the color and glyph.
                base(Color.Black, Color.White, 'T', true)
            { }

            /// <summary>
            /// Constructs a new Tree, to replace an existing Environment Element.
            /// </summary>
            /// <param name="row">The row position of the new Tree, 
            /// to be used to replace any existing Environment Element.</param>
            /// <param name="column">The column position of the new Tree, 
            /// to be used to replace any existing Environment Element.</param>
            public Tree(int row, int column) : this()
            {
                if (environment != null)
                    environment.ReplaceWith(row, column, this);
            }
        }
    }
}
