namespace AsLegacy.Progression;

/// <summary>
/// A message specifying that the game has been started.
/// </summary>
[Context]
public class GameStartMessage
{
    /// <summary>
    /// Whether this message has been processed.
    /// </summary>
    public ContextState<bool> HasBeenProcessed { get; init; } = false;

    /// <summary>
    /// The selected configuration option specifying the type of game to start.
    /// </summary>
    public ContextState<int> SelectedConfigurationOption { get; init; } = 0;
}
