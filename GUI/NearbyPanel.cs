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
        public World.Character Focus 
        { 
            get => focus; 
            set 
            { 
                focus = value;

                foreach (CharacterOverview co in Components)
                    co.Viewer = value;
            } 
        }
        private World.Character focus;

        /// <summary>
        /// Constructs a new NearbyPanel.
        /// </summary>
        /// <param name="width">The width of the Panel.</param>
        /// <param name="height">The height of the Panel.</param>
        /// <param name="focus">The Character whose location is used to determine 
        /// other nearby Characters for displaying in the Panel.</param>
        public NearbyPanel(int width, int height, World.Character focus) : base(width, height)
        {
            this.focus = focus;
        }

        /// <summary>
        /// Updates the CharacterOverviews of this NearbyPanel with the nearest Characters 
        /// of the Panel's focus Character.
        /// </summary>
        /// <param name="timeElapsed">The time since the last update.</param>
        public override void Update(System.TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);
            
            World.Character[] nearbyCharacters = World.CharactersNear(Focus,
                Display.MapViewPortHalfWidth, Display.MapViewPortHalfHeight);

            while (nearbyCharacters.Length > Components.Count)
                Components.Add(new CharacterOverview(
                    Components.Count * CharacterOverview.OverviewHeight, Focus));

            for (int c = 0, count = Components.Count; c < count; c++)
                if (c >= nearbyCharacters.Length)
                    (Components[c] as CharacterOverview).Character = null;
                else
                    (Components[c] as CharacterOverview).Character = nearbyCharacters[c];
        }
    }
}
