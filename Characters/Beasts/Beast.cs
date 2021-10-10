using Microsoft.Xna.Framework;
using System;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Beast, which is a Character that cannot 
    /// use Items and has a number of nature-based skills and abilities.
    /// </summary>
    public partial class Beast : World.Character
    {
        private const int NumOfBeastTypes = 3;
        private const int PassivePointsUpperBound = 16;

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
            return (Type) r.Next(0, NumOfBeastTypes);
        }

        /// <summary>
        /// Provides a new set of BeastSettings for the given Beast Type.
        /// </summary>
        /// <param name="type">The Type of Beast.</param>
        /// <returns>A newly constructed BeastSettings.</returns>
        private static Settings GetBeastSettings(Type type)
        {
            Random r = new Random();
            int[] passiveInvestments = new int[]
            {
                r.Next(0, PassivePointsUpperBound),
                r.Next(0, PassivePointsUpperBound),
                r.Next(0, PassivePointsUpperBound)
            };

            switch (type)
            {
                case Type.GiantRat:
                    return new GiantRatSettings(passiveInvestments);
                case Type.Wolf:
                    return new WolfSettings(passiveInvestments);
                case Type.Bear:
                    return new BearSettings(passiveInvestments);
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
        public Beast(int row, int column, Type type) : this(row, column, GetBeastSettings(type))
        { }

        /// <summary>
        /// Constructs a new Beast at the provided point on the map.
        /// </summary>
        /// <param name="point">The position of the new Beast.</param>
        public Beast(Point point, Type type) : this(point.Y, point.X, GetBeastSettings(type))
        { }

        private Beast(int row, int column, Settings settings) : 
            base(row, column, settings.Name, settings, settings.InitialLegacy)
        { }
    }
}
