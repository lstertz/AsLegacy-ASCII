using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

using AsLegacy.Global;
using AsLegacy.GUI.Screens;
using System;
using AsLegacy.Characters;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines the CharacterPanel aspect of a Display, 
    /// which is responsible for showing the desired view for character details.
    /// </summary>
    public class CharacterPanel : ControlsConsole
    {
        private readonly Ranking ranking;

        /// <summary>
        /// Constructs a new Character Panel, which defines all of the Panes 
        /// to show the details of the Player's Character.
        /// </summary>
        /// <param name="width">The width of the Panel.</param>
        /// <param name="height">The height of the Panel.</param>
        public CharacterPanel(int width, int height) : base(width, height)
        {
            ThemeColors = Colors.StandardTheme;

            Button items = new Button(7, 1)
            {
                Position = new Point(9, 1),
                Text = "Items"
            };
            items.Click += (s, e) => PlayScreen.ShowItems();
            Add(items);

            Button skills = new Button(8, 1)
            {
                Position = new Point(width - 16, 1),
                Text = "Skills"
            };
            skills.Click += (s, e) => PlayScreen.ShowSkills();
            Add(skills);

            ranking = new Ranking(8, true)
            {
                Position = new Point(1, 15)
            };
            Children.Add(ranking);
        }

        /// <summary>
        /// Forces a redraw of the CharacterPanel.
        /// </summary>
        public void Refresh()
        {
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            base.Invalidate();

            DrawTitle();
            DrawHealth();
        }

        /// <inheritdoc/>
        public override void Draw(TimeSpan update)
        {
            Refresh();
            base.Draw(update);
        }

        /// <summary>
        /// Draws the health stats onto the CharacterPanel.
        /// </summary>
        private void DrawHealth()
        {
            if (AsLegacy.Focus == null)
                return;

            string health;
            if (AsLegacy.Focus.CurrentHealth <= 0.0f)
                health = "0";
            else
                health = MathF.Ceiling(AsLegacy.Focus.CurrentHealth).ToString();

            string maxHealth = MathF.Ceiling(AsLegacy.Focus.MaxHealth).ToString();
            Print(1, 3, "Health: " + health + "/" + maxHealth, Color.White);
        }

        /// <summary>
        /// Draws the title (current focus Character's full name) onto the CharacterPanel.
        /// </summary>
        private void DrawTitle()
        {
            if (AsLegacy.Focus == null)
                return;

            string title;
            if (AsLegacy.Focus is ItemUser)
                title = (AsLegacy.Focus as ItemUser).FullName;
            else
                title = AsLegacy.Focus.Name;

            Print(Width / 2 - title.Length / 2, 0, title, Color.White);
        }
    }
}
