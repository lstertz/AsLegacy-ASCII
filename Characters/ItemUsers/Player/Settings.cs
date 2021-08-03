using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    public partial class Player : ItemUser
    {
        /// <summary>
        /// Defines Settings, the basic attributes of the Player, 
        /// prior to instance-specific adjustments.
        /// </summary>
        protected new class Settings : ItemUser.Settings
        {
            /// <summary>
            /// Defines the AI of the Player.
            /// </summary>
            public override IAI AI => new AI();

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

            /// <summary>
            /// Constructs a new set of settings.
            /// </summary>
            /// <param name="initialPassiveInvestments">
            /// <see cref="InitialPassiveInvestments"/></param>
            public Settings(int[] initialPassiveInvestments = null) : 
                base(initialPassiveInvestments)
            { }
        }
    }
}
