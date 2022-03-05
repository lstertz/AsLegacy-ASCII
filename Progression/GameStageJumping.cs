namespace AsLegacy.Progression;

/// <summary>
/// Handles a <see cref="GameStageJumpMessage"/> to jump to a stage of the game.
/// </summary>
[Behavior]
[Dependency<GameStageJumpMessage>(Binding.Unique, Fulfillment.Existing, Message)]
[Dependency<GameState>(Binding.Unique, Fulfillment.Existing, State)]
public class GameStageJumping
{
    private const string Message = "message";
    private const string State = "state";

    /// <summary>
    /// Jumps to the specified stage of the game in accordance with the jump message.
    /// </summary>
    public GameStageJumping()
    {
        // Workaround for dependencies not being injected to constructors.
        GameStageJumpMessage message = GetContext<GameStageJumpMessage>();
        GameState state = GetContext<GameState>();

        state.CurrentStage.Value = message.Stage;
        message.HasBeenProcessed.Value = true;
    }

    /// <summary>
    /// Consumes the jump message once it has been processed.
    /// </summary>
    [Operation]
    [OnChange(Message, nameof(GameStageJumpMessage.HasBeenProcessed))]
    public void ConsumeMessage(GameStageJumpMessage message)
    {
        if (message.HasBeenProcessed)
            Decontextualize(message);
    }
}
