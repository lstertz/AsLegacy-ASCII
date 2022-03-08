using AsLegacy.Progression;

namespace AsLegacy;


/// <summary>
/// Defines the state of the game, such as its current stage and whether 
/// it should continue running.
/// </summary>
[Context]
public class GameState
{
    /// <summary>
    /// Specifies whether the game should continue running.
    /// </summary>
    public ContextState<bool> ContinueRunning { get; init; } = true;

    /// <summary>
    /// Specifies the current stage of the game.
    /// </summary>
    public ContextState<GameStageMap.Stage> CurrentStage { get; init; } = 
        GameStageMap.Stage.Opening;
}
