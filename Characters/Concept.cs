using System;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Concept, a Character Talent that is a singular concept defining the 
    /// underlying purpose and properties of a set of modular skills.
    /// </summary>
    public record Concept : Talent
    {
        public Affinity[] Affinities { get; init; }

        // TODO :: Define the properties of the Concept.
        //          The properties specify how the Concept applies in non-skill-specific ways,
        //              as well as how the Concept is categorized, which determines how 
        //              the Concept's skills can be augmented by other Concepts.

        /// <summary>
        /// Constructs a new <see cref="Concept"/>.
        /// </summary>
        /// <param name="title">The title of the <see cref="Concept"/>.</param>
        public Concept() : base()
        {
        }
    }
}
