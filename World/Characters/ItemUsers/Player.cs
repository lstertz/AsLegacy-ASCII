using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Player, the focal Character, on the World map, of the real-life player as 
    /// they interact with the game.
    /// </summary>
    public class Player : ItemUser
    {
        /// <summary>
        /// Defines BaseSettings, the basic attributes of the Player, 
        /// prior to instance-specific adjustments.
        /// </summary>
        protected new class BaseSettings : ItemUser.BaseSettings
        {
            /// <summary>
            /// Defines the color of the Player's Glyphs.
            /// </summary>
            public override Color GlyphColor => Color.Goldenrod;
            /// <summary>
            /// Defines the glyph to be shown when the Player is in attack mode.
            /// </summary>
            public override int AttackGlyph => 229;//'δ'
            /// <suary>
            /// Defines the glyph to be shown when the Player is in defend mode.
            /// </summary>
            public override int DefendGlyph => 233;//'Θ'
            /// <suary>
            /// Defines the glyph to be shown when the Player is in normal mode.
            /// </summary>
            public override int NormalGlyph => 64;//'@'
        }

        /// <summary>
        /// Specifies the target of the Player.
        /// The target will be selected, and will be the recipient of 
        /// certain actions performed by the Player.
        /// </summary>
        public override World.Character Target
        {
            get { return target; }
            set
            {
                UpdateSelected(target, value);
                base.Target = value;
            }
        }


        /// <summary>
        /// Constructs a new Player at the provided row and column on the map.
        /// </summary>
        /// <param name="row">The row position of the new Player.</param>
        /// <param name="column">The column position of the new Player.</param>
        /// <param name="name">The name of the current Player Character.</param>
        /// <param name="lineageName">The name of the Player's Lineage.</param>
        public Player(int row, int column, string name, string lineageName) :
            base(row, column, name, new BaseSettings(), 0, lineageName)
        {

        }
    }
}
