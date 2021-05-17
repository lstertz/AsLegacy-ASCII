using System.Collections.ObjectModel;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Attribute, a single quality of an <see cref="Affinity"/> that specifies 
    /// some feature or quantified affect of that <see cref="Affinity"/>.
    /// </summary>
    public record Attribute
    {
        /// <summary>
        /// Any <see cref="Aspect"/>s that should apply to this <see cref="Attribute"/>'s affect.
        /// </summary>
        public ReadOnlyCollection<Aspect> Aspects { get; init; } = 
            new(System.Array.Empty<Aspect>());

        /// <summary>
        /// The initial scalar to be applied to the affect of this <see cref="Attribute"/>.
        /// </summary>
        public float BaseScale { get; init; } = 1.0f;

        /// <summary>
        /// The initial (unscaled) value of the affect of this <see cref="Attribute"/>.
        /// </summary>
        public float BaseValue { get; init; } = 0.0f;

        /// <summary>
        /// The descriptive name for presenting or referring to this <see cref="Attribute"/>.
        /// </summary>
        public string Name { get; init; }
    }
}
