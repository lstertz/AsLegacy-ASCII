﻿using SadConsole;
using SadConsole.Controls;

using Colors = AsLegacy.Global.Colors;
using AsLegacy.GUI.Panes;

using Microsoft.Xna.Framework;

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

        private readonly Pane[] panes = new Pane[3];

        /// <summary>
        /// Constructs a new Character Panel, which defines all of the Panes 
        /// to show the details of the Player's Character.
        /// </summary>
        /// <param name="width">The width of the Panel.</param>
        /// <param name="height">The height of the Panel.</param>
        public CharacterPanel(int width, int height) : base(width, height)
        {
            SadConsole.Themes.Colors colors = SadConsole.Themes.Colors.CreateDefault();
            colors.ControlBack = Colors.Transparent;
            colors.Text = Colors.FadedWhite;
            colors.TextDark = Colors.White;
            colors.TextLight = Colors.White;
            colors.TextFocused = Colors.FadedWhite;
            colors.TextSelected = Colors.White;
            colors.TextSelectedDark = Colors.White;
            colors.ControlBackLight = Colors.Transparent;
            colors.ControlBackSelected = Colors.Transparent;
            colors.ControlBackDark = Colors.Transparent;
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

            panes[0] = new StatsPane(width - 2, height)
            {
                Position = new Point(1, 0)
            };
            Children.Add(panes[0]);

            panes[1] = new Pane("Skills", "Skills to be managed here.", width - 2, height)
            {
                Position = new Point(1, 0)
            };
            Children.Add(panes[1]);
            panes[1].IsVisible = false;

            panes[2] = new Pane("Items", "Items to be managed here.", width - 2, height)
            {
                Position = new Point(1, 0)
            };
            Children.Add(panes[2]);
            panes[2].IsVisible = false;
        }
    }
}
