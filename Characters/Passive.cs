using System;
using static AsLegacy.World;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Passive, a Character Talent that defines a passive alteration 
    /// for a single Character affect based on the amount of investment in the passive.
    /// </summary>
    public record Passive : Talent
    {
        /// <summary>
        /// The altered Character affect.
        /// </summary>
        public Character.Affect Affect { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="Passive"/>.
        /// </summary>
        /// <param name="affect">The altered Character affect.</param>
        /// <param name="title">The title of the <see cref="Passive"/>.</param>
        /// <param name="investmentAlgorithm">The algorithm that calculates the affect amount 
        /// to be applied, for a specified investment.</param>
        public Passive(Character.Affect affect) : base()
        {
            Affect = affect;
        }
    }
}
