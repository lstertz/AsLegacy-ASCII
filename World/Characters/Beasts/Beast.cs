using Microsoft.Xna.Framework;
using System;

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
        /// The Type of the Beast, which may define its name, initial legacy, 
        /// appearance, and base stats.
        /// </summary>
        public enum Type
        {
            GiantRat = 0,
            Wolf = 1,
            Bear = 2
        }

        /// <summary>
        /// Provides a random Beast Type.
        /// </summary>
        /// <returns>A random Beast Type.</returns>
        public static Type GetRandomType()
        {
            Random r = new Random();
            return (Type) r.Next(0, 3);
        }

        private static readonly string[] names = new string[] { "Giant Rat", "Wolf", "Bear" };
        private static readonly int[] initialLegacies = new int[] { 4, 8, 12 };

        /// <summary>
        /// Constructs a new Beast at the provided row and column on the map.
        /// </summary>
        /// <param name="row">The row position of the new Beast.</param>
        /// <param name="column">The column position of the new Beast.</param>
        /// <param name="name">The name of the new Beast.</param>
        /// <param name="legacy">The starting legacy of the new Beast.</param>
        public Beast(int row, int column, Type type) : 
            base(row, column, names[(int)type], new BaseSettings(), 
                new Combat.Legacy(initialLegacies[(int)type]))
        {
        }

        /// <summary>
        /// Constructs a new Beast at the provided point on the map.
        /// </summary>
        /// <param name="point">The position of the new Beast.</param>
        /// <param name="name">The name of the new Beast.</param>
        /// <param name="legacy">The starting legacy of the new Beast.</param>
        public Beast(Point point, Type type) :
            base(point.Y, point.X, names[(int)type], new BaseSettings(),
                new Combat.Legacy(initialLegacies[(int)type]))
        {
        }
    }
}
