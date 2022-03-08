namespace AsLegacy.Progression;

/// <summary>
/// A message specifying that the game has been set up.
/// </summary>
[Context]
public class GameSetUpMessage
{
    /// <summary>
    /// Whether this message has been processed.
    /// </summary>
    public ContextState<bool> HasBeenProcessed { get; init; } = false;

    /// <summary>
    /// The set lineage of the game about to be played.
    /// </summary>
    public ContextState<string> Lineage { get; init; } = "";

    /// <summary>
    /// The set starting character name of the game about to be played.
    /// </summary>
    public ContextState<string> Name { get; init; } = "";
}
