using AsLegacy.Characters;
using AsLegacy.Characters.Skills;
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
    /// Defines the TalentsPopup, which displays information and 
    /// options for the Player to manage their Character's skills.
    /// </summary>
    public class TalentsPopup : Popup
    {
        private readonly string AffinityIcon = char.ToString((char)4);
        private const string AvailablePointsText = "Available Points: ";
        private const int AvailablePointsY = 4;

        private const string SuccessorPointsText = "Successor Points: ";
        private const int PointsMaxLength = 25;

        private const int MaxConceptCount = 7;
        private const int MaxPassiveCount = 7;

        private const int TalentNameX = 23;

        private readonly Label _availablePointsLabel;
        private readonly Button[] _conceptInvestmentButtons = new Button[MaxConceptCount];
        private readonly List<Button> _conceptAffinityButtons = new();
        private readonly Button[] _passiveInvestmentButtons = new Button[MaxPassiveCount];
        private readonly Label _successorPointsLabel;

        private DynamicContentPopup _hoverPopup;
        private Button _hoveredButton = null;
        private int _hoveredInvestmentIndex = -1;

        private ConfirmationPopup _learnSkillPopup;

        /// <summary>
        /// Constructs a new TalentsPopup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public TalentsPopup(int width, int height) : base("Talents", width, height)
        {
            _hoverPopup = new("", 10, 40, 20)
            {
                IsVisible = false
            };
            Children.Add(_hoverPopup);

            _learnSkillPopup = new("Learn Skill", "", 40, 7)
            {
                Position = new(width / 2 - 20, height / 2 - 3),
                IsVisible = false
            };
            Children.Add(_learnSkillPopup);

            _availablePointsLabel = new Label(PointsMaxLength)
            {
                Alignment = HorizontalAlignment.Right,
                DisplayText = $"{AvailablePointsText}0",
                Position = new(width - PointsMaxLength - 2, AvailablePointsY),
                TextColor = Colors.White
            };
            Add(_availablePointsLabel);

            _successorPointsLabel = new Label(PointsMaxLength)
            {
                Alignment = HorizontalAlignment.Right,
                DisplayText = $"{SuccessorPointsText}0",
                Position = new(width - PointsMaxLength - 2, Height - 2),
                TextColor = Colors.White
            };
            Add(_successorPointsLabel);

            for (int c = 0; c < MaxConceptCount; c++)
            {
                int index = c;
                Button button = new(1, 1)
                {
                    CanFocus = false,
                    IsEnabled = true,
                    Position = new(width - 3, 5 + c),
                    Text = "+"
                };
                button.Click += (sender, args) => OnInvestment(sender, args, index);
                button.MouseEnter += (sender, args) => OnHoverBegin(sender, args, index);
                button.MouseExit += OnHoverEnd;

                Add(button);
                _conceptInvestmentButtons[c] = button;
            }

            for (int c = 0; c < MaxPassiveCount; c++)
            {
                int index = c + MaxConceptCount;
                Button button = new(1, 1)
                {
                    CanFocus = false,
                    IsEnabled = true,
                    Position = new(width - 3, Height - 9 + c),
                    Text = "+"
                };
                button.Click += (sender, args) => OnInvestment(sender, args, index);
                button.MouseEnter += (sender, args) => OnHoverBegin(sender, args, index);
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
        }

        /// <inheritdoc/>
        public override bool ProcessMouse(MouseConsoleState state)
        {
            if (_learnSkillPopup.IsVisible)
                return true;

            return base.ProcessMouse(state);
        }

        /// <summary>
        /// Handles the clicking of an affinity button.
        /// </summary>
        /// <param name="sender">The event sender (the affinity button).</param>
        /// <param name="args">The event arguments.</param>
        /// <param name="conceptIndex">The index of the concept whose 
        /// affinity icon is being clicked.</param>
        /// <param name="affinityIndex">The index of the affinity icon, within its 
        /// concept's collection of affinities, that is being clicked.</param>
        private void OnClickAffinity(object sender, MouseEventArgs args, 
            int conceptIndex, int affinityIndex)
        {
            Affinity affinity = AsLegacy.Player.Class.Concepts[conceptIndex].Affinities[affinityIndex];
            _learnSkillPopup.Prompt = $"Learn the {affinity.Name} skill?";
            _learnSkillPopup.OnConfirmation = () => 
            {
                Skill skill = new()
                {
                    Affinity = affinity,
                    Concept = AsLegacy.Player.Class.Concepts[conceptIndex]
                };
                AsLegacy.Player.LearnSkill(skill);
                Invalidate();

                (sender as Button).IsFocused = false; 
            };
            _learnSkillPopup.OnRejection = () => 
            { 
                (sender as Button).IsFocused = false; 
            };
            _learnSkillPopup.IsVisible = true;

            _hoverPopup.IsVisible = false;
        }

        /// <summary>
        /// Handles the start of the hovering of an investment button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        /// <param name="index">The index of the talent whose 
        /// investment button is being hovered.</param>
        private void OnHoverBegin(object sender, MouseEventArgs args, int index)
        {
            _hoveredButton = sender as Button;
            _hoveredInvestmentIndex = index;
            
            UpdateHoverContent();

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
            _hoverPopup.Position = (sender as Button).Position -
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
            if (sender != _hoveredButton)
                return;

            _hoveredButton = null;
            _hoveredInvestmentIndex = -1;
            _hoverPopup.IsVisible = false;
        }

        /// <summary>
        /// Handles the investment of a talent.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event args.</param>
        /// <param name="index">The index of the talent to be invested in.</param>
        private void OnInvestment(object sender, System.EventArgs args, int index)
        {
            if (!AsLegacy.HasPlayer)
                return;

            // TODO :: Support various investment amounts (eg. 1, 5, 10, etc.).
            Talent talent;
            if (index >= MaxConceptCount)
                talent = AsLegacy.Player.Class.Passives[index - MaxConceptCount];
            else
                talent = AsLegacy.Player.Class.Concepts[index];

            if (AsLegacy.Player.InvestInTalent(talent, 1))
                UpdateHoverContent();
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

                            SadConsole.Themes.Colors colors = Colors.StandardTheme;
                            colors.Text = affinity.AffectColor;
                            colors.RebuildAppearances();

                            Button affinityButton = new(1)
                            {
                                Text = AffinityIcon,
                                Position = new(TalentNameX + concepts[c].Name.Length + 1, 5 + c),
                                ThemeColors = colors
                            };
                            affinityButton.MouseEnter += (sender, args) => OnHoverAffinityBegin(
                                sender, args, conceptIndex, affinityIndex);
                            affinityButton.MouseButtonClicked += (sender, args) => OnClickAffinity(
                                sender, args, conceptIndex, affinityIndex);
                            affinityButton.MouseExit += OnHoverEnd;

                            _conceptAffinityButtons.Add(affinityButton);
                            Add(affinityButton);
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

                    for (int cc = 0; cc < _conceptAffinityButtons.Count; cc++)
                        Remove(_conceptAffinityButtons[cc]);
                    _conceptAffinityButtons.Clear();
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

            string active = " Skills ";
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
            UpdateActiveSkills();
            UpdateSuccessorPoints();
        }

        /// <summary>
        /// Updates the content of the investment hover pop-up, if it is active.
        /// </summary>
        private void UpdateHoverContent()
        {
            if (_hoveredButton == null || _hoveredInvestmentIndex == -1)
                return;

            bool isPassive = _hoveredInvestmentIndex >= MaxConceptCount;
            Talent talent;
            if (isPassive)
                talent = AsLegacy.Player.Class.Passives[_hoveredInvestmentIndex - MaxConceptCount];
            else
                talent = AsLegacy.Player.Class.Concepts[_hoveredInvestmentIndex];

            int investment = AsLegacy.Player.GetInvestment(talent);
            string content = $"{talent.GetDescription(AsLegacy.Player)}\n" +
                talent.GetDifferenceDescription(investment, investment + 1) +
                " for 1 point.";

            _hoverPopup.UpdateTitle(talent.Name);
            _hoverPopup.UpdateContent(content);
            _hoverPopup.Position = _hoveredButton.Position -
                new Point(_hoverPopup.Width, isPassive ? _hoverPopup.Height : 0);
        }


        /// <summary>
        /// Updates the list of the active skills to be displayed in the popup.
        /// </summary>
        private void UpdateActiveSkills()
        {
            string[] skillNames = AsLegacy.Player.SkillNames;
            for (int c = 0, count = skillNames.Length; c < count; c++)
                Print(2, 4 + c, skillNames[c]);
        }

        /// <summary>
        /// Updates the available points label of the popup.
        /// </summary>
        private void UpdateAvailablePoints()
        {
            _availablePointsLabel.DisplayText =
                $"{AvailablePointsText}{AsLegacy.Player.AvailableSkillPoints}";
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
                string investment = AsLegacy.Player.GetInvestment(concepts[c]).ToString();

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

        /// <summary>
        /// Updates the successor points label of the popup.
        /// </summary>
        private void UpdateSuccessorPoints()
        {
            _successorPointsLabel.DisplayText =
                $"{SuccessorPointsText}{AsLegacy.Player.CharacterLineage.SuccessorPoints}";
            _successorPointsLabel.IsDirty = true;
        }
    }
}
