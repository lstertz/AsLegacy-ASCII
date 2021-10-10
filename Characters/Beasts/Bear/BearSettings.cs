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
        /// Defines the settings of a Bear.
        /// </summary>
        protected class BearSettings : Settings
        {
            /// <inheritdoc/>
            public override Color GlyphColor => Color.DarkOrange;

            /// <inheritdoc/>
            public override Class.Type ClassType => Class.Type.Bear;
            /// <inheritdoc/>
            public override Combat.Legacy InitialLegacy => new Combat.Legacy(12);
            /// <inheritdoc/>
            public override string Name => "Bear";
            
            /// <inheritdoc/>
            public override int AttackGlyph => 226;//'Γ'
            /// <inheritdoc/>
            public override int DefendGlyph => 66;//'B'
            /// <inheritdoc/>
            public override int NormalGlyph => 225;//'ß'

            /// <inheritdoc/>
            public override float InitialAttackDamage => 4f;
            /// <inheritdoc/>
            public override int InitialAttackInterval => 3000;
            /// <inheritdoc/>
            public override int InitialAttackMovementInterval => 600;
            /// <inheritdoc/>
            public override float InitialBaseMaxHealth => 10f;
            /// <inheritdoc/>
            public override float InitialDefenseDamageReduction => 0.2f;
            /// <inheritdoc/>
            public override int InitialNormalMovementInterval => 600;
        }
    }
}
