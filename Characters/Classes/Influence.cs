namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Influence, a representation that defines a base or scale of 
    /// an <see cref="Aspect"/>.
    /// </summary>
    public record Influence
    {
        /// <summary>
        /// The purpose of an influence, how it is to be applied 
        /// to an <see cref="AffectedAspect"/>.
        /// </summary>
        public enum Purpose
        {
            /// <summary>
            /// The purpose of being added to the base value of an <see cref="Aspect"/>.
            /// </summary>
            Add,
            /// <summary>
            /// The purpose of being added to a scale value, to scale the base value 
            /// of an <see cref="Aspect"/>.
            /// </summary>
            Scale
        }

        /// <summary>
        /// The influenced <see cref="Aspect"/>.
        /// </summary>
        public Aspect AffectedAspect { get; init; }

        /// <summary>
        /// The <see cref="Purpose"/> of the influence, i.e. how it is applied.
        /// </summary>
        public Purpose AffectOnAspect { get; init; }
    }
}
