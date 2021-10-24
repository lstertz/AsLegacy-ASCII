namespace AsLegacy.Configs
{
    /// <summary>
    /// Defines the configuration of the application, specifically what implementations 
    /// and settings the application should use.
    /// </summary>
    public class Configuration : IConfigurationDetails
    {
        /// <inheritdoc/>
        public string Name { get; }
    }
}
