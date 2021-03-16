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
                Position = new Point(width - AvailablePointsMaxLength - 1, AvailablePointsY),
                TextColor = Colors.White
            };
            Add(_availablePointsLabel);
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            UpdateText();
            base.Invalidate();
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
                $"{AvailablePointsText}{AsLegacy.Player.AvailableSkillPoints}";
            _availablePointsLabel.IsDirty = true;
        }
    }
}
