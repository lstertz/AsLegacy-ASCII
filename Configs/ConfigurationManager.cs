using ContextualProgramming;
using System.IO;
using System.Reflection;
using System.Text.Json;
using PathIO = System.IO.Path;


namespace AsLegacy.Configs
{
    /// <summary>
    /// The manager of all possible configurations of the application.
    /// </summary>
    [Behavior]
    [Dependency<ConfigurationOptions>(Binding.Unique, 
        Fulfillment.SelfCreated, ConfigurationOptions)]
    public class ConfigurationManager
    {
        private const string ConfigurationOptions = "configurationOptions";

        private static readonly string ConfigsDirectory = PathIO.Combine(
            PathIO.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Resources",
            "Configs");

        public ConfigurationManager(out ConfigurationOptions configurationOptions)
        {
            string[] files = Directory.GetFiles(ConfigsDirectory, "",
                SearchOption.AllDirectories);
            string[] options = new string[files.Length];

            for (int c = 0, count = files.Length; c < count; c++)
            {
                ConfigurationDetails details = JsonSerializer
                    .Deserialize<ConfigurationDetails>(File.ReadAllText(files[c]));
                options[c] = details.Name;
            }

            configurationOptions = new()
            {
                AvailableConfigurations = options,
                ConfigurationFiles = files,
                CurrentConfiguration = -1
            };
        }

        /// <summary>
        /// Loads the configuration specified as the current configuration.
        /// </summary>
        [Operation]
        [OnChange(ConfigurationOptions)]
        public void LoadConfiguration(ConfigurationOptions configurationOptions)
        {
            int option = configurationOptions.CurrentConfiguration;
            if (option == -1)
                return;

            Configuration config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(
                configurationOptions.ConfigurationFiles[option]));
            Contextualize(config.Test);
        }
    }
}
