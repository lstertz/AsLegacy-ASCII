using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Player, the focal character, on the World map, of the real-life player as 
    /// they interact with the game.
    /// </summary>
    public class Player : World.PresentCharacter
    {
        private const int normal = '@';

        /// <summary>
        /// Defines the glyph to be shown when the Player is in attack mode.
        /// </summary>
        protected override int attackGlyph => 235;

        /// <summary>
        /// Defines the glyph to be shown when the Player is in defend mode.
        /// </summary>
        protected override int defendGlyph => 233;

        /// <summary>
        /// Defines the glyph to be shown when the Player is in normal mode.
        /// </summary>
        protected override int normalGlyph => normal;

        /// <summary>
        /// Provides the point (global location) of this Player.
        /// </summary>
        public Point Point => new Point(Column, Row);


        /// <summary>
        /// Constructs a new Player at the provided row and column on the map.
        /// </summary>
        /// <param name="row">The row position of the new Player.</param>
        /// <param name="column">The column position of the new Player.</param>
        public Player(int row, int column) :
            base(row, column, Color.Goldenrod, normal)
        {

        }
    }
}
