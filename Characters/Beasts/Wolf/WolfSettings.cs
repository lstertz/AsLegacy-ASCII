using AsLegacy.Characters;
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
        /// Defines the settings of a Wolf.
        /// </summary>
        protected class WolfSettings : Settings
        {
            /// <inheritdoc/>
            public override Color GlyphColor => Color.BurlyWood;

            /// <inheritdoc/>
            public override Class.Type ClassType => Class.Type.Wolf;
            /// <inheritdoc/>
            public override Combat.Legacy InitialLegacy => new Combat.Legacy(8);
            /// <inheritdoc/>
            public override string Name => "Wolf";

            /// <inheritdoc/>
            public override int AttackGlyph => 228;//'Σ'
            /// <inheritdoc/>
            public override int DefendGlyph => 210;//'╥'
            /// <inheritdoc/>
            public override int NormalGlyph => 239;//'∩'

            /// <inheritdoc/>
            public override float InitialAttackDamage => 1.5f;
            /// <inheritdoc/>
            public override int InitialAttackInterval => 1500;
            /// <inheritdoc/>
            public override int InitialAttackMovementInterval => 600;
            /// <inheritdoc/>
            public override float InitialBaseMaxHealth => 7.0f;
            /// <inheritdoc/>
            public override float InitialDefenseDamageReduction => 0.15f;
            /// <inheritdoc/>
            public override int InitialNormalMovementInterval => 400;
        }
    }
}
