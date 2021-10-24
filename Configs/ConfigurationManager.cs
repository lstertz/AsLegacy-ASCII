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
        private static readonly Dictionary<string, string> _configOptions = new();

        /// <summary>
        /// Initializes the configuration manager by loading the available application
        /// configuration options.
        /// </summary>
        public static void Initialize()
        {
            string[] files = Directory.GetFiles(ConfigsDirectory);
            string[] options = new string[files.Length];

            for (int c = 0, count = files.Length; c < count; c++)
            {
                ConfigurationDetails details = JsonSerializer
                    .Deserialize<ConfigurationDetails>(File.ReadAllText(files[c]));
                options[c] = details.Name;
                _configOptions.Add(details.Name, files[c]);
                System.Diagnostics.Debug.WriteLine(details.Name);
            }

            ConfigurationOptions = new(options);
        }
    }
}
