namespace AsLegacy;

// TODO :: Create PlaySettings through configuration loading.

/// <summary>
/// Defines the play settings of the game, such as the rules and goals.
/// </summary>
[Context]
public class PlaySettings
{
    /// <summary>
    /// The legacy (point) goal of the game.
    /// </summary>
    public ContextState<int> Goal { get; init; } = 25;
}
