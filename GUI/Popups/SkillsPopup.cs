using AsLegacy.Characters;
using AsLegacy.Global;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the SkillsPopup, which displays information and 
    /// options for the Player to manage their Character's skills.
    /// </summary>
    public class SkillsPopup : Popup
    {
        private readonly string AffinityIcon = char.ToString((char)4);
        private const string AvailablePointsText = "Available Points: ";
        private const int AvailablePointsY = 4;
        private const int AvailablePointsMaxLength = 25;

        private const int MaxConceptCount = 7;
        private const int MaxPassiveCount = 7;

        private const int TalentNameX = 23;

        private readonly Label _availablePointsLabel;
        private readonly Button[] _conceptInvestmentButtons = new Button[MaxConceptCount];
        private readonly List<Label> _conceptAffinityLabels = new();
        private readonly Button[] _passiveInvestmentButtons = new Button[MaxPassiveCount];

        private DynamicContentPopup _hoverPopup;
        private int _hoveredPassiveIndex = -1;

        /// <summary>
        /// Constructs a new SkillsPopup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public SkillsPopup(int width, int height) : base("Skills", width, height)
        {
            _hoverPopup = new DynamicContentPopup("", 10, 40, 20)
            {
                IsVisible = false
            };
            Children.Add(_hoverPopup);

            _availablePointsLabel = new Label(AvailablePointsMaxLength)
            {
                Alignment = HorizontalAlignment.Right,
                DisplayText = $"{AvailablePointsText}0",
                Position = new Point(width - AvailablePointsMaxLength - 2, AvailablePointsY),
                TextColor = Colors.White
            };
            Add(_availablePointsLabel);

            for (int c = 0; c < MaxConceptCount; c++)
            {
                int conceptIndex = c;
                Button button = new Button(1, 1)
                {
                    IsEnabled = false,
                    Position = new Point(width - 3, 5 + c),
                    Text = "+"
                };
                //button.Click += (sender, args) => OnPassiveInvestment(sender, args, conceptIndex);
                //button.MouseEnter += (sender, args) => OnHoverBegin(sender, args, conceptIndex);
                //button.MouseExit += OnHoverEnd;

                Add(button);
                _conceptInvestmentButtons[c] = button;
            }

            for (int c = 0; c < MaxPassiveCount; c++)
            {
                int passiveIndex = c;
                Button button = new Button(1, 1)
                {
                    IsEnabled = true,
                    Position = new Point(width - 3, Height - 9 + c),
                    Text = "+"
                };
                button.Click += (sender, args) => OnPassiveInvestment(sender, args, passiveIndex);
                button.MouseEnter += (sender, args) => OnHoverPassiveBegin(sender, args, passiveIndex);
                button.MouseExit += OnHoverEnd;

                Add(button);
                _passiveInvestmentButtons[c] = button;
            }
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            base.Invalidate();

            DrawFrame();
            UpdateContent();

            Print(2, 4, "Sample Skill");

            string successor = "Successor Points: ##";
            Print(Width - successor.Length - 2, Height - 2, successor);
        }

        /// <summary>
        /// Handles the start of the hovering of an investment button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        /// <param name="passiveIndex">The index of the passive skill whose 
        /// investment button is being hovered.</param>
        private void OnHoverPassiveBegin(object sender, MouseEventArgs args, int passiveIndex)
        {
            _hoveredPassiveIndex = passiveIndex;
            UpdateHoverPassiveContent(sender as Button);

            _hoverPopup.IsVisible = true;
            _hoverPopup.IsDirty = true;
        }

        /// <summary>
        /// Handles the start of the hovering of an affinity icon.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        /// <param name="conceptIndex">The index of the concept whose 
        /// affinity icon is being hovered.</param>
        /// <param name="affinityIndex">The index of the affinity icon, within its 
        /// concept's collection of affinities, that is being hovered.</param>
        private void OnHoverAffinityBegin(object sender, MouseEventArgs args, 
            int conceptIndex, int affinityIndex)
        {
            Affinity affinity = AsLegacy.Player.Class.Concepts[conceptIndex].Affinities[affinityIndex];

            _hoverPopup.UpdateTitle(affinity.Name);
            _hoverPopup.UpdateContent(affinity.GetDescription(AsLegacy.Player));
            _hoverPopup.Position = (sender as Label).Position -
                new Point(_hoverPopup.Width, 0);

            _hoverPopup.IsVisible = true;
            _hoverPopup.IsDirty = true;
        }

        /// <summary>
        /// Handles the end of the hovering of a button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnHoverEnd(object sender, MouseEventArgs args)
        {
            _hoveredPassiveIndex = -1;
            _hoverPopup.IsVisible = false;
        }

        /// <summary>
        /// Handles the investment of a passive skill.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event args.</param>
        /// <param name="passiveIndex">The index of he passive skill to be invested in.</param>
        private void OnPassiveInvestment(object sender, System.EventArgs args, int passiveIndex)
        {
            if (!AsLegacy.HasPlayer)
                return;

            // TODO :: Support various investment amounts (eg. 1, 5, 10, etc.).
            AsLegacy.Player.InvestInTalent(AsLegacy.Player.Class.Passives[passiveIndex], 1);
            UpdateHoverPassiveContent(sender as Button);
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            if (AsLegacy.HasPlayer && IsVisible)
            {
                UpdateContent();

                ReadOnlyCollection<Concept> concepts = AsLegacy.Player.Class.Concepts;
                int conceptCount = concepts.Count;
                for (int c = 0; c < MaxConceptCount; c++)
                {
                    bool hasConcept = c < conceptCount;
                    _conceptInvestmentButtons[c].IsVisible = hasConcept;

                    if (hasConcept)
                    {
                        int conceptIndex = c;
                        for (int cc = 0; cc < concepts[c].Affinities.Count; cc++)
                        {
                            int affinityIndex = cc;
                            Affinity affinity = concepts[c].Affinities[cc];
                            Label affinityLabel = new(1)
                            {
                                DisplayText = AffinityIcon,
                                Position = new(TalentNameX + concepts[c].Name.Length + 1, 5 + c),
                                TextColor = affinity.AffectColor
                            };
                            affinityLabel.MouseEnter += (sender, args) => OnHoverAffinityBegin(
                                sender, args, conceptIndex, affinityIndex);
                            affinityLabel.MouseExit += OnHoverEnd;

                            _conceptAffinityLabels.Add(affinityLabel);
                            Add(affinityLabel);
                        }
                    }
                }

                int passiveCount = AsLegacy.Player.Class.Passives.Count;
                for (int c = 0; c < MaxPassiveCount; c++)
                    _passiveInvestmentButtons[c].IsVisible = c < passiveCount;
            }
            else
            {
                for (int c = 0; c < MaxConceptCount; c++)
                {
                    _conceptInvestmentButtons[c].IsVisible = false;

                    for (int cc = 0; cc < _conceptAffinityLabels.Count; cc++)
                        Remove(_conceptAffinityLabels[cc]);
                    _conceptAffinityLabels.Clear();
                }
                for (int c = 0; c < MaxPassiveCount; c++)
                    _passiveInvestmentButtons[c].IsVisible = false;
            }

            IsFocused = IsVisible;
        }


        /// <summary>
        /// Draws the frame (divisions) within the popup.
        /// </summary>
        private void DrawFrame()
        {
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
            Print(TalentNameX, 3, text, Color.White);
            string passives = " Passives ";
            Print(TalentNameX, Height - 10, passives, Color.White);
        }

        /// <summary>
        /// Updates the text and text display elements of the popup.
        /// </summary>
        private void UpdateContent()
        {
            if (!AsLegacy.HasPlayer)
                return;

            UpdateAvailablePoints();
            UpdateConcepts();
            UpdatePassives();
        }

        /// <summary>
        /// Updates the content of the investment hover pop-up, if it is active.
        /// </summary>
        private void UpdateHoverPassiveContent(Button hoveredButton)
        {
            // TODO :: Abstract to any Talent content?

            if (hoveredButton == null)
                return;

            Passive passive = AsLegacy.Player.Class.Passives[_hoveredPassiveIndex];

            int investment = AsLegacy.Player.GetInvestment(passive);
            string content = $"{passive.GetDescription(AsLegacy.Player)}\n" +
                passive.GetDifferenceDescription(investment, investment + 1) + 
                " for 1 point";

            _hoverPopup.UpdateTitle(passive.Name);
            _hoverPopup.UpdateContent(content);
            _hoverPopup.Position = hoveredButton.Position -
                new Point(_hoverPopup.Width, _hoverPopup.Height);
        }


        /// <summary>
        /// Updates the available points label of the popup.
        /// </summary>
        private void UpdateAvailablePoints()
        {
            _availablePointsLabel.DisplayText =
                $"{AvailablePointsText}{(int)AsLegacy.Player.AvailableSkillPoints}";
            _availablePointsLabel.IsDirty = true;
        }

        /// <summary>
        /// Updates the text of the concepts' descriptions in the popup.
        /// </summary>
        private void UpdateConcepts()
        {
            ReadOnlyCollection<Concept> concepts = AsLegacy.Player.Class.Concepts;
            for (int c = 0, count = concepts.Count, y = 5; c < count; c++, y++)
            {
                string investment = "33";// AsLegacy.Player.GetInvestment(concepts[c]).ToString();

                Print(TalentNameX, y, concepts[c].Name);
                Print(Width - investment.Length - 4, y, investment);
            }
        }

        /// <summary>
        /// Updates the text of the passives' descriptions in the popup.
        /// </summary>
        private void UpdatePassives()
        {
            ReadOnlyCollection<Passive> passives = AsLegacy.Player.Class.Passives;
            for (int c = 0, count = passives.Count, y = Height - 9; c < count; c++, y++)
            {
                string investment = AsLegacy.Player.GetInvestment(passives[c]).ToString();

                Print(TalentNameX, y, passives[c].Name);
                Print(Width - investment.Length - 4, y, investment);
            }
        }
    }
}
