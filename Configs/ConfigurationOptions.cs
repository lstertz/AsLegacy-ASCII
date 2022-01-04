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
        /// The configuration currently chosen to define the game's functionality.
        /// </summary>
        public ContextState<string> CurrentConfiguration { get; init; }
    }
}
