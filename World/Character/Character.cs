﻿using AsLegacy.Characters;
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
        public abstract partial class Character : CharacterBase, 
            IInternalCharacter, IRankedCharacter, ICharacter
        {
            protected const string ActivityMessageAttack = "Hostile";
            protected const string ActivityMessageAttacking = "Attacking";
            protected const string ActivityMessageCooldown = "Cooldown";
            protected const string ActivityMessageDead = "Dead";
            protected const string ActivityMessageDefend = "Defending";
            protected const string ActivityMessageMovingPrefix = "Moving ";
            protected const string ActivityMessageNone = "";

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
            private readonly Dictionary<Direction, string> _directionMapping = new()
            {
                { Direction.Down, "South" },
                { Direction.Left, "West" },
                { Direction.Right, "East" },
                { Direction.Up, "North" }
            };

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
            IAI IInternalCharacter.AI => CharacterAI;

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

            /// <inheritdoc/>
            public int AvailableSkillPoints
            {
                get => _availableSkillPoints;
                set
                {
                    int original = _availableSkillPoints;
                    _availableSkillPoints = value;

                    if (value > original)
                        OnAvailableSkillPointGain?.Invoke(value - original);
                }
            }
            private int _availableSkillPoints = 0;
            private Action<int> OnAvailableSkillPointGain { get; set; }

            /// <summary>
            /// The Character's AI.
            /// </summary>
            protected IAI CharacterAI { get; set; }

            /// <summary>
            /// The class of this Character.
            /// </summary>
            public Class Class { get; private set; }

            /// <summary>
            /// The cooldown of this Character, as a percentage (0 - 1), of how much of 
            /// a current cooldown time remains.
            /// </summary>
            public float Cooldown => TotalCooldown == 0 ?
                0.0f : 1.0f - (PassedCooldown * 1.0f) / TotalCooldown;

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

            public string CurrentActivity
            {
                get
                {
                    if (!IsAlive)
                        return ActivityMessageDead;

                    Action action = CurrentAction as Action;
                    if (action == null)
                    {
                        switch (ActiveMode)
                        {
                            case Mode.Attack:
                                if (Cooldown > 0)
                                    return ActivityMessageCooldown;
                                return ActivityMessageAttack;
                            case Mode.Defend:
                                return ActivityMessageDefend;
                            default:
                                if (Cooldown > 0)
                                    return ActivityMessageCooldown;
                                return ActivityMessageNone;
                        }
                    }
                    return action.Name;
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
            private readonly string[] _equippedSkills = new string[6];

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

            /// <summary>
            /// Specifies whether this Character is in a cooldown stage.
            /// </summary>
            public bool IsInCooldown => TotalCooldown > 0;

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
            /// How much time has passed since the current cooldown stage had begun.
            /// </summary>
            protected int PassedCooldown { get; set; }

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
            /// <remarks>Null if this Character has no target.</remarks>
            public virtual Character Target { get; set; }

            /// <summary>
            /// The total cooldown time for the current cooldown stage being experienced
            /// by this Character.
            /// </summary>
            protected int TotalCooldown { get; set; }

            private readonly BaseSettings _baseSettings;
            private readonly Combat.State _combatState;
            private int _consecutiveAttackCount = 0;
            private int _unappliedCooldown = 0;

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

                if (baseSettings.InitialPassiveInvestments != null)
                    for (int c = 0; c < baseSettings.InitialPassiveInvestments.Length; c++)
                    {
                        if (Class.Passives.Count <= c)
                            break;

                        if (baseSettings.InitialPassiveInvestments[c] != 0)
                            IncreaseTalentInvestment(Class.Passives[c],
                                baseSettings.InitialPassiveInvestments[c]);
                    }


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

            /// <inheritdoc/>
            public virtual float GetAffect(Characters.Attribute affectAttribute)
            {
                float baseValue = affectAttribute.BaseValue;
                float scale = 1.0f;

                for (int c = 0, count = affectAttribute.Aspects.Count; c < count; c++)
                {
                    GetAspectInfluences(affectAttribute.Aspects[c], out float aspectBaseValue,
                        out float aspectScale);
                    baseValue += aspectBaseValue;
                    scale += aspectScale;
                }
                scale = affectAttribute.BaseScale * scale;

                return baseValue * scale;
            }

            /// <inheritdoc/>
            public int GetInvestment(Talent talent)
            {
                _talentInvestments.TryGetValue(talent, out int amount);
                return amount;
            }

            /// <summary>
            /// Provides a Character that exists only as a projection of this Character's stats.
            /// The projection does not have presence on the map and will not alter 
            /// this Character's state when it is altered, however it will otherwise provide stats 
            /// that reflect its own changes on top of any stats of this Character.
            /// </summary>
            /// <returns>The projected Character.</returns>
            public Projection GetProjection()
            {
                return new Projection(this);
            }

            /// <summary>
            /// Specifies whether the character has already learned 
            /// the specified <see cref="Skill"/>.
            /// </summary>
            /// <param name="skill">The skill being checked for.</param>
            /// <returns>Whether the skill has already been learned.</returns>
            public bool HasLearnedSkill(Skill skill)
            {
                return _skills.ContainsKey(skill.Affinity.Name);
            }

            /// <summary>
            /// Initiates the activation and subsequent performance of 
            /// the identified <see cref="Skill"/>.
            /// </summary>
            /// <param name="skillName">The name identifying the <see cref="Skill"/>.</param>
            public void InitiateSkill(string skillName)
            {
                if (skillName == null || IsInCooldown || !IsAlive)
                    return;

                Skill skill = _skills[skillName];
                Affect[] affects = skill.GetAffects(this);
                int activationInMilliseconds = (int)(GetActivation(skill) * 1000.0f);

                _ = new Action(this, activationInMilliseconds, skill.Affinity.Name,
                    () =>
                    {
                        Effect lastMadeEffect = null;
                        for (int c = affects.Length - 1; c >= 0; c--)
                        {
                            Affect affect = affects[c];
                            switch (affects[c].Type)
                            {
                                case Skill.Type.AreaOfEffect:
                                    lastMadeEffect = new ExpandingRingEffect()
                                    {
                                        Color = affects[c].AffectColor,
                                        BaseDamage = affects[c].BaseDamage,
                                        Element = affects[c].Element,
                                        Followup = lastMadeEffect,
                                        Origin = affects[c].Origin,
                                        Performer = this,
                                        Range = affects[c].Range,
                                        Target = affects[c].Target,
                                        UponDamageDealt = () =>
                                        {
                                            AvailableSkillPoints++;

                                            Skill.Performance p = skill.Affinity.Performance;
                                            if (p == Skill.Performance.Attack)
                                                _consecutiveAttackCount++;
                                            else if (p != Skill.Performance.Spell)
                                                throw new NotImplementedException(
                                                    $"The skill performance type, " +
                                                    $"{p}, is not handled for updating " +
                                                    $"consecutive attack counts.");
                                        }
                                    };
                                    break;
                                default:
                                    throw new NotImplementedException($"The concept type, " +
                                        $"{affects[c].Type}, is not yet supported to be " +
                                        $"realized as a skill effect.");
                            }
                        }

                        lastMadeEffect.Start();

                        PassedCooldown = 0;
                        TotalCooldown += (int)(GetCooldown(skill) * 1000.0f);
                        TotalCooldown += _unappliedCooldown;
                        _unappliedCooldown = 0;

                        if (skill.Affinity.Performance == Skill.Performance.Spell)
                            _consecutiveAttackCount = 0;
                        else if (skill.Affinity.Performance != Skill.Performance.Attack)
                            throw new NotImplementedException(
                                $"The skill performance type, {skill.Affinity.Performance}, " +
                                $"is not handled for updating consecutive attack counts.");
                    },
                    () =>
                    {
                        // May be defined by the Skill.
                        return IsAlive;
                    });
            }

            /// <inheritdoc/>
            public virtual int InvestInTalent(Talent talent, int amount)
            {
                if (AvailableSkillPoints == 0)
                    return 0;

                int actualAmount = AvailableSkillPoints < amount ?
                    AvailableSkillPoints : amount;

                AvailableSkillPoints -= actualAmount;
                IncreaseTalentInvestment(talent, actualAmount);

                return actualAmount;
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
            /// Attempts to have this Character learn the specified <see cref="Skill"/>.
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
                if (_mode == Mode.Defend || IsInCooldown)
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

                    GetAspectInfluences(Aspect.Activation, 
                        out float valueChange1, out float scaleChange1);
                    GetAspectInfluences(Aspect.MovementActivation,
                        out float valueChange2, out float scaleChange2);
                    movementInterval += (int)((valueChange1 + valueChange2) * 
                        1000.0f);  // Convert value to seconds.
                    movementInterval = (int)(movementInterval * (
                        1.0f + (scaleChange1 + scaleChange2)));

                    string actionName = ActivityMessageMovingPrefix + _directionMapping[direction];
                    _ = new Action(this, movementInterval, actionName,
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
            /// Specifies that this character has been dealt damage.
            /// The provided damage may be reduced (or eliminated) before being applied 
            /// to this character's health.
            /// </summary>
            /// <param name="attacker">The dealer of the damage.</param>
            /// <param name="damage">The amount of damage dealt to this character.</param>
            /// <param name="element">The elemental type of the damage dealt.</param>
            public void ReceiveDamage(Character attacker, float damage, Skill.Element element)
            {
                _combatState.ReceiveDamage(attacker, damage, element);
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
            /// <returns>Whether auto-attack or auto-move was initiated.</returns>
            protected bool AutoAttackOrMove()
            {
                if (IsInCooldown || CurrentAction != null)
                    return false;

                switch (ActiveMode)
                {
                    case Mode.Normal:
                        // TODO :: Move towards if 'following' is enabled.
                        break;
                    case Mode.Attack:
                        // TODO :: Move towards if not in range of attack.
                        Combat.PerformStandardAttack(this, Target);
                        break;
                    case Mode.Defend:
                        break;
                    default:
                        return false;
                }

                return true;
            }

            /// <summary>
            /// Initiates the death of this Character.
            /// </summary>
            protected virtual void UponDeath()
            {
                GlyphColor = DeadColor;
                ActiveMode = Mode.Normal;

                (CurrentAction as IAction)?.Cancel();
                _ = new World.Action(CharacterRemovalTime, () => RemoveCharacter(this));
            }


            /// <summary>
            /// Provides the actual activation time to perform the provided <see cref="Skill"/>.
            /// </summary>
            /// <param name="skill">The skill whose activation time is being retrieved.</param>
            /// <returns>The activation time, in seconds.</returns>
            private float GetActivation(Skill skill)
            {
                float activation = skill.GetActivation(this);

                if (skill.Affinity.Performance == Skill.Performance.Spell)
                {
                    GetAspectInfluences(Aspect.NextSpellReductionForConsecutiveAttacks, 
                        out float value, out float scale);
                    activation += value * _consecutiveAttackCount;
                    activation *= ((scale * _consecutiveAttackCount) + 1);
                }

                if (activation > 0.0f)
                    return activation;
                return 0.0f;
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
                        case Influence.Purpose.ScaleDown:
                            totalScaleChange -= affect;
                            break;
                        case Influence.Purpose.ScaleUp:
                            totalScaleChange += affect;
                            break;
                        case Influence.Purpose.Subtract:
                            totalBaseValue -= affect;
                            break;
                        default:
                            throw new NotImplementedException($"The influence purpose " +
                                $"{influence.AffectOnAspect} is not supported.");
                    }
                }
            }

            /// <summary>
            /// Provides the actual cooldown time from performing the provided <see cref="Skill"/>.
            /// </summary>
            /// <param name="skill">The skill whose cooldown time is being retrieved.</param>
            /// <returns>The cooldown time, in seconds.</returns>
            private float GetCooldown(Skill skill)
            {
                float cooldown = skill.GetCooldown(this);

                if (skill.Affinity.Performance == Skill.Performance.Spell)
                {
                    GetAspectInfluences(Aspect.NextSpellReductionForConsecutiveAttacks,
                        out float value, out float scale);
                    cooldown += value * _consecutiveAttackCount;
                    cooldown *= ((scale * _consecutiveAttackCount + 1));
                }

                if (cooldown > 0.0f)
                    return cooldown;
                return 0.0f;
            }

            /// <summary>
            /// Increases the investment in the specified <see cref="Talent"/> 
            /// by the specified amount, or defines a new investment if there had been no
            /// previous investment.
            /// </summary>
            /// <param name="talent">The talent whose investment is to increase.</param>
            /// <param name="amount">The amount to increase.</param>
            private void IncreaseTalentInvestment(Talent talent, int amount)
            {
                float previousMaxHealth = MaxHealth;

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


            /// <inheritdoc/>
            void IInternalCharacter.Update(int timeDelta)
            {
                if (IsInCooldown)
                {
                    PassedCooldown += timeDelta;
                    if (PassedCooldown >= TotalCooldown)
                    {
                        PassedCooldown = 0;
                        TotalCooldown = 0;
                    }
                }
            }
        }
    }
}
