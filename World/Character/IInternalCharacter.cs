using static AsLegacy.World.Character;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines the World's private interface for a Character.
        /// </summary>
        private interface IInternalCharacter
        {
            /// <summary>
            /// The Character's AI.
            /// </summary>
            IAI AI { get; }

            /// <summary>
            /// Updates a Character's internal processes for the given passed time.
            /// </summary>
            /// <param name="timeDelta">The amount of time passed since 
            /// the last update.</param>
            void Update(int timeDelta);
        }
    }
}
