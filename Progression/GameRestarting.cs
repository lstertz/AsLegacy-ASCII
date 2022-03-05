namespace AsLegacy.Progression;


/// <summary>
/// Handles the <see cref="GameRestartMessage"/> to initiate the restart of the game.
/// </summary>
[Behavior]
[Dependency<GameStageCompletionMessage>(Binding.Unique, Fulfillment.SelfCreated, StageMessage)]
[Dependency<GameRestartMessage>(Binding.Unique, Fulfillment.Existing, RestartMessage)]
public class GameRestarting
{
    private const string RestartMessage = "restartMessage";
    private const string StageMessage = "stageMessage";

    /// <summary>
    /// Sets the configuration selection from the start message.
    /// </summary>
    /// <param name="stageMessage">The completion message created to signify 
    /// that the opening stage has been completed.</param>
    public GameRestarting(out GameStageCompletionMessage stageMessage)
    {
        // Workaround for dependencies not being injected to constructors.
        GameRestartMessage restartMessage = GetContext<GameRestartMessage>();

        stageMessage = new GameStageCompletionMessage();
        stageMessage.CompletedGoingForward.Value = false;

        restartMessage.HasBeenProcessed.Value = true;
    }

    /// <summary>
    /// Consumes the restart message once it has been processed.
    /// </summary>
    [Operation]
    [OnChange(RestartMessage, nameof(GameRestartMessage.HasBeenProcessed))]
    public void ConsumeMessage(GameRestartMessage restartMessage)
    {
        if (restartMessage.HasBeenProcessed)
            Decontextualize(restartMessage);
    }
}
