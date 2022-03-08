using AsLegacy.Configs;

namespace AsLegacy.Progression;


/// <summary>
/// Handles the <see cref="GameStartMessage"/> to initiate the start of the game.
/// </summary>
[Behavior]
[Dependency<ConfigurationSelection>(Binding.Unique, Fulfillment.Existing, Selection)]
[Dependency<ConfigurationSet>(Binding.Unique, Fulfillment.Existing, Configurations)]
[Dependency<GameStageCompletionMessage>(Binding.Unique, Fulfillment.SelfCreated, StageMessage)]
[Dependency<GameStartMessage>(Binding.Unique, Fulfillment.Existing, StartMessage)]
public class GameStarting
{
    private const string Configurations = "configurations";
    private const string Selection = "selection";
    private const string StageMessage = "stageMessage";
    private const string StartMessage = "startMessage";

    /// <summary>
    /// Sets the configuration selection from the start message.
    /// </summary>
    /// <param name="stageMessage">The completion message created to signify 
    /// that the opening stage has been completed.</param>
    public GameStarting(out GameStageCompletionMessage stageMessage)
    {
        // Workaround for dependencies not being injected to constructors.
        GameStartMessage startMessage = GetContext<GameStartMessage>();
        ConfigurationSet configurations = GetContext<ConfigurationSet>();
        ConfigurationSelection selection = GetContext<ConfigurationSelection>();

        int option = startMessage.SelectedConfigurationOption;
        selection.ConfigurationFile.Value = configurations.ConfigurationFiles[option];
        selection.ConfigurationName.Value = configurations.ConfigurationNames[option];

        stageMessage = new GameStageCompletionMessage();
        startMessage.HasBeenProcessed.Value = true;
    }

    /// <summary>
    /// Consumes the start message once it has been processed.
    /// </summary>
    [Operation]
    [OnChange(StartMessage, nameof(GameStartMessage.HasBeenProcessed))]
    public void ConsumeMessage(GameStartMessage startMessage)
    {
        if (startMessage.HasBeenProcessed)
            Decontextualize(startMessage);
    }
}
