namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Influence, a representation that defines a base or modifier of 
    /// an attribute of a Character.
    /// </summary>
    public record Influence
    {
        public enum Purpose
        {
            Add,
            Scale
        }

        public Aspect AffectedAspect { get; init; }

        public Purpose AffectOnAspect { get; init; }
    }
}
