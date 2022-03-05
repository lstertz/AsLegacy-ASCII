namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines the settings of the display, including what screen is currently being displayed.
    /// </summary>
    [Context]
    public class Display
    {
        /// <summary>
        /// The height of the display, in cells.
        /// </summary>
        public ReadonlyContextState<int> Height { get; init; } = 25;

        /// <summary>
        /// The width of the display, in cells.
        /// </summary>
        public ReadonlyContextState<int> Width { get; init; } = 80;
    }
}
