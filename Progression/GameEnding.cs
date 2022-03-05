namespace AsLegacy.Progression;


/// <summary>
/// Handles the <see cref="GameRestartMessage"/> to initiate the restart of the game.
/// </summary>
[Behavior]
[Dependency<GameStageCompletionMessage>(Binding.Unique, Fulfillment.SelfCreated, StageMessage)]
[Dependency<GameEndMessage>(Binding.Unique, Fulfillment.Existing, EndMessage)]
public class GameEnding
{
    private const string EndMessage = "endMessage";
    private const string StageMessage = "stageMessage";

    /// <summary>
    /// Sets the configuration selection from the start message.
    /// </summary>
    /// <param name="stageMessage">The completion message created to signify 
    /// that the opening stage has been completed.</param>
    public GameEnding(out GameStageCompletionMessage stageMessage)
    {
        // Workaround for dependencies not being injected to constructors.
        GameEndMessage endMessage = GetContext<GameEndMessage>();

        stageMessage = new GameStageCompletionMessage();
        endMessage.HasBeenProcessed.Value = true;
    }

    /// <summary>
    /// Consumes the restart message once it has been processed.
    /// </summary>
    [Operation]
    [OnChange(EndMessage, nameof(GameEndMessage.HasBeenProcessed))]
    public void ConsumeMessage(GameEndMessage endMessage)
    {
        if (endMessage.HasBeenProcessed)
            Decontextualize(endMessage);
    }
}
