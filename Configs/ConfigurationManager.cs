using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.Json;
using PathIO = System.IO.Path;

namespace AsLegacy.Configs
{
    /// <summary>
    /// The manager of all possible configurations of the application.
    /// </summary>
    public static class ConfigurationManager
    {
        private static readonly string ConfigsDirectory = PathIO.Combine(
            PathIO.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Resources",
            "Configs");

        /// <summary>
        /// Provides the available application configuration options as each 
        /// configuration's user-friendly name.
        /// </summary>
        public static ReadOnlyCollection<string> ConfigurationOptions { get; private set; }
        private static readonly Dictionary<string, string> _configurationOptions = new();

        private static Configuration _currentConfiguration = null;


        /// <summary>
        /// Initializes the configuration manager by loading the available application
        /// configuration options.
        /// </summary>
        public static void Initialize()
        {
            string[] files = Directory.GetFiles(ConfigsDirectory, "", 
                SearchOption.AllDirectories);
            string[] options = new string[files.Length];

            for (int c = 0, count = files.Length; c < count; c++)
            {
                ConfigurationDetails details = JsonSerializer
                    .Deserialize<ConfigurationDetails>(File.ReadAllText(files[c]));
                options[c] = details.Name;
                _configurationOptions.Add(details.Name, files[c]);
            }

            ConfigurationOptions = new(options);
        }

        /// <summary>
        /// Loads the configuration, identified by the provided configuration option, 
        /// as the configuration of the game.
        /// </summary>
        /// <param name="configurationOption">The option identifying the 
        /// configuration to be loaded.</param>
        public static void LoadConfiguration(string configurationOption)
        {
            _currentConfiguration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(
                _configurationOptions[configurationOption]));
        }
    }
}
