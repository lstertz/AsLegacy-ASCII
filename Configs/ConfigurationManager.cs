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
    [Dependency<Configurations>(Binding.Unique, Fulfillment.SelfCreated, Configurations)]
    public class ConfigurationManager
    {
        private const string Configurations = "configurations";

        private static readonly string ConfigsDirectory = PathIO.Combine(
            PathIO.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Resources",
            "Configs");


        public ConfigurationManager(out Configurations configurations)
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

            configurations = new()
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
        [OnChange(Configurations)]
        public void LoadConfiguration(Configurations configurations)
        {
            int option = configurations.CurrentConfiguration;
            if (option == -1)
                return;

            Configuration config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(
                configurations.ConfigurationFiles[option]));
            Contextualize(config.Test);
        }
    }
}
