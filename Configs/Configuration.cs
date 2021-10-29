using AsLegacy.Configs.Converters;
using System.Text.Json.Serialization;

namespace AsLegacy.Configs
{
    /// <summary>
    /// Defines the configuration of the application, specifically what implementations 
    /// and settings the application should use.
    /// </summary>
    public class Configuration : ConfigurationDetails
    {
        [JsonConverter(typeof(TestConverter))]
        public ITest Test { get; init; }
    }
}
