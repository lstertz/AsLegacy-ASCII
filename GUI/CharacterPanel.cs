using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using SadConsole.Controls;
using SadConsole.Themes;

namespace AsLegacy
{
    public partial class Display : DrawConsoleComponent
    {
        /// <summary>
        /// Defines the CharacterPanel aspect of a Display, 
        /// which is responsible for showing the desired view for character details.
        /// </summary>
        public class CharacterPanel : ControlsConsole
        {
            private static readonly Color fadedWhite = new Color(255, 255, 255, 235);

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

            private Pane[] panes = new Pane[3];

            /// <summary>
            /// Constructs a new Character Panel, which defines all of the Panes 
            /// to show the details of the Player's Character.
            /// </summary>
            /// <param name="width">The width of the Panel.</param>
            /// <param name="height">The height of the Panel.</param>
            public CharacterPanel(int width, int height) : base(width, height)
            {
                Colors colors = Colors.CreateDefault();
                colors.ControlBack = Color.Transparent;
                colors.Text = fadedWhite;
                colors.TextDark = Color.White;
                colors.TextLight = Color.White;
                colors.TextFocused = fadedWhite;
                colors.TextSelected = Color.White;
                colors.TextSelectedDark = Color.White;
                colors.ControlBackLight = Color.Transparent;
                colors.ControlBackSelected = Color.Transparent;
                colors.ControlBackDark = Color.Transparent;
                colors.RebuildAppearances();
                ThemeColors = colors;

                Button left = new Button(1, 1)
                {
                    Position = new Point(0, 0),
                    Text = ((char)17).ToString(),
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

                panes[0] = new Pane("Stats", "Character data to be shown here.", width - 2, height);
                panes[0].Position = new Point(1, 0);
                Children.Add(panes[0]);

                panes[1] = new Pane("Skills", "Skills to be managed here.", width - 2, height);
                panes[1].Position = new Point(1, 0);
                Children.Add(panes[1]);
                panes[1].IsVisible = false;

                panes[2] = new Pane("Items", "Items to be managed here.", width - 2, height);
                panes[2].Position = new Point(1, 0);
                Children.Add(panes[2]);
                panes[2].IsVisible = false;
            }
        }
    }
}
