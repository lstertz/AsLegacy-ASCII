using SadConsole;

using AsLegacy.GUI.Elements;
using System.Collections.Generic;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines the NearbyPanel aspect of a Display, 
    /// which is responsible for showing an overview of the Characters near the focus Character.
    /// </summary>
    public class NearbyPanel : Console
    {
        /// <summary>
        /// The Character whose location is used to determine other nearby Characters 
        /// for displaying in the Panel.
        /// </summary>
        public World.Character Focus { get; set; }
        // TODO :: Update the Viewer of all CharacterOverviews when the Focus is set.

        /// <summary>
        /// Constructs a new NearbyPanel.
        /// </summary>
        /// <param name="width">The width of the Panel.</param>
        /// <param name="height">The height of the Panel.</param>
        /// <param name="focus">The Character whose location is used to determine 
        /// other nearby Characters for displaying in the Panel.</param>
        public NearbyPanel(int width, int height, World.Character focus) : base(width, height)
        {
            Focus = focus;

            CharacterOverview co = new CharacterOverview(0, focus);
            Components.Add(co);
        }

        public override void Update(System.TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);
            
            List<World.Character> nearbyCharacters = World.CharactersNear(Focus,
                Display.MapViewPortHalfWidth, Display.MapViewPortHalfHeight);
            // TODO :: Use the nearby Characters to update the Components' Characters.
        }
    }
}
