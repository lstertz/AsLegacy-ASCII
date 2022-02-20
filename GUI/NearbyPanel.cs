using SadConsole;

using AsLegacy.GUI.Elements;
using AsLegacy.GUI.Screens;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines the NearbyPanel aspect of a Display, 
    /// which is responsible for showing an overview of the Characters near the focus Character.
    /// </summary>
    public class NearbyPanel : Console
    {
        /// <summary>
        /// Constructs a new NearbyPanel.
        /// </summary>
        /// <param name="width">The width of the Panel.</param>
        /// <param name="height">The height of the Panel.</param>
        /// <param name="focus">The Character whose location is used to determine 
        /// other nearby Characters for displaying in the Panel.</param>
        public NearbyPanel(int width, int height) : base(width, height)
        {
            while (PlayScreen.MapViewPortHeight / CharacterOverview.Height > Components.Count)
                Components.Add(new CharacterOverview(
                    Components.Count * CharacterOverview.Height));
        }

        /// <summary>
        /// Updates the CharacterOverviews of this NearbyPanel with the nearest Characters 
        /// of the Panel's focus Character.
        /// </summary>
        /// <param name="timeElapsed">The time since the last update.</param>
        public override void Update(System.TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);
            
            // TODO :: If no focus, support showing characters closest to current center of view.

            World.Character[] nearbyCharacters = World.CharactersNear(GameExecution.Focus,
                PlayScreen.MapViewPortHalfWidth, PlayScreen.MapViewPortHalfHeight);

            while (nearbyCharacters.Length > Components.Count)
                Components.Add(new CharacterOverview(
                    Components.Count * CharacterOverview.Height, GameExecution.Focus));

            for (int c = 0, count = Components.Count; c < count; c++)
                if (c >= nearbyCharacters.Length)
                    (Components[c] as CharacterOverview).Character = null;
                else
                    (Components[c] as CharacterOverview).Character = nearbyCharacters[c];
        }
    }
}
