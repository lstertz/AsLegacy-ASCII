using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines a Tree, which is an Environment Element that represents a 
        /// tree, as a Tile, on the World map.
        /// </summary>
        public class Path : EnvironmentElement
        {
            /// <summary>
            /// Constructs a new Path.
            /// </summary>
            public Path() :
                base(Color.Black, Color.SandyBrown, 176, true)
            {
            }

            /// <summary>
            /// Constructs a new Path, to replace an existing Environment Element.
            /// </summary>
            /// <param name="row">The row position of the new Path, 
            /// to be used to replace any existing Environment Element.</param>
            /// <param name="column">The column position of the new Path, 
            /// to be used to replace any existing Environment Element.</param>
            public Path(int row, int column) : this()
            {
                if (environment != null)
                    environment.ReplaceWith(row, column, this);
            }
        }
    }
}