using AsLegacy.Global;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the SkillsPopup, which displays information and 
    /// options for the Player to manage their Character's skills.
    /// </summary>
    public class SkillsPopup : Popup
    {
        private const string AvailablePointsText = "Available Points: ";
        private const int AvailablePointsY = 4;
        private const int AvailablePointsMaxLength = 25;

        private Label _availablePointsLabel;

        /// <summary>
        /// Constructs a new SkillsPopup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public SkillsPopup(int width, int height) : base("Skills", "", width, height)
        {
            _availablePointsLabel = new Label(AvailablePointsMaxLength)
            {
                Alignment = HorizontalAlignment.Right,
                DisplayText = $"{AvailablePointsText}0",
                Position = new Point(width - AvailablePointsMaxLength - 2, AvailablePointsY),
                TextColor = Colors.White
            };
            Add(_availablePointsLabel);
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            UpdateText();
            base.Invalidate();

            SetGlyph(0, 3, 199, Colors.White, Colors.Black);
            Fill(new Rectangle(1, 3, Width - 2, 1), Colors.White, Colors.Black, 196);
            SetGlyph(21, 3, 194, Colors.White, Colors.Black);
            Fill(new Rectangle(21, 4, 1, Height - 4), Colors.White, Colors.Black, 179);
            SetGlyph(21, Height - 1, 207, Colors.White, Colors.Black);
            SetGlyph(Width - 1, 3, 182, Colors.White, Colors.Black);

            Fill(new Rectangle(22, Height - 10, Width - 23, 1), Colors.White, Colors.Black, 45);

            string active = " Active ";
            Print(2, 3, active, Color.White);
            string text = " Concepts ";
            Print(23, 3, text, Color.White);
            string passives = " Passives ";
            Print(23, Height - 10, passives, Color.White);

            Print(2, 4, "Sample Skill");
            Print(23, 5, "Sample Skill Concept " + ((char)7) + " " + ((char)7));
            Print(Width - 6, 5, "## +");

            Print(23, Height - 9, "Health +0.0");
            Print(Width - 6, Height - 9, "## +");

            string successor = "Successor Points: ##";
            Print(Width - successor.Length - 2, Height - 2, successor);
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            if (IsVisible)
                UpdateText();
            IsFocused = IsVisible;
        }

        /// <summary>
        /// Updates the text display elements of the popup.
        /// </summary>
        private void UpdateText()
        {
            if (!AsLegacy.HasPlayer)
                return;

            _availablePointsLabel.DisplayText =
                $"{AvailablePointsText}{(int)AsLegacy.Player.AvailableSkillPoints}";
            _availablePointsLabel.IsDirty = true;
        }
    }
}
