using ContextualProgramming;
using System;

namespace AsLegacy.Configs
{
    /// <summary>
    /// The configuration options that can be loaded to define the 
    /// functionality of the game.
    /// </summary>
    [Context]
    public class ConfigurationSet
    {
        /// <summary>
        /// The user-friendly names of the available configurations.
        /// </summary>
        public ReadonlyContextStateList<string> ConfigurationNames { get; init; } = 
            Array.Empty<string>();

        /// <summary>
        /// The file paths of the available configurations.
        /// </summary>
        public ReadonlyContextStateList<string> ConfigurationFiles { get; init; } =
            Array.Empty<string>();
    }
}