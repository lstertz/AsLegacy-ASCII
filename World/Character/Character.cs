using AsLegacy.Characters;
using AsLegacy.Characters.Skills;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Represents an abstraction of a CharacterBase that has a presence within the World.
        /// Such characters exist in a formal sense and can be interacted with.
        /// </summary>
        public abstract partial class Character : CharacterBase, ICharacter, IRankedCharacter
        {
            /// <summary>
            /// Defines the standard directions, for immediate actions, available to the Character.
            /// </summary>
            public enum Direction
            {
                Left,
                Right,
                Up,
                Down
            }

            /// <summary>
            /// Defines the different modes of a Character, which heavily influence the state and 
            /// available actions of a Character.
            /// </summary>
            public enum Mode
            {
                Normal,
                Attack,
                Defend
            }

            /// <summary>
            /// Highlights the provided Character, if it isn't null, and removes any 
            /// existing highlight.
            /// </summary>
            /// <param name="c">The Character to be highlighted, null if no 
            /// Character should be highlighted.</param>
            public static void Highlight(Character c)
            {
                if (HighlightedTile != c && HighlightedTile != null)
                    HighlightedTile.Highlighted = false;

                if (c != null)
                    c.Highlighted = true;
            }

            /// <summary>
            /// The time that passes after death before the Character is removed from the World.
            /// </summary>
            protected const int CharacterRemovalTime = 700;

            private static readonly Color DeadColor = Color.DarkGray;


            /// <inheritdoc/>
            IAI ICharacter.AI => CharacterAI;

            /// <summary>
            /// The character's present mode.
            /// </summary>
            public Mode ActiveMode
            {
                get
                {
                    return _mode;
                }
                private set
                {
                    if (_mode == value)
                        return;
                    _mode = value;

                    switch (value)
                    {
                        case Mode.Normal:
                            Glyph = _baseSettings.NormalGlyph;
                            break;
                        case Mode.Attack:
                            Glyph = _baseSettings.AttackGlyph;
                            break;
                        case Mode.Defend:
                            Glyph = _baseSettings.DefendGlyph;
                            break;
                        default:
                            break;
                    }
                }
            }
            private Mode _mode;
            private bool _attackEnabled = false;
            private bool _defenseEnabled = false;

            /// <summary>
            /// The number of skill points that this Character has available 
            /// for investing in skills.
            /// </summary>
            public float AvailableSkillPoints { get; private set; }

            /// <summary>
            /// The Character's AI.
            /// </summary>
            protected IAI CharacterAI { get; set; }

            public Class Class { get; private set; }

            /// <summary>
            /// The current action being performed by this Character, 
            /// or null if it is performing no action.
            /// </summary>
            public World.Action CurrentAction
            {
                get
                {
                    if (CharacterActions.ContainsKey(this))
                        return CharacterActions[this];

                    return null;
                }
            }

            /// <summary>
            /// The current health of this Character, as an absolute value.
            /// </summary>
            public float CurrentHealth => _combatState.CurrentHealth;

            /// <summary>
            /// Provides the names of the skills that this Character currently has equipped.
            /// Empty skill slots are represented as null.
            /// </summary>
            public string[] EquippedSkills
            {
                get
                {
                    string[] skills = new string[_equippedSkills.Length];
                    Array.Copy(_equippedSkills, skills, _equippedSkills.Length);
                    return skills;
                }
            }
            private string[] _equippedSkills = new string[6];

            /// <summary>
            /// The health of this Character, as a percentage (0 - 1) of its maximum health.
            /// </summary>
            public float Health => _combatState.CurrentHealth / _combatState.MaxHealth;

            /// <summary>
            /// Specifies whether this Character has been removed from the World, which 
            /// occurs after the removal time has passed once the Character has died.
            /// </summary>
            public bool HasBeenRemoved => IsAlive ? false : Characters.Get(Row, Column) != this;

            /// <summary>
            /// Specifies whether this Character is alive.
            /// </summary>
            public bool IsAlive => _combatState.CurrentHealth > 0;

            /// <inheritdoc/>
            public int Legacy => _combatState.Legacy;

            /// <summary>
            /// The highest recorded legacy of this Character, 
            /// represented as a numerical value (points)
            /// </summary>
            public virtual int LegacyRecord => _combatState.Legacy;

            /// <summary>
            /// The max health of this Character, as an absolute value.
            /// </summary>
            public float MaxHealth => _combatState.MaxHealth;

            /// <inheritdoc/>
            public string Name { get; private set; }

            /// <summary>
            /// Provides the point (global location) of this Character.
            /// </summary>
            public Point Point => new(Column, Row);

            /// <summary>
            /// Provides the names of the <see cref="Skill"/>s known to this Character.
            /// </summary>
            public string[] SkillNames
            {
                get
                {
                    string[] names = new string[_skills.Count];
                    _skills.Keys.CopyTo(names, 0);
                    return names;
                }
            }
            private readonly Dictionary<string, Skill> _skills = new();

            /// <summary>
            /// Specifies the target of this Character.
            /// The target will be the recipient of certain actions performed 
            /// by this Character.
            /// </summary>
            public virtual Character Target { get; set; }

            private readonly BaseSettings _baseSettings;
            private readonly Combat.State _combatState;

            private readonly Dictionary<Talent, int> _talentInvestments = new();
            private readonly Dictionary<Aspect, List<Talent>> _aspectInfluencers = new();

            /// <summary>
            /// Constructs a new Character.
            /// </summary>
            /// <param name="row">The row position of the new Character.</param>
            /// <param name="column">The column position of the new Character.</param>
            /// <param name="name">The string name given to the new Character.</param>
            /// <param name="baseSettings">The base settings that define various 
            /// aspects of the new Character.</param>
            /// <param name="legacy">The starting legacy of the new Character.</param>
            protected Character(int row, int column, string name,
                BaseSettings baseSettings, Combat.Legacy legacy) : base(row, column, Color.Transparent,
                    baseSettings.GlyphColor, baseSettings.NormalGlyph, false)
            {
                _mode = Mode.Normal;
                Name = name;

                CharacterAI = baseSettings.AI;
                Class = Class.Get(baseSettings.ClassType);

                _baseSettings = baseSettings;
                _combatState = new Combat.State(this, baseSettings, legacy);

                RankedCharacters.Add(this);
            }

            /// <summary>
            /// Specifies whether the Defend Mode is enabled, and updates 
            /// the current active mode.
            /// </summary>
            /// <param name="enabled">Whether the Defend Mode should be enabled.</param>
            public void EnableDefense(bool enabled)
            {
                if (_defenseEnabled == enabled)
                    return;

                _defenseEnabled = enabled;
                UpdateActiveMode();
            }

            /// <summary>
            /// Provides the complete affect, based on the Character's investments, 
            /// for the specified attribute.
            /// </summary>
            /// <param name="affectAttribute">The attribute whose affect is being provided.</param>
            /// <returns>The totaled (additive influencers) and scaled (scaling influencers) 
            /// affect for the specified attribute, or the attribute's default calculated value 
            /// if this Character has not invested in any of the attribute's 
            /// related <see cref="Talent"/>s.</returns>
            public virtual float GetAffect(Characters.Attribute affectAttribute)
            {
                float baseValue = affectAttribute.BaseValue;
                float scale = affectAttribute.BaseScale;

                for (int c = 0, count = affectAttribute.Aspects.Count; c < count; c++)
                {
                    GetAspectInfluences(affectAttribute.Aspects[c], out float aspectBaseValue, out float aspectScale);
                    baseValue += aspectBaseValue;
                    scale += aspectScale;
                }

                return baseValue * scale;
            }

            /// <summary>
            /// Provides the amount of investment that this Character has 
            /// in the provided <see cref="Talent"/>.
            /// </summary>
            /// <param name="talent">The <see cref="Talent"/> whose investment 
            /// is to be retrieved.</param>
            /// <returns>The amount of investment; 0 if there has been no investment.</returns>
            public int GetInvestment(Talent talent)
            {
                _talentInvestments.TryGetValue(talent, out int amount);
                return amount;
            }

            /// <summary>
            /// Invests the specified amount in the provided <see cref="Talent"/>.
            /// </summary>
            /// <param name="talent">The <see cref="Talent"/> to be invested in.</param>
            /// <param name="amount">The investment amount.</param>
            public void InvestInTalent(Talent talent, int amount)
            {
                if (AvailableSkillPoints == 0)
                    return;

                if (AvailableSkillPoints < amount)
                    amount = (int)AvailableSkillPoints;

                float previousMaxHealth = MaxHealth;

                AvailableSkillPoints -= amount;
                if (!_talentInvestments.ContainsKey(talent))
                {
                    Aspect affectedAttribute = talent.Influence.AffectedAspect;
                    if (!_aspectInfluencers.ContainsKey(affectedAttribute))
                        _aspectInfluencers.Add(affectedAttribute, new());
                    _aspectInfluencers[affectedAttribute].Add(talent);

                    _talentInvestments.Add(talent, amount);
                }
                else
                    _talentInvestments[talent] += amount;

                // TODO :: Remove.
                // Temporarily update current health for the change in max health.
                if (talent.Influence.AffectedAspect == Aspect.MaxHealth)
                    _combatState.UpdateForNewMaxHealth(previousMaxHealth);
            }

            /// <summary>
            /// Specifies whether this Character is adjacent (left, right, up, or down) 
            /// to the provided position.
            /// </summary>
            /// <param name="row">The row position to be checked for adjacency.</param>
            /// <param name="column">The column position to be checked for adjacency.</param>
            /// <returns>True if this Character is adjacent to the provided position.</returns>
            public bool IsAdjacentTo(int row, int column)
            {
                int rowDiff = Row - row;
                int columnDiff = Column - column;

                return (rowDiff == -1 && columnDiff == 0) || (rowDiff == 1 && columnDiff == 0) ||
                    (rowDiff == 0 && columnDiff == -1) || (rowDiff == 0 && columnDiff == 1);
            }

            /// <summary>
            /// Attempts to have this Character learn the specified Skill.
            /// </summary>
            /// <param name="skill">The skill to be learned.</param>
            /// <returns>Whether the skill was learned; false if the Character already 
            /// knew the skill through some other means, true otherwise.</returns>
            public bool LearnSkill(Skill skill)
            {
                string name = skill.Affinity.Name;
                if (_skills.ContainsKey(name))
                    return false;

                _skills.Add(name, skill);
                for (int c = 0, count = _equippedSkills.Length; c < count; c++)
                {
                    if (_equippedSkills[c] == null)
                    {
                        _equippedSkills[c] = name;
                        break;
                    }
                }

                return true;
            }

            /// <summary>
            /// Attempts to initiate Character movement in the specified direction.
            /// </summary>
            /// <param name="direction">The direction in which the Character is 
            /// to attempt to move, repeatedly by default.</param>
            /// <param name="repeatMovement">A Func that defines whether movement should 
            /// continue to be attempted after it has succeeded, or null, if movement should 
            /// never be attempted after completing successfully.</param>
            /// <returns>Whether the Character initiated an attempt to move.</returns>
            public bool MoveInDirection(Direction direction, Func<bool> repeatMovement = null)
            {
                if (_mode == Mode.Defend)
                    return false;

                int intendedRow = Row;
                int intendedColumn = Column;
                switch (direction)
                {
                    case Direction.Left:
                        intendedColumn--;
                        break;
                    case Direction.Right:
                        intendedColumn++;
                        break;
                    case Direction.Up:
                        intendedRow--;
                        break;
                    case Direction.Down:
                        intendedRow++;
                        break;
                    default:
                        break;
                }

                if (IsPassable(intendedRow, intendedColumn))
                {
                    Mode movementMode = _mode;
                    int movementInterval = _mode == Mode.Normal ?
                        _baseSettings.InitialNormalMovementInterval :
                        _baseSettings.InitialAttackMovementInterval;

                    new Action(this, movementInterval,
                        () =>
                        {
                            Move(intendedRow, intendedColumn);

                            if (repeatMovement == null || repeatMovement() == false)
                                (CurrentAction as IAction).Cancel();
                            else
                                switch (direction)
                                {
                                    case Direction.Left:
                                        intendedColumn--;
                                        break;
                                    case Direction.Right:
                                        intendedColumn++;
                                        break;
                                    case Direction.Up:
                                        intendedRow--;
                                        break;
                                    case Direction.Down:
                                        intendedRow++;
                                        break;
                                    default:
                                        break;
                                }
                        },
                        () =>
                        {
                            return IsAlive && _mode == movementMode &&
                                IsPassable(intendedRow, intendedColumn);
                        }, true);
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Toggles whether Attack Mode is enabled, and updates 
            /// the current active mode.
            /// </summary>
            public void ToggleAttackMode()
            {
                _attackEnabled = !_attackEnabled;
                UpdateActiveMode();
            }


            /// <summary>
            /// Performs either auto-attack on or an auto-move towards 
            /// the Character's target.
            /// </summary>
            protected void AutoAttackOrMove()
            {
                switch (ActiveMode)
                {
                    case Mode.Normal:
                        // TODO :: Move towards if 'following' is enabled.
                        break;
                    case Mode.Attack:
                        // TODO :: Move towards if not in range of attack.
                        Combat.PerformStandardAttack(this, Target);
                        return;
                    case Mode.Defend:
                        break;
                    default:
                        break;
                }
            }

            /// <summary>
            /// Initiates the death of this Character.
            /// </summary>
            protected virtual void UponDeath()
            {
                GlyphColor = DeadColor;
                ActiveMode = Mode.Normal;

                (CurrentAction as IAction)?.Cancel();
                new World.Action(CharacterRemovalTime, () => RemoveCharacter(this));
            }


            /// <summary>
            /// Provides the cumulative base value and scale change associated with the 
            /// specified <see cref="Aspect"/> for this <see cref="Character"/>.
            /// </summary>
            /// <param name="aspect">The aspect whose cumulative base value and scale change
            /// are to be provided.</param>
            /// <param name="totalBaseValue">The cumulative base value.</param>
            /// <param name="totalScaleChange">The cumulative scale change.</param>
            private void GetAspectInfluences(Aspect aspect, out float totalBaseValue,
                out float totalScaleChange)
            {
                totalBaseValue = 0.0f;
                totalScaleChange = 0.0f;

                if (!_aspectInfluencers.ContainsKey(aspect))
                    return;
                List<Talent> influencers = _aspectInfluencers[aspect];

                for (int c = 0, count = influencers.Count; c < count; c++)
                {
                    Talent talent = influencers[c];
                    if (!_talentInvestments.ContainsKey(talent))
                        continue;

                    float affect = talent.GetAffect(_talentInvestments[talent]);
                    Influence influence = talent.Influence;
                    switch (influence.AffectOnAspect)
                    {
                        case Influence.Purpose.Add:
                            totalBaseValue += affect;
                            break;
                        case Influence.Purpose.Scale:
                            totalScaleChange += affect;
                            break;
                        default:
                            throw new NotImplementedException($"The influence purpose " +
                                $"{influence.AffectOnAspect} is not supported.");
                    }
                }
            }

            /// <summary>
            /// Updates the active mode of the character, based on the current state of 
            /// enabled defense/attack.
            /// </summary>
            private void UpdateActiveMode()
            {
                if (_defenseEnabled)
                    ActiveMode = Mode.Defend;
                else if (_attackEnabled)
                    ActiveMode = Mode.Attack;
                else
                    ActiveMode = Mode.Normal;
            }
        }
    }
}
