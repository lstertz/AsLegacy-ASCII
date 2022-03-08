namespace AsLegacy.Progression;

/// <summary>
/// A message specifying that the game has ended (the goal has been reached).
/// </summary>
[Context]
public class GameEndMessage
{
    /// <summary>
    /// Whether this message has been processed.
    /// </summary>
    public ContextState<bool> HasBeenProcessed { get; init; } = false;
}
