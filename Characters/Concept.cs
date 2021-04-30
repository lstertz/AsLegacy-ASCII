using System.Collections.ObjectModel;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Concept, a Character Talent that is a singular concept defining the 
    /// underlying purpose and properties of a set of modular skills.
    /// </summary>
    public record Concept : Talent
    {
        public ReadOnlyCollection<Affinity> Affinities { get; init; }

        public Skill.Category Category { get; init; }

        public Skill.Type Type { get; init; }
    }
}
