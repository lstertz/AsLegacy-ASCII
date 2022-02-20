namespace AsLegacy;

/// <summary>
/// Defines the details of the player's character.
/// </summary>
[Context]
public class PlayerDetails
{
    /// <summary>
    /// The name of the player's living (or last living) character.
    /// </summary>
    public ContextState<string> CharacterName { get; init; } = new(null);

    /// <summary>
    /// The name of the player's characters' lineage.
    /// </summary>
    public ContextState<string> LineageName { get; init; } = new(null);
}
