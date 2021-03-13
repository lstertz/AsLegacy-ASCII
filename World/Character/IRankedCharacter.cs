using System;

namespace AsLegacy
{
    /// <summary>
    /// Defines the interface of a Character's ranking properties and implements 
    /// the IComparable to rank the Character.
    /// </summary>
    interface IRankedCharacter : IComparable<World.Character>
    {
        /// <summary>
        /// The name of this Character.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The legacy of this Character, represented as a numerical value (points).
        /// </summary>
        int Legacy { get; }

        /// <inheritdoc/>
        int IComparable<World.Character>.CompareTo(World.Character other)
        {
            if (Legacy > other.Legacy)
                return -1;
            if (Legacy < other.Legacy)
                return 1;

            int nameCompare = Name.CompareTo(other.Name);
            if (nameCompare != 0)
                return nameCompare;

            return GetHashCode().CompareTo(other.GetHashCode());
        }
    }
}
