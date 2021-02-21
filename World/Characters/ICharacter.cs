using AsLegacy.Abstractions;
using static AsLegacy.World.Character;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines the World's private interface for a Character.
        /// </summary>
        private interface ICharacter
        {
            /// <summary>
            /// The Character's AI.
            /// </summary>
            IAI AI { get; }
        }
    }
}
