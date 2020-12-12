namespace AsLegacy.GUI.HUDs
{
    /// <summary>
    /// Defines the PlayerHUD aspect of a Display, 
    /// which is responsible for showing the details of the Player Character.
    /// </summary>
    public class PlayerHUD : HUD
    {
        /// <summary>
        /// Constructs a new PlayerHUD.
        /// </summary>
        /// <param name="width">The width of the PlayerHUD.</param>
        public PlayerHUD(int width) : base(width, 205, World.Player)
        {
        }
    }
}
