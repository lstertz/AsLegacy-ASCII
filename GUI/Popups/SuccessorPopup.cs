using AsLegacy.Characters;
using AsLegacy.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System;
using System.Collections.ObjectModel;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the SuccessorPopup, which displays information and 
    /// options for the Player to define details about a successor.
    /// </summary>
    public class SuccessorPopup : Popup
    {
        private const string AvailablePointsText = "Available Points: ";
        private const int AvailablePointsY = 3;
        private const int PointsMaxLength = 25;

        private const int MaxPassiveCount = 7;
        private const int MaxAssignedPointsLength = 3;

        private const int PassiveNameX = 2;

        /// <summary>
        /// The name of the successor.
        /// </summary>
        public string SuccessorName { get; set; }

        /// <inheritdoc/>
        protected override string Title
        {
            get
            {
                return $"What are {SuccessorName}'s Passives?";
            }
        }

        private readonly Button _ok;
        private readonly Label _availablePointsLabel;
        private readonly TextBox[] _passiveInvestmentBoxes = new TextBox[MaxPassiveCount];
        private int _availablePoints;

        private DynamicContentPopup _hoverPopup;
        private TextBox _hoveredBox = null;
        private int _hoveredInvestmentIndex = -1;

        /// <summary>
        /// Constructs a new <see cref="SuccessorPopup"/>.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public SuccessorPopup(int width, int height) : base("", width, height, false)
        {
            _availablePointsLabel = new Label(PointsMaxLength)
            {
                Alignment = HorizontalAlignment.Right,
                DisplayText = $"{AvailablePointsText}0",
                Position = new(width - PointsMaxLength - 2, AvailablePointsY),
                TextColor = Colors.White
            };
            Add(_availablePointsLabel);

            for (int c = 0; c < MaxPassiveCount; c++)
            {
                int index = c;
                TextBox box = new(MaxAssignedPointsLength + 1)
                {
                    AllowDecimal = false,
                    CanFocus = true,
                    IsEnabled = true,
                    IsNumeric = true,
                    MaxLength = MaxAssignedPointsLength,
                    Position = new(width - 3 - MaxAssignedPointsLength, AvailablePointsY + 1 + c),
                    Text = "",
                    TextAlignment = HorizontalAlignment.Right
                };
                box.IsDirtyChanged += (sender, args) => OnInvestmentChanged(sender as TextBox);
                box.MouseEnter += (sender, args) => OnHoverInvestmentBegin(sender, args, index);
                box.MouseExit += OnHoverInvestmentEnd;
                box.TextChangedPreview += (sender, args) =>
                    OnInvestmentChangedFinal(sender as TextBox, args);

                Add(box);
                _passiveInvestmentBoxes[c] = box;
            }

            _ok = new Button(2, 1)
            {
                IsEnabled = false,
                Position = new Point(width / 2 - 1, height - 2),
                Text = "Ok"
            };
            _ok.Click += (s, e) =>
            {
                IsVisible = false;
                int[] initialInvestments = new int[MaxPassiveCount];
                for (int c = 0; c < MaxPassiveCount; c++)
                {
                    if (_passiveInvestmentBoxes[c].EditingText != "")
                        initialInvestments[c] = Convert.ToInt32(
                            _passiveInvestmentBoxes[c].EditingText);
                }

                Player.CreateSuccessor(SuccessorName, initialInvestments);
            };
            Add(_ok);

            _hoverPopup = new("", 10, 40, 20)
            {
                IsVisible = false
            };
            Children.Add(_hoverPopup);
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            base.Invalidate();
            UpdatePassiveLabels();
        }

        /// <summary>
        /// Handles the start of the hovering of an investment text box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        /// <param name="index">The index of the talent whose 
        /// investment text box is being hovered.</param>
        private void OnHoverInvestmentBegin(object sender, MouseEventArgs args, int index)
        {
            _hoveredBox = sender as TextBox;
            _hoveredInvestmentIndex = index;

            UpdateHoverContent();

            _hoverPopup.IsVisible = true;
            _hoverPopup.IsDirty = true;
        }

        /// <summary>
        /// Handles the end of the focusing of an investment text box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnHoverInvestmentEnd(object sender, MouseEventArgs args)
        {
            if (sender != _hoveredBox)
                return;

            _hoveredBox = null;
            _hoveredInvestmentIndex = -1;
            _hoverPopup.IsVisible = false;
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            if (IsVisible)
            {
                int passiveCount = AsLegacy.Player.Class.Passives.Count;
                _availablePoints = Player.Character.CharacterLineage.SuccessorPoints;
                for (int c = 0; c < MaxPassiveCount; c++)
                {
                    bool isVisible = c < passiveCount;
                    if (_passiveInvestmentBoxes[c].EditingText != "" && isVisible)
                        _availablePoints -= Convert.ToInt32(
                            _passiveInvestmentBoxes[c].EditingText);

                    _passiveInvestmentBoxes[c].IsVisible = isVisible;
                }

                UpdateAvailablePointsLabel();
                _ok.IsEnabled = _availablePoints == 0;
            }

            IsFocused = IsVisible;
        }

        /// <summary>
        /// Updates the available points, permitting negative values, and the Ok button
        /// for a change in point investment.
        /// </summary>
        private void OnInvestmentChanged(TextBox investmentBox)
        {
            if (Player.Character == null || !investmentBox.IsFocused)
                return;

            int spentPoints = 0;
            for (int c = 0, count = _passiveInvestmentBoxes.Length; c < count; c++)
            {
                if (_passiveInvestmentBoxes[c].EditingText == "")
                    continue;
                spentPoints += Convert.ToInt32(_passiveInvestmentBoxes[c].EditingText);
            }

            _availablePoints = Player.Character.CharacterLineage.SuccessorPoints - spentPoints;
            UpdateAvailablePointsLabel();
            UpdateHoverContent();

            _ok.IsEnabled = _availablePoints == 0;
        }

        /// <summary>
        /// Updates the available points and Ok button for a change in point investment.
        /// </summary>
        private void OnInvestmentChangedFinal(TextBox investmentBox,
            TextBox.TextChangedEventArgs args)
        {
            if (Player.Character == null || !investmentBox.IsFocused)
                return;

            int spentPoints = 0;
            for (int c = 0, count = _passiveInvestmentBoxes.Length; c < count; c++)
            {
                if (_passiveInvestmentBoxes[c].EditingText == "")
                    continue;
                spentPoints += Convert.ToInt32(_passiveInvestmentBoxes[c].EditingText);
            }

            _availablePoints = Player.Character.CharacterLineage.SuccessorPoints - spentPoints;
            if (_availablePoints < 0)
            {
                args.NewValue = (Convert.ToInt32(investmentBox.EditingText) +
                    _availablePoints).ToString();
                _availablePoints = 0;
            }
            UpdateAvailablePointsLabel();

            _ok.IsEnabled = _availablePoints == 0;
        }

        /// <summary>
        /// Updates the label for the available successor points.
        /// </summary>
        private void UpdateAvailablePointsLabel()
        {
            _availablePointsLabel.DisplayText =
                $"{AvailablePointsText}{_availablePoints}";
            _availablePointsLabel.IsDirty = true;
        }

        /// <summary>
        /// Updates the content of the investment hover pop-up, if it is active.
        /// </summary>
        private void UpdateHoverContent()
        {
            if (_hoveredBox == null || _hoveredInvestmentIndex == -1)
                return;

            Talent talent = AsLegacy.Player.Class.Passives[_hoveredInvestmentIndex];

            int investment = 0;
            string investmentText = _passiveInvestmentBoxes[_hoveredInvestmentIndex].EditingText;
            if (investmentText != "")
                investment = Convert.ToInt32(investmentText);

            string content = $"{talent.GetDescription(investment)}\n" +
                talent.GetDifferenceDescription(investment, investment + 1) +
                " for the next point.";

            _hoverPopup.UpdateTitle(talent.Name);
            _hoverPopup.UpdateContent(content);
            _hoverPopup.Position = _hoveredBox.Position -
                new Point(_hoverPopup.Width, _hoverPopup.Height / 2);
        }

        /// <summary>
        /// Updates the text of the passives' descriptions in the popup.
        /// </summary>
        private void UpdatePassiveLabels()
        {
            if (!AsLegacy.HasPlayer)
                return;

            // TODO : 85 :: Use locally defined class reference.
            ReadOnlyCollection<Passive> passives = AsLegacy.Player.Class.Passives;
            for (int c = 0, count = passives.Count, y = AvailablePointsY + 1; c < count; c++, y++)
                Print(PassiveNameX, y, passives[c].Name);
        }
    }
}
