namespace AsLegacy.GUI.HUDs
{
    /// <summary>
    /// Defines the FocusHUD aspect of a Display, 
    /// which is responsible for showing the details of the currently focused Character.
    /// </summary>
    public class FocusHUD : HUD
    {
        /// <summary>
        /// Constructs a new FocusHUD.
        /// </summary>
        /// <param name="width">The width of the FocusHUD.</param>
        public FocusHUD(int width) : base(width, 205, () => { return AsLegacy.Focus; })
        {
        }
    }
}
