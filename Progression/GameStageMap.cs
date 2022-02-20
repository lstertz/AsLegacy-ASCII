using System.Collections.Generic;

namespace AsLegacy.Progression;

/// <summary>
/// Defines the mapping of stages going forward and backward in progression.
/// </summary>
[Context]
public class GameStageMap
{
    /// <summary>
    /// The stages of the game.
    /// </summary>
    public enum Stage
    {
        Opening = 0,
        Setup = 10,
        Play = 20,
        Results = 30
    }

    // Workaround for no ContextStateMap.

    /// <summary>
    /// The stage progression mapping.
    /// </summary>
    public Dictionary<Stage, StageProgressions> StageProgressions { get; init; } = new();
}
