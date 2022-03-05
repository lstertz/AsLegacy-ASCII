namespace AsLegacy.Progression;

/// <summary>
/// A message specifying that the game has been restarted.
/// </summary>
[Context]
public class GameRestartMessage
{
    /// <summary>
    /// Whether this message has been processed.
    /// </summary>
    public ContextState<bool> HasBeenProcessed { get; init; } = false;
}
