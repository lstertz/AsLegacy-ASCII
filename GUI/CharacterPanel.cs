using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

using AsLegacy.Global;
using AsLegacy.GUI.Panes;
using AsLegacy.GUI.Popups;
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
        /// Specifies which Pane is currently active, as is 
        /// identified by the Pane's index.
        /// </summary>
        public int CurrentPaneIndex
        {
            get { return currentPane; }
            set
            {
                panes[currentPane].IsVisible = false;
                currentPane = (value + panes.Length) % panes.Length;
                panes[currentPane].IsVisible = true;
            }
        }
        private int currentPane = 0;

        private readonly Pane[] panes = new Pane[2];

        private Popup skillsPopup;

        /// <summary>
        /// Constructs a new Character Panel, which defines all of the Panes 
        /// to show the details of the Player's Character.
        /// </summary>
        /// <param name="width">The width of the Panel.</param>
        /// <param name="height">The height of the Panel.</param>
        public CharacterPanel(int width, int height) : base(width, height)
        {
            ThemeColors = Colors.StandardTheme;

            Button left = new Button(1, 1)
            {
                Position = new Point(0, 0),
                Text = ((char)17).ToString()
            };
            left.Click += (s, e) => CurrentPaneIndex--;
            Add(left);

            Button right = new Button(1, 1)
            {
                Position = new Point(width - 1, 0),
                Text = ((char)16).ToString()
            };
            right.Click += (s, e) => CurrentPaneIndex++;
            Add(right);

            Button skills = new Button(8, 1)
            {
                Position = new Point(width - 12, 0),
                Text = "Skills"
            };
            skills.Click += (s, e) => PlayScreen.ShowSkills();
            Add(skills);

            panes[0] = new StatsPane(width - 2, height)
            {
                Position = new Point(1, 0)
            };
            Children.Add(panes[0]);

            panes[1] = new Pane("Items", "Items to be managed here.", width - 2, height)
            {
                Position = new Point(1, 0)
            };
            Children.Add(panes[1]);
            panes[1].IsVisible = false;
        }
    }
}
