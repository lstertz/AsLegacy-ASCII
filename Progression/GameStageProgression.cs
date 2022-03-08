namespace AsLegacy.Progression;

/// <summary>
/// Handles a <see cref="GameStageCompletionMessage"/> to progress the stage 
/// of the game.
/// </summary>
[Behavior]
[Dependency<GameStageCompletionMessage>(Binding.Unique, Fulfillment.Existing, Message)]
[Dependency<GameStageMap>(Binding.Unique, Fulfillment.Existing, StageMap)]
[Dependency<GameState>(Binding.Unique, Fulfillment.Existing, State)]
public class GameStageProgression
{
    private const string Message = "message";
    private const string StageMap = "stageMap";
    private const string State = "state";

    /// <summary>
    /// Progresses the stage of the game in accordance with the 
    /// stage mapping and the completion message.
    /// </summary>
    public GameStageProgression()
    {
        // Workaround for dependencies not being injected to constructors.
        GameStageCompletionMessage message = GetContext<GameStageCompletionMessage>();
        GameStageMap stageMap = GetContext<GameStageMap>();
        GameState state = GetContext<GameState>();

        if (message.CompletedGoingForward)
            state.CurrentStage.Value = stageMap.StageProgressions[state.CurrentStage].Next;
        else
            state.CurrentStage.Value = stageMap.StageProgressions[state.CurrentStage].Previous;

        message.HasBeenProcessed.Value = true;
    }

    /// <summary>
    /// Consumes the completion message once it has been processed.
    /// </summary>
    [Operation]
    [OnChange(Message, nameof(GameStageCompletionMessage.HasBeenProcessed))]
    public void ConsumeMessage(GameStageCompletionMessage message)
    {
        if (message.HasBeenProcessed)
            Decontextualize(message);
    }
}
