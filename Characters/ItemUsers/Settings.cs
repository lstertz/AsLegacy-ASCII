﻿using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    public partial class ItemUser
    {
        /// <summary>
        /// Defines Settings, the basic attributes of an ItemUser, 
        /// prior to instance-specific adjustments.
        /// </summary>
        protected class Settings : BaseSettings
        {
            /// <inheritdoc/>
            public override IAI AI => new BasicAI();

            /// <inheritdoc/>
            public override Color GlyphColor => Color.DarkSalmon;
            /// <inheritdoc/>
            public override int AttackGlyph => 231;//'τ'
            /// <inheritdoc/>
            public override int DefendGlyph => 232;//'φ'
            /// <inheritdoc/>
            public override int NormalGlyph => 227;//'π'

            /// <inheritdoc/>
            public override float InitialAttackDamage => 1.67f;
            /// <inheritdoc/>
            public override int InitialAttackInterval => 2000;
            /// <inheritdoc/>
            public override int InitialAttackMovementInterval => 1000;
            /// <inheritdoc/>
            public override float InitialBaseMaxHealth => 10.0f;
            /// <inheritdoc/>
            public override float InitialDefenseDamageReduction => 0.1f;
            /// <inheritdoc/>
            public override int InitialNormalMovementInterval => 500;
        }
    }
}
