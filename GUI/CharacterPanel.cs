using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

using AsLegacy.Global;
using AsLegacy.GUI.Panes;
using AsLegacy.GUI.Screens;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines the CharacterPanel aspect of a Display, 
    /// which is responsible for showing the desired view for character details.
    /// </summary>
    public class CharacterPanel : ControlsConsole
    {
        /// <summary>
        /// Constructs a new Character Panel, which defines all of the Panes 
        /// to show the details of the Player's Character.
        /// </summary>
        /// <param name="width">The width of the Panel.</param>
        /// <param name="height">The height of the Panel.</param>
        public CharacterPanel(int width, int height) : base(width, height)
        {
            ThemeColors = Colors.StandardTheme;

            Button items = new Button(8, 1)
            {
                Position = new Point(5, 0),
                Text = "Items"
            };
            items.Click += (s, e) => PlayScreen.ShowItems();
            Add(items);

            Button skills = new Button(8, 1)
            {
                Position = new Point(width - 12, 0),
                Text = "Skills"
            };
            skills.Click += (s, e) => PlayScreen.ShowSkills();
            Add(skills);

            Children.Add(new StatsPane(width - 2, height)
            {
                Position = new Point(1, 0)
            });
        }
    }
}
