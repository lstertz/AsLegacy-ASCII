using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Path, which is an Environment Element that represents a 
    /// path, as a Tile, on the World map.
    /// </summary>
    public class Path : World.EnvironmentElement
    {
        /// <summary>
        /// Constructs a new Path.
        /// </summary>
        public Path() : base(Color.Black, Color.SandyBrown, 176, true)
        { }

        /// <summary>
        /// Constructs a new Path, to replace an existing Environment Element.
        /// </summary>
        /// <param name="row">The row position of the new Path, 
        /// to be used to replace any existing Environment Element.</param>
        /// <param name="column">The column position of the new Path, 
        /// to be used to replace any existing Environment Element.</param>
        public Path(int row, int column) :
            base(row, column, Color.Black, Color.SandyBrown, 176, true)
        { }
    }
}