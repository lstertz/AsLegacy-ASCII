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
    [Dependency<ConfigurationSet>(Binding.Unique, Fulfillment.SelfCreated, Options)]
    [Dependency<ConfigurationSelection>(Binding.Unique, Fulfillment.SelfCreated, Selection)]
    public class ConfigurationManagement
    {
        private const string Options = "options";
        private const string Selection = "selection";

        private static readonly string ConfigsDirectory = PathIO.Combine(
            PathIO.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Resources",
            "Configs");


        public ConfigurationManagement(out ConfigurationSet options, 
            out ConfigurationSelection selection)
        {
            string[] files = Directory.GetFiles(ConfigsDirectory, "",
                SearchOption.AllDirectories);
            string[] names = new string[files.Length];

            for (int c = 0, count = files.Length; c < count; c++)
            {
                ConfigurationDetails details = JsonSerializer
                    .Deserialize<ConfigurationDetails>(File.ReadAllText(files[c]));
                names[c] = details.Name;
            }

            options = new()
            {
                ConfigurationNames = names,
                ConfigurationFiles = files
            };
            selection = new();
        }

        /// <summary>
        /// Loads the configuration specified as the current configuration.
        /// </summary>
        [Operation]
        [OnChange(Selection)]
        public void LoadConfiguration(ConfigurationSelection selection)
        {
            if (!selection.HasSelection() || selection.IsLoaded)
                return;

            Configuration config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(
                selection.ConfigurationFile));
            Contextualize(config.Test);

            selection.IsLoaded.Value = true;
        }
    }
}
