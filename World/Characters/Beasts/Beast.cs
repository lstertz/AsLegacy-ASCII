using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Beast, which is a Character that cannot 
    /// use Items and has a number of nature-based skills and abilities.
    /// </summary>
    public class Beast : World.Character
    {
        /// <summary>
        /// Defines BaseSettings, the basic attributes of a Beast, 
        /// prior to instance-specific adjustments.
        /// </summary>
        protected new class BaseSettings : World.Character.BaseSettings
        {
            /// <summary>
            /// Defines the color of a Beast's Glyphs.
            /// </summary>
            public override Color GlyphColor => Color.DarkOrange;
            /// <summary>
            /// Defines the glyph to be shown when a Beast is in attack mode.
            /// </summary>
            public override int AttackGlyph => 229;//'σ'
            /// <summary>
            /// Defines the glyph to be shown when a Beast is in defend mode.
            /// </summary>
            public override int DefendGlyph => 239;//'∩'
            /// <summary>
            /// Defines the glyph to be shown when a Beast is in normal mode.
            /// </summary>
            public override int NormalGlyph => 224;//'α'

            /// <summary>
            /// The initial base attack damage of a Beast.
            /// </summary>
            public override float InitialAttackDamage => 1.67f;
            /// <summary>
            /// The initial base attack interval of a Beast.
            /// </summary>
            public override int InitialAttackInterval => 2000;
            /// <summary>
            /// The initial base maximum health of a Beast.
            /// </summary>
            public override float InitialBaseMaxHealth => 6.0f;
        }

        /// <summary>
        /// Constructs a new Beast at the provided row and column on the map.
        /// </summary>
        /// <param name="row">The row position of the new Beast.</param>
        /// <param name="column">The column position of the new Beast.</param>
        /// <param name="name">The name of the new Beast.</param>
        /// <param name="legacy">The starting legacy of the new Beast.</param>
        public Beast(int row, int column, string name, int legacy) : 
            base(row, column, name, new BaseSettings(), legacy)
        {
        }
    }
}
