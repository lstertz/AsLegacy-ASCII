namespace AsLegacy.Progression;


/// <summary>
/// Handles the <see cref="GameSetUpMessage"/> to initiate the setting up of the game.
/// </summary>
[Behavior]
[Dependency<GameStageCompletionMessage>(Binding.Unique, Fulfillment.SelfCreated, StageMessage)]
[Dependency<GameSetUpMessage>(Binding.Unique, Fulfillment.Existing, SetUpMessage)]
public class GameSettingUp
{
    private const string StageMessage = "stageMessage";
    private const string SetUpMessage = "setUpMessage";

    /// <summary>
    /// Sets the configuration selection from the start message.
    /// </summary>
    /// <param name="stageMessage">The completion message created to signify 
    /// that the set up stage has been completed.</param>
    public GameSettingUp(out GameStageCompletionMessage stageMessage)
    {
        // Workaround for dependencies not being injected to constructors.
        GameSetUpMessage setUpMessage = GetContext<GameSetUpMessage>();

        // TODO :: Create Player context with the name and lineage.
        // TODO :: Create contexts for any other player-specified game settings.

        stageMessage = new GameStageCompletionMessage();
        setUpMessage.HasBeenProcessed.Value = true;
    }

    /// <summary>
    /// Consumes the set up message once it has been processed.
    /// </summary>
    [Operation]
    [OnChange(SetUpMessage, nameof(GameSetUpMessage.HasBeenProcessed))]
    public void ConsumeMessage(GameSetUpMessage setUpMessage)
    {
        if (setUpMessage.HasBeenProcessed)
            Decontextualize(setUpMessage);
    }
}
