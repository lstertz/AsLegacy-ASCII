using ContextualProgramming;

namespace AsLegacy.Configs
{
    /// <summary>
    /// The details of a selected configuration option from a <see cref="ConfigurationSet"/>.
    /// </summary>
    [Context]
    public class ConfigurationSelection
    {
        /// <summary>
        /// The user-friendly name of a selected configuration.
        /// </summary>
        public ContextState<string> ConfigurationName { get; init; } = new(null);

        /// <summary>
        /// The file path of a selected configuration. 
        /// </summary>
        public ContextState<string> ConfigurationFile { get; init; } = new(null);


        /// <summary>
        /// Specifies whether a configuration has been selected.
        /// </summary>
        /// <returns>True if a configuration has been selected, as determined 
        /// by whether the configuration name and file path have been set to 
        /// non-null values.</returns>
        public bool HasSelection() => ConfigurationName != null && ConfigurationFile != null;
    }
}