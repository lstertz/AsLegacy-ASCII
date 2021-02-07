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
        /// Defines the settings of a Beast.
        /// </summary>
        protected abstract class BeastSettings : BaseSettings
        {
            /// <summary>
            /// The initial legacy of the Beast.
            /// </summary>
            public abstract Combat.Legacy InitialLegacy { get; }

            /// <summary>
            /// The name of the Beast.
            /// </summary>
            public abstract string Name { get; }
        }

        /// <summary>
        /// Defines the settings of a Giant Rat.
        /// </summary>
        protected class GiantRatSettings : BeastSettings
        {
            /// <inheritdoc/>
            public override Color GlyphColor => Color.DarkOrange;
            /// <inheritdoc/>
            public override Combat.Legacy InitialLegacy => new Combat.Legacy(4);
            /// <inheritdoc/>
            public override string Name => "Giant Rat";

            /// <inheritdoc/>
            public override int AttackGlyph => 229;//'σ'
            /// <inheritdoc/>
            public override int DefendGlyph => 239;//'∩'
            /// <inheritdoc/>
            public override int NormalGlyph => 224;//'α'

            /// <inheritdoc/>
            public override float InitialAttackDamage => 1f;
            /// <inheritdoc/>
            public override int InitialAttackInterval => 2000;
            /// <inheritdoc/>
            public override float InitialBaseMaxHealth => 4.0f;
        }

        /// <summary>
        /// Defines the settings of a Wolf.
        /// </summary>
        protected class WolfSettings : BeastSettings
        {
            /// <inheritdoc/>
            public override Color GlyphColor => Color.DarkOrange;
            /// <inheritdoc/>
            public override Combat.Legacy InitialLegacy => new Combat.Legacy(8);
            /// <inheritdoc/>
            public override string Name => "Wolf";

            /// <inheritdoc/>
            public override int AttackGlyph => 229;//'σ'
            /// <inheritdoc/>
            public override int DefendGlyph => 239;//'∩'
            /// <inheritdoc/>
            public override int NormalGlyph => 224;//'α'

            /// <inheritdoc/>
            public override float InitialAttackDamage => 1.5f;
            /// <inheritdoc/>
            public override int InitialAttackInterval => 1500;
            /// <inheritdoc/>
            public override float InitialBaseMaxHealth => 7.0f;
        }

        /// <summary>
        /// Defines the settings of a Bear.
        /// </summary>
        protected class BearSettings : BeastSettings
        {
            /// <inheritdoc/>
            public override Color GlyphColor => Color.DarkOrange;
            /// <inheritdoc/>
            public override Combat.Legacy InitialLegacy => new Combat.Legacy(12);
            /// <inheritdoc/>
            public override string Name => "Bear";

            /// <inheritdoc/>
            public override int AttackGlyph => 229;//'σ'
            /// <inheritdoc/>
            public override int DefendGlyph => 239;//'∩'
            /// <inheritdoc/>
            public override int NormalGlyph => 224;//'α'

            /// <inheritdoc/>
            public override float InitialAttackDamage => 4f;
            /// <inheritdoc/>
            public override int InitialAttackInterval => 3000;
            /// <inheritdoc/>
            public override float InitialBaseMaxHealth => 10f;
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

        /// <summary>
        /// Provides a new set of BeastSettings for the given Beast Type.
        /// </summary>
        /// <param name="type">The Type of Beast.</param>
        /// <returns>A newly constructed BeastSettings.</returns>
        private static BeastSettings GetBeastSettings(Type type)
        {
            switch (type)
            {
                case Type.GiantRat:
                    return new GiantRatSettings();
                case Type.Wolf:
                    return new WolfSettings();
                case Type.Bear:
                    return new BearSettings();
                default:
                    throw new NotImplementedException("That Beast Type does " +
                        "not have settings implemented yet.");
            };
        }

        /// <summary>
        /// Constructs a new Beast at the provided row and column on the map.
        /// </summary>
        /// <param name="row">The row position of the new Beast.</param>
        /// <param name="column">The column position of the new Beast.</param>
        /// <param name="name">The name of the new Beast.</param>
        /// <param name="legacy">The starting legacy of the new Beast.</param>
        public Beast(int row, int column, Type type) : this(row, column, GetBeastSettings(type))
        { }

        /// <summary>
        /// Constructs a new Beast at the provided point on the map.
        /// </summary>
        /// <param name="point">The position of the new Beast.</param>
        /// <param name="name">The name of the new Beast.</param>
        /// <param name="legacy">The starting legacy of the new Beast.</param>
        public Beast(Point point, Type type) : this(point.Y, point.X, GetBeastSettings(type))
        { }

        private Beast(int row, int column, BeastSettings settings) : 
            base(row, column, settings.Name, settings, settings.InitialLegacy)
        { }
    }
}
