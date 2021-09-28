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
        private const int EquippedSkillsStartingY = 10;
        private const int HealthY = 3;

        private static readonly string EmptySkillSlotGlyph = "-";

        private readonly Ranking _ranking;

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

            Button talents = new Button(9, 1)
            {
                Position = new Point(width - 16, 1),
                Text = "Talents"
            };
            talents.Click += (s, e) => PlayScreen.ShowTalents();
            Add(talents);

            _ranking = new Ranking(8, true)
            {
                Position = new Point(1, 15)
            };
            Children.Add(_ranking);
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

            if (AsLegacy.Focus == null)
                return;

            DrawTitle();
            DrawHealth();
            DrawEquippedSkills();
        }

        /// <inheritdoc/>
        public override void Draw(TimeSpan update)
        {
            Refresh();
            base.Draw(update);
        }

        /// <summary>
        /// Draws the equipped skills onto the CharacterPanel.
        /// </summary>
        private void DrawEquippedSkills()
        {
            Print(1, EquippedSkillsStartingY, "Equipped Skills", Color.White);
            string[] equippedSkills = AsLegacy.Focus.EquippedSkills;
            for (int c = 0, count = equippedSkills.Length, half = count / 2; c < count; c++)
            {
                string text = equippedSkills[c];
                if (string.IsNullOrEmpty(text))
                    text = EmptySkillSlotGlyph;

                if (c < half)
                    Print(1, EquippedSkillsStartingY + c + 1, $"{c + 1}: {text}", Color.White);
                else
                    Print(Width / 2 + 1, EquippedSkillsStartingY + (c - half) + 1, 
                        $"{c + 1}: {text}", Color.White);
            }
        }

        /// <summary>
        /// Draws the health stats onto the CharacterPanel.
        /// </summary>
        private void DrawHealth()
        {
            string health;
            if (AsLegacy.Focus.CurrentHealth <= 0.0f)
                health = "0";
            else
                health = MathF.Ceiling(AsLegacy.Focus.CurrentHealth).ToString();

            string maxHealth = MathF.Ceiling(AsLegacy.Focus.MaxHealth).ToString();
            Print(1, HealthY, $"Health: {health}/{maxHealth}", Color.White);
        }

        /// <summary>
        /// Draws the title (current focus Character's full name) onto the CharacterPanel.
        /// </summary>
        private void DrawTitle()
        {
            string title;
            if (AsLegacy.Focus is ItemUser)
                title = (AsLegacy.Focus as ItemUser).FullName;
            else
                title = AsLegacy.Focus.Name;

            Print(Width / 2 - title.Length / 2, 0, title, Color.White);
        }
    }
}
