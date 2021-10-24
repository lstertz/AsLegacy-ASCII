namespace AsLegacy.Configs
{
    /// <summary>
    /// Defines the details of a configuration of the application.
    /// </summary>
    public interface IConfigurationDetails
    {
        /// <summary>
        /// The user-friendly name that identifies the configuration.
        /// </summary>
        string Name { get; }
    }
}
