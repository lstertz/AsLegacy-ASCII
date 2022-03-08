namespace AsLegacy.Progression;

/// <summary>
/// A message specifying that the game should jump to a specified stage.
/// </summary>
[Context]
public class GameStageJumpMessage
{
    /// <summary>
    /// Whether this message has been processed.
    /// </summary>
    public ContextState<bool> HasBeenProcessed { get; init; } = false;

    /// <summary>
    /// The stage to be jumped to.
    /// </summary>
    public ContextState<GameStageMap.Stage> Stage { get; init; } = GameStageMap.Stage.Opening;
}
