using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    public partial class ItemUser : World.Character, ILineal
    {
        /// <summary>
        /// Defines Settings, the basic attributes of an ItemUser, 
        /// prior to instance-specific adjustments.
        /// </summary>
        protected class Settings : BaseSettings
        {
            /// <summary>
            /// Defines the default AI of an ItemUser.
            /// </summary>
            public override IAI AI => new BasicAI();

            /// <summary>
            /// Defines the color of an ItemUser's Glyphs.
            /// </summary>
            public override Color GlyphColor => Color.DarkSalmon;
            /// <summary>
            /// Defines the glyph to be shown when an ItemUser is in attack mode.
            /// </summary>
            public override int AttackGlyph => 231;//'τ'
            /// <suary>
            /// Defines the glyph to be shown when an ItemUser is in defend mode.
            /// </summary>
            public override int DefendGlyph => 232;//'φ'
            /// <suary>
            /// Defines the glyph to be shown when an ItemUser is in normal mode.
            /// </summary>
            public override int NormalGlyph => 227;//'π'

            /// <summary>
            /// The initial base attack damage of an ItemUser.
            /// </summary>
            public override float InitialAttackDamage => 1.67f;
            /// <summary>
            /// The initial base attack interval of an ItemUser.
            /// </summary>
            public override int InitialAttackInterval => 2000;
            /// <summary>
            /// The initial base maximum health of an ItemUser.
            /// </summary>
            public override float InitialBaseMaxHealth => 10.0f;
            /// <summary>
            /// The initial damage reduction, as a percentage, for an ItemUser in defend mode.
            /// </summary>
            public override float InitialDefenseDamageReduction => 0.1f;
            /// <summary>
            /// The initial movement interval of an ItemUser.
            /// </summary>
            public override int InitialMovementInterval => 500;
        }
    }
}
