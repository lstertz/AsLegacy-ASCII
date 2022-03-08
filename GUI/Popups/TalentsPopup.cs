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
        private const int ApplyY = 5;
        private const string AvailablePointsText = "Available Points: ";
        private const int AvailablePointsY = 4;
        private const int ConceptsStartingY = 6;

        private const string SuccessorPointsText = "Successor Points: ";
        private const int PointsMaxLength = 25;

        private const int MaxConceptCount = 7;
        private const int MaxPassiveCount = 7;

        private const int TalentNameX = 23;

        private const string HelpText = "Talents are concepts and passives that a character " +
            "can invest in, by using the + and - buttons, with talent points. Any investments " +
            "must be applied to take effect, and cannot be undone once applied. Any damage " +
            "dealing action will earn a character a talent point. A passive grants the benefit " +
            "of its investment to the character at all times. A concept imparts the benefit of " +
            "its investment to any skills that are defined by the concept. Any skill can be " +
            "learned by clicking its diamond next to its primary concept. Once learned, the " +
            "skill can be equipped and used.";

        private readonly Button _applyInvestmentsButton;
        private readonly Label _availablePointsLabel;
        private readonly Button[] _conceptDivestmentButtons = new Button[MaxConceptCount];
        private readonly Button[] _conceptInvestmentButtons = new Button[MaxConceptCount];
        private readonly List<Button> _conceptAffinityButtons = new();
        private readonly Button[] _passiveDivestmentButtons = new Button[MaxPassiveCount];
        private readonly Button[] _passiveInvestmentButtons = new Button[MaxPassiveCount];
        private readonly Label _successorPointsLabel;

        private World.Character.Projection _projection;
        private float _pendingSuccessorPoints = 0.0f;

        private readonly DynamicContentPopup _hoverPopup;
        private Button _hoveredButton = null;
        private int _hoveredInvestmentIndex = -1;

        private readonly ConfirmationPopup _learnSkillPopup;
        private readonly NotificationPopup _notificationPopup;


        /// <summary>
        /// Constructs a new TalentsPopup.
        /// </summary>
        /// <param name="width">The width of the Popup window.</param>
        /// <param name="height">The height of the Popup window.</param>
        public TalentsPopup(int width, int height) : 
            base("Talents", width, height, true, HelpText)
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

            _notificationPopup = new("", 10, 40, 20)
            {
                Position = new(width / 2 - 20, height / 2 - 3),
                IsVisible = false
            };
            Children.Add(_notificationPopup);

            _availablePointsLabel = new Label(PointsMaxLength)
            {
                Alignment = HorizontalAlignment.Right,
                DisplayText = $"{AvailablePointsText}0",
                Position = new(width - PointsMaxLength - 2, AvailablePointsY),
                TextColor = Colors.White
            };
            Add(_availablePointsLabel);

            _applyInvestmentsButton = new Button(7)
            {
                IsEnabled = false,
                Position = new(width - 8, ApplyY),
                Text = "Apply"
            };
            _applyInvestmentsButton.Click += (sender, args) => ApplyInvestments();
            Add(_applyInvestmentsButton);

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
                Button divest = new(1, 1)
                {
                    CanFocus = false,
                    IsEnabled = false,
                    Position = new(width - 4, ConceptsStartingY + c),
                    Text = "-"
                };
                divest.Click += (sender, args) => OnInvestmentChange(sender, args, index, false);
                divest.MouseEnter += (sender, args) => OnHoverInvestmentChangeBegin(
                    sender, args, index, false);
                divest.MouseExit += OnHoverEnd;

                Button invest = new(1, 1)
                {
                    CanFocus = false,
                    IsEnabled = true,
                    Position = new(width - 3, ConceptsStartingY + c),
                    Text = "+"
                };
                invest.Click += (sender, args) => OnInvestmentChange(sender, args, index, true);
                invest.MouseEnter += (sender, args) => OnHoverInvestmentChangeBegin(
                    sender, args, index, true);
                invest.MouseExit += OnHoverEnd;

                Add(divest);
                Add(invest);
                _conceptDivestmentButtons[c] = divest;
                _conceptInvestmentButtons[c] = invest;
            }

            for (int c = 0; c < MaxPassiveCount; c++)
            {
                int index = c + MaxConceptCount;
                Button divest = new(1, 1)
                {
                    CanFocus = false,
                    IsEnabled = false,
                    Position = new(width - 4, Height - 9 + c),
                    Text = "-"
                };
                divest.Click += (sender, args) => OnInvestmentChange(sender, args, index, false);
                divest.MouseEnter += (sender, args) => OnHoverInvestmentChangeBegin(
                    sender, args, index, false);
                divest.MouseExit += OnHoverEnd;

                Button invest = new(1, 1)
                {
                    CanFocus = false,
                    IsEnabled = true,
                    Position = new(width - 3, Height - 9 + c),
                    Text = "+"
                };
                invest.Click += (sender, args) => OnInvestmentChange(sender, args, index, true);
                invest.MouseEnter += (sender, args) => OnHoverInvestmentChangeBegin(
                    sender, args, index, true);
                invest.MouseExit += OnHoverEnd;

                Add(divest);
                Add(invest);
                _passiveDivestmentButtons[c] = divest;
                _passiveInvestmentButtons[c] = invest;
            }
        }

        /// <summary>
        /// Applies any investments that have been made to the actual Character.
        /// </summary>
        private void ApplyInvestments()
        {
            _projection.ApplyToBase();
            _pendingSuccessorPoints = 0.0f;

            _applyInvestmentsButton.IsEnabled = _projection.HasChanged;

            for (int c = 0; c < MaxConceptCount; c++)
                _conceptDivestmentButtons[c].IsEnabled = false;
            for (int c = 0; c < MaxPassiveCount; c++)
                _passiveDivestmentButtons[c].IsEnabled = false;
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
            if (_learnSkillPopup.IsVisible || _notificationPopup.IsVisible)
                return true;

            return base.ProcessMouse(state);
        }

        /// <summary>
        /// Handles the clicking of an affinity button.
        /// </summary>
        /// <param name="sender">The event sender (the affinity button).</param>
        /// <param name="conceptIndex">The index of the concept whose 
        /// affinity icon is being clicked.</param>
        /// <param name="affinityIndex">The index of the affinity icon, within its 
        /// concept's collection of affinities, that is being clicked.</param>
        private void OnClickAffinity(object sender, MouseEventArgs _,
            int conceptIndex, int affinityIndex)
        {
            Button affinityButton = sender as Button;
            affinityButton.IsEnabled = false;

            Affinity affinity = GameExecution.Player.Class.Concepts[conceptIndex]
                .Affinities[affinityIndex];
            Skill skill = new()
            {
                Affinity = affinity,
                Concept = GameExecution.Player.Class.Concepts[conceptIndex]
            };

            if (GameExecution.Player.HasLearnedSkill(skill))
            {
                _notificationPopup.Content = $"{affinity.Name}\nhas " + 
                    $"already been learned.";
                _notificationPopup.OnDismissal = () =>
                {
                    affinityButton.IsEnabled = true;
                };
                _notificationPopup.IsVisible = true;
            }
            else
            {
                _learnSkillPopup.Prompt = $"Learn {affinity.Name}?";
                _learnSkillPopup.OnConfirmation = () =>
                {
                    Skill skill = new()
                    {
                        Affinity = affinity,
                        Concept = GameExecution.Player.Class.Concepts[conceptIndex]
                    };
                    GameExecution.Player.LearnSkill(skill);
                    Invalidate();

                    affinityButton.IsEnabled = true;
                };
                _learnSkillPopup.OnRejection = () =>
                {
                    affinityButton.IsEnabled = true;
                };
                _learnSkillPopup.IsVisible = true;
            }

            _hoverPopup.IsVisible = false;
        }

        /// <summary>
        /// Handles the start of the hovering of an investment button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="index">The index of the talent whose 
        /// investment button is being hovered.</param>
        /// <param name="isForInvestment">Whether the change is to increase investment.</param>
        private void OnHoverInvestmentChangeBegin(object sender, MouseEventArgs _,
            int index, bool isForInvestment)
        {
            _hoveredButton = sender as Button;
            _hoveredInvestmentIndex = index;

            UpdateHoverContent(isForInvestment);

            _hoverPopup.IsVisible = true;
            _hoverPopup.IsDirty = true;
        }

        /// <summary>
        /// Handles the start of the hovering of an affinity icon.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="conceptIndex">The index of the concept whose 
        /// affinity icon is being hovered.</param>
        /// <param name="affinityIndex">The index of the affinity icon, within its 
        /// concept's collection of affinities, that is being hovered.</param>
        private void OnHoverAffinityBegin(object sender, MouseEventArgs _,
            int conceptIndex, int affinityIndex)
        {
            Affinity affinity = GameExecution.Player.Class.Concepts[conceptIndex]
                .Affinities[affinityIndex];
            _hoveredButton = sender as Button;

            _hoverPopup.Title = affinity.Name;
            _hoverPopup.Content = affinity.GetDescription(_projection);
            _hoverPopup.Position = (sender as Button).Position -
                new Point(_hoverPopup.Width, 0);

            _hoverPopup.IsVisible = true;
            _hoverPopup.IsDirty = true;
        }

        /// <summary>
        /// Handles the end of the hovering of a button.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        private void OnHoverEnd(object sender, MouseEventArgs _)
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
        /// <param name="index">The index of the talent to be invested in.</param>
        /// <param name="isForInvestment">Whether the change is to increase investment.</param>
        private void OnInvestmentChange(object sender, System.EventArgs _,
            int index, bool isForInvestment)
        {
            if (!GameExecution.HasPlayer)
                return;

            Talent talent;
            if (index >= MaxConceptCount)
                talent = GameExecution.Player.Class.Passives[index - MaxConceptCount];
            else
                talent = GameExecution.Player.Class.Concepts[index];

            // TODO :: Support various investment amounts (eg. 1, 5, 10, etc.).
            int changeAmount = 1;
            bool divestEnabled = true;
            if (isForInvestment)
            {
                _projection.InvestInTalent(talent, changeAmount);
                if (index >= MaxConceptCount)
                    _pendingSuccessorPoints += changeAmount / 2.0f;

                if (_projection.AvailableSkillPoints == 0)
                    OnHoverEnd(sender, null);
            }
            else
            {
                int actualChange = _projection.DivestInTalent(talent, changeAmount);

                if (index >= MaxConceptCount)
                    _pendingSuccessorPoints -= actualChange / 2.0f;

                if (_projection.GetProjectedInvestment(talent) <= 0)
                {
                    divestEnabled = false;
                    OnHoverEnd(sender, null);
                }
            }

            if (index >= MaxConceptCount)
                _passiveDivestmentButtons[index - MaxConceptCount].IsEnabled = divestEnabled;
            else
                _conceptDivestmentButtons[index].IsEnabled = divestEnabled;

            UpdateHoverContent(isForInvestment);
            _applyInvestmentsButton.IsEnabled = _projection.HasChanged;
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            if (GameExecution.HasPlayer && IsVisible)
            {
                _projection = GameExecution.Player.GetProjection();
                UpdateContent();

                ReadOnlyCollection<Concept> concepts = GameExecution.Player.Class.Concepts;
                int conceptCount = concepts.Count;
                for (int c = 0; c < MaxConceptCount; c++)
                {
                    bool hasConcept = c < conceptCount;
                    _conceptDivestmentButtons[c].IsVisible = hasConcept;
                    _conceptDivestmentButtons[c].IsEnabled = false;
                    _conceptInvestmentButtons[c].IsVisible = hasConcept;

                    if (hasConcept)
                    {
                        int conceptIndex = c;
                        int startX = TalentNameX + concepts[c].Name.Length + 1;
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
                                Position = new(startX + 2 * cc, ConceptsStartingY + c),
                                ThemeColors = colors
                            };
                            affinityButton.MouseEnter += (sender, args) => OnHoverAffinityBegin(
                                sender, args, conceptIndex, affinityIndex);
                            affinityButton.MouseButtonClicked += (sender, args) => OnClickAffinity(
                                sender, args, conceptIndex, affinityIndex);
                            affinityButton.MouseExit += OnHoverEnd;
                            affinityButton.FocusOnClick = false;

                            _conceptAffinityButtons.Add(affinityButton);
                            Add(affinityButton);
                        }
                    }
                }

                int passiveCount = GameExecution.Player.Class.Passives.Count;
                for (int c = 0; c < MaxPassiveCount; c++)
                {
                    _passiveDivestmentButtons[c].IsVisible = c < passiveCount;
                    _passiveDivestmentButtons[c].IsEnabled = false;

                    _passiveInvestmentButtons[c].IsVisible = c < passiveCount;
                }
            }
            else
            {
                for (int c = 0; c < MaxConceptCount; c++)
                {
                    _conceptDivestmentButtons[c].IsVisible = false;
                    _conceptInvestmentButtons[c].IsVisible = false;

                    for (int cc = 0; cc < _conceptAffinityButtons.Count; cc++)
                        Remove(_conceptAffinityButtons[cc]);
                    _conceptAffinityButtons.Clear();
                }
                for (int c = 0; c < MaxPassiveCount; c++)
                {
                    _passiveDivestmentButtons[c].IsVisible = false;
                    _passiveInvestmentButtons[c].IsVisible = false;
                }
            }

            if (!IsVisible)
            {
                _hoveredButton = null;
                _hoveredInvestmentIndex = -1;
                _hoverPopup.IsVisible = false;

                _learnSkillPopup.IsVisible = false;
                _notificationPopup.IsVisible = false;
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
            if (_projection == null)
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
        /// <param name="diffForInvestment">Whether the difference displayed should be 
        /// for an increase in investment.</param>
        private void UpdateHoverContent(bool diffForInvestment)
        {
            if (_hoveredButton == null || _hoveredInvestmentIndex == -1)
                return;

            bool isPassive = _hoveredInvestmentIndex >= MaxConceptCount;
            Talent talent;
            if (isPassive)
                talent = GameExecution.Player.Class.Passives[_hoveredInvestmentIndex - MaxConceptCount];
            else
                talent = GameExecution.Player.Class.Concepts[_hoveredInvestmentIndex];

            int investment = _projection.GetInvestment(talent);
            int diff = diffForInvestment ? investment + 1 : investment - 1;
            string content = $"{talent.GetDescription(investment)}\n" +
                talent.GetDifferenceDescription(investment, diff) +
                " for 1 point.";

            _hoverPopup.Title = talent.Name;
            _hoverPopup.Content = content;
            _hoverPopup.Position = _hoveredButton.Position -
                new Point(_hoverPopup.Width, isPassive ? _hoverPopup.Height : 0);
        }


        /// <summary>
        /// Updates the list of the active skills to be displayed in the popup.
        /// </summary>
        private void UpdateActiveSkills()
        {
            string[] skillNames = GameExecution.Player.SkillNames;
            for (int c = 0, count = skillNames.Length; c < count; c++)
                Print(2, 4 + c, skillNames[c]);
        }

        /// <summary>
        /// Updates the available points label of the popup.
        /// </summary>
        private void UpdateAvailablePoints()
        {
            int points = _projection.AvailableSkillPoints;
            _availablePointsLabel.DisplayText = $"{AvailablePointsText}{points}";
            _availablePointsLabel.IsDirty = true;

            bool investmentButtonsEnabled = points > 0;
            for (int c = 0, count = _passiveInvestmentButtons.Length; c < count; c++)
                _passiveInvestmentButtons[c].IsEnabled = investmentButtonsEnabled;
            for (int c = 0, count = _conceptInvestmentButtons.Length; c < count; c++)
                _conceptInvestmentButtons[c].IsEnabled = investmentButtonsEnabled;
        }

        /// <summary>
        /// Updates the text of the concepts' descriptions in the popup.
        /// </summary>
        private void UpdateConcepts()
        {
            ReadOnlyCollection<Concept> concepts = GameExecution.Player.Class.Concepts;
            for (int c = 0, count = concepts.Count, y = ConceptsStartingY; c < count; c++, y++)
            {
                int investment = _projection.GetInvestment(concepts[c]);
                string investmentText = investment.ToString();

                Print(TalentNameX, y, concepts[c].Name);
                Print(Width - investmentText.Length - 5, y, investmentText);
            }
        }

        /// <summary>
        /// Updates the text of the passives' descriptions in the popup.
        /// </summary>
        private void UpdatePassives()
        {
            ReadOnlyCollection<Passive> passives = GameExecution.Player.Class.Passives;
            for (int c = 0, count = passives.Count, y = Height - 9; c < count; c++, y++)
            {
                int investment = _projection.GetInvestment(passives[c]);
                string investmentText = investment.ToString();

                Print(TalentNameX, y, passives[c].Name);
                Print(Width - investmentText.Length - 5, y, investmentText);
            }
        }

        /// <summary>
        /// Updates the successor points label of the popup.
        /// </summary>
        private void UpdateSuccessorPoints()
        {
            int points = GameExecution.Player.CharacterLineage.SuccessorPoints +
                ((int)_pendingSuccessorPoints);
            _successorPointsLabel.DisplayText = $"{SuccessorPointsText}{points}";
            _successorPointsLabel.IsDirty = true;
        }
    }
}
