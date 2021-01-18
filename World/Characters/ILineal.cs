namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines the Interface of a Character that belongs to a lineage, 
    /// providing access to the details of that lineage.
    /// </summary>
    public interface ILineal
    {
        /// <summary>
        /// The highest recorded legacy of a Character's Lineage, 
        /// represented as a numerical value (points)
        /// </summary>
        int LegacyRecord { get; }

        // TODO :: Lineage Name.
    }
}
