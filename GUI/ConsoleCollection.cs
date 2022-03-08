using System;

namespace AsLegacy.GUI
{
    /// <summary>
    /// The classified consoles of the game.
    /// </summary>
    [Context]
    public class ConsoleCollection
    {
        /// <summary>
        /// The highest-level console of the game.
        /// This console has no parent and controls the over-arching display of the game.
        /// </summary>
        public ContextState<Console> PrimaryConsole { get; init; } = new(null);

        public ContextStateList<Console> ScreenConsoles { get; init; } =
            new(Array.Empty<Console>());
    }
}
