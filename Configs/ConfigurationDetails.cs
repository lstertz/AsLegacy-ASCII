using System.Text.Json.Serialization;

namespace AsLegacy.Configs
{
    /// <inheritdoc cref="IConfigurationDetails"/>
    public class ConfigurationDetails : IConfigurationDetails
    {
        /// <inheritdoc/>
        public string Name { get; private set; }

        /// <summary>
        /// Default JSON constructor.
        /// </summary>
        /// <param name="name"><see cref="Name"/></param>
        [JsonConstructor]
        public ConfigurationDetails(string name) => (Name) = (name);
}
}
