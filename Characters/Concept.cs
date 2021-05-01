using System.Collections.ObjectModel;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Concept, a Character Talent that is a singular concept defining the 
    /// underlying purpose and properties of a set of modular <see cref="Skill"/>s, which are 
    /// represented abstractly by the Affinities (<see cref="Affinity"/>) of the Concept.
    /// </summary>
    public record Concept : Talent
    {
        /// <summary>
        /// A collection of <see cref="Affinity"/> that define the practical applications 
        /// of this <see cref="Concept"/>.
        /// </summary>
        public ReadOnlyCollection<Affinity> Affinities { get; init; }

        /// <summary>
        /// The category of this <see cref="Concept"/> when considered as a <see cref="Skill"/>.
        /// </summary>
        public Skill.Category Category { get; init; }

        /// <summary>
        /// The type of this <see cref="Concept"/> when considered as a <see cref="Skill"/>.
        /// </summary>
        public Skill.Type Type { get; init; }
    }
}
