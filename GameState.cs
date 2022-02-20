namespace AsLegacy;

/// <summary>
/// Defines the state of the game, such as its current stage and whether 
/// it should continue running.
/// </summary>
[Context]
public class GameState
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

    /// <summary>
    /// Specifies whether the game should continue running.
    /// </summary>
    public ContextState<bool> ContinueRunning { get; init; } = true;

    /// <summary>
    /// Specifies the current stage of the game.
    /// </summary>
    public ContextState<Stage> CurrentStage { get; init; } = Stage.Opening;
}
