using ContextualProgramming;

namespace AsLegacy.Configs
{
    /// <summary>
    /// The configuration options that can be loaded to define the 
    /// functionality of the game.
    /// </summary>
    [Context]
    public class ConfigurationOptions
    {
        /// <summary>
        /// The user-friendly names of the available configurations.
        /// </summary>
        public ContextStateList<string> AvailableConfigurations { get; init; }

        /// <summary>
        /// The file paths of the available configurations.
        /// </summary>
        public ContextStateList<string> ConfigurationFiles { get; init; }

        /// <summary>
        /// The index of the configuration currently chosen to define the game's functionality.
        /// </summary>
        public ContextState<int> CurrentConfiguration { get; init; }
    }
}
