namespace AsLegacy.Progression;

/// <summary>
/// Defines the forward and backward progressions of a stage.
/// </summary>
public struct StageProgressions
{
    /// <summary>
    /// The next stage.
    /// </summary>
    public GameStageMap.Stage Next { get; init; }

    /// <summary>
    /// The previous stage.
    /// </summary>
    public GameStageMap.Stage Previous { get; init; }
}
