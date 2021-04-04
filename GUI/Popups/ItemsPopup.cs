using Microsoft.Xna.Framework;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the ItemsPopup, which displays information and 
    /// options for the Player to manage their Character's items.
    /// </summary>
    public class ItemsPopup : Popup
    {

        /// <summary>
        /// Constructs a new ItemsPopup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public ItemsPopup(int width, int height) : base("Items", width, height)
        {
        }

        protected override void Invalidate()
        {
            base.Invalidate();

            Print(2, 2, "Items to be managed here.", Color.White);
        }
    }
}
