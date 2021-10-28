using System.Text.Json.Serialization;

namespace AsLegacy.Configs
{
    /// <summary>
    /// Defines the details of a configuration of the application.
    /// </summary>
    public class ConfigurationDetails
    {
        /// <summary>
        /// The user-friendly name that identifies the configuration.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Default JSON constructor.
        /// </summary>
        /// <param name="name"><see cref="Name"/></param>
        [JsonConstructor]
        public ConfigurationDetails(string name) => (Name) = (name);
}
}
