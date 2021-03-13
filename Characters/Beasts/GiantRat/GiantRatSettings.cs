using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Beast, which is a Character that cannot 
    /// use Items and has a number of nature-based skills and abilities.
    /// </summary>
    public partial class Beast : World.Character
    {
        /// <summary>
        /// Defines the settings of a Giant Rat.
        /// </summary>
        protected class GiantRatSettings : Settings
        {
            /// <inheritdoc/>
            public override Color GlyphColor => Color.LightPink;
            /// <inheritdoc/>
            public override Combat.Legacy InitialLegacy => new Combat.Legacy(4);
            /// <inheritdoc/>
            public override string Name => "Giant Rat";

            /// <inheritdoc/>
            public override int AttackGlyph => 213;//'╒'
            /// <inheritdoc/>
            public override int DefendGlyph => 214;//'╓'
            /// <inheritdoc/>
            public override int NormalGlyph => 114;//'r'

            /// <inheritdoc/>
            public override float InitialAttackDamage => 1f;
            /// <inheritdoc/>
            public override int InitialAttackInterval => 2000;
            /// <inheritdoc/>
            public override float InitialBaseMaxHealth => 4.0f;
            /// <inheritdoc/>
            public override float InitialDefenseDamageReduction => 0.1f;
            /// <inheritdoc/>
            public override int InitialMovementInterval => 500;
        }
    }
}
