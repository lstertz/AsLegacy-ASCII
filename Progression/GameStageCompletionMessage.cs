namespace AsLegacy.Progression;

/// <summary>
/// A message specifying that the current game stage has been completed.
/// </summary>
[Context]
public class GameStageCompletionMessage
{
    /// <summary>
    /// Whether the stage was completed as part of a forward progression.
    /// </summary>
    public ContextState<bool> CompletedGoingForward { get; init; } = true;

    /// <summary>
    /// Whether this message has been processed.
    /// </summary>
    public ContextState<bool> HasBeenProcessed { get; init; } = false;
}
