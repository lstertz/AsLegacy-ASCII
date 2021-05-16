namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Influence, a representation that defines a base or modifier of 
    /// an attribute of a Character.
    /// </summary>
    public record Influence
    {
        public enum Attribute
        {
            AreaOfEffectDamage,
            MaxHealth
        }

        public enum Purpose
        {
            Add,
            Scale
        }

        public Attribute AffectedAttribute { get; init; }

        public Purpose AffectOnAttribute { get; init; }
    }
}
