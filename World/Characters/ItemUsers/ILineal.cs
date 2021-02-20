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

        /// <summary>
        /// The name of the Character's Lineage.
        /// </summary>
        string LineageName { get; }

        /// <summary>
        /// The full name, formatted Character name and Lineage name, of this ILineal Character.
        /// </summary>
        string FullName { get; }
    }
}
