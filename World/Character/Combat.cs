using AsLegacy.Characters;
using System.Collections.Generic;

namespace AsLegacy
{
    public partial class World
    {
        public partial class Character
        {
            /// <summary>
            /// Defines Combat, the system that tracks all combat occurring within the game and 
            /// manages the results, recording relevant value changes in Combat States 
            /// that are referenced by Characters.
            /// </summary>
            protected static class Combat
            {
                /// <summary>
                /// An interface to permit the Combat system to retrieve/change the Combat States, 
                /// and to prevent external constructs from doing so for values that should 
                /// not be needed external to the system.
                /// </summary>
                private interface ICombat
                {
                    /// <summary>
                    /// The standard attack damage dealt by the Character.
                    /// </summary>
                    public float AttackDamage { get; }

                    /// <summary>
                    /// The standard attack interval of the Character.
                    /// </summary>
                    public int AttackInterval { get; }

                    /// <summary>
                    /// The maximum health of the Character.
                    /// </summary>
                    public float BaseMaxHealth { set; }

                    /// <summary>
                    /// The current health of the Character, as an absolute value.
                    /// </summary>
                    public float CurrentHealth { get; set; }

                    /// <summary>
                    /// The reduction in damage dealt to the Character when in defend mode.
                    /// </summary>
                    public float DefenseDamageReduction { get; }

                    /// <summary>
                    /// The current legacy of the Character, 
                    /// represented as a numerical value (points).
                    /// </summary>
                    public int Legacy { get; set; }
                }

                /// <summary>
                /// An interface to permit the Combat system to retrieve/change the Legacy value.
                /// </summary>
                private interface ILegacy
                {
                    /// <summary>
                    /// The current legacy of the Character, 
                    /// represented as a numerical value (points).
                    /// </summary>
                    public int Legacy { get; set; }
                }

                /// <summary>
                /// Defines a Combat Legacy, which records the legacy of a Character.
                /// </summary>
                public class Legacy : ILegacy
                {
                    /// <summary>
                    /// The current legacy of the Character, 
                    /// represented as a numerical value (points).
                    /// </summary>
                    public virtual int CurrentLegacy { get; protected set; }

                    /// <inheritdoc/>
                    int ILegacy.Legacy
                    {
                        get => CurrentLegacy;
                        set => CurrentLegacy = value;
                    }

                    /// <summary>
                    /// Constructs a new Combat Legacy.
                    /// </summary>
                    /// <param name="initialLegacy">The initial legacy of 
                    /// the new Combat Legacy.</param>
                    public Legacy(int initialLegacy)
                    {
                        CurrentLegacy = initialLegacy;
                    }
                }

                /// <summary>
                /// Defines a Combat State, which records the current state of a Character's 
                /// combat-related values.
                /// </summary>
                public class State : ICombat
                {
                    /// <inheritdoc/>
                    float ICombat.AttackDamage { get => _baseAttackDamage; }
                    private readonly float _baseAttackDamage;

                    /// <inheritdoc/>
                    int ICombat.AttackInterval { get => _baseAttackInterval; }
                    private readonly int _baseAttackInterval;

                    /// <summary>
                    /// The current health of the Character, as an absolute value.
                    /// </summary>
                    public float CurrentHealth { get; private set; }
                    float ICombat.CurrentHealth 
                    {
                        get => CurrentHealth;
                        set => CurrentHealth = value;
                    }

                    /// <inheritdoc/>
                    float ICombat.DefenseDamageReduction { get => _baseDefenseDamageReduction; }
                    private readonly float _baseDefenseDamageReduction;

                    /// <summary>
                    /// The legacy of the Character, represented as a numerical value (points).
                    /// </summary>
                    public int Legacy 
                    { 
                        get => _legacy.Legacy; 
                        private set => _legacy.Legacy = value; 
                    }

                    /// <inheritdoc/>
                    int ICombat.Legacy
                    {
                        get => Legacy;
                        set => Legacy = value;
                    }
                    private readonly ILegacy _legacy;

                    /// <summary>
                    /// The maximum health of the Character.
                    /// </summary>
                    public float MaxHealth 
                    { 
                        get => _character.GetAffect(Influence.Aspect.MaxHealth, _baseMaxHealth); 
                    }

                    /// <inheritdoc/>
                    float ICombat.BaseMaxHealth { set => _baseMaxHealth = value; }
                    private float _baseMaxHealth;

                    private Character _character;


                    /// <summary>
                    /// Creates a new State.
                    /// </summary>
                    /// <param name="baseSettings">The Character.BaseSettings that define 
                    /// the base/initial values of the new State.</param>
                    /// <param name="legacy">The legacy of the new State.</param>
                    public State(Character character, BaseSettings baseSettings, Legacy legacy)
                    {
                        _baseMaxHealth = baseSettings.InitialBaseMaxHealth;
                        _baseAttackDamage = baseSettings.InitialAttackDamage;
                        _baseAttackInterval = baseSettings.InitialAttackInterval;
                        _baseDefenseDamageReduction = baseSettings.InitialDefenseDamageReduction;
                        CurrentHealth = _baseMaxHealth;

                        _character = character;
                        _legacy = legacy;
                    }

                    /// <summary>
                    /// Temporary method to increase the CurrentHealth with an increase 
                    /// in the MaxHealth.
                    /// </summary>
                    /// <param name="previousMax">The previous max health.</param>
                    public void UpdateForNewMaxHealth(float previousMax)
                    {
                        CurrentHealth += MaxHealth - previousMax;
                    }
                }


                /// <summary>
                /// Initiates the performance of the standard attack of the attacker 
                /// upon the target with a TargetedAction.
                /// </summary>
                /// <param name="attacker">The performer of the attack.</param>
                /// <param name="target">The target of the attack.</param>
                public static void PerformStandardAttack(Character attacker, Character target)
                {
                    ICombat aState = attacker._combatState;
                    new TargetedAction(attacker, target, aState.AttackInterval,
                        (t) =>
                        {
                            ICombat tState = t._combatState;
                            float dealtDamage = aState.AttackDamage;

                            float damageReduction = 0.0f;
                            if (t.ActiveMode == Mode.Defend)
                                damageReduction = aState.AttackDamage * 
                                    tState.DefenseDamageReduction;

                            t.AvailableSkillPoints += dealtDamage;
                            attacker.AvailableSkillPoints += dealtDamage;
                            tState.CurrentHealth -= (dealtDamage - damageReduction);

                            if (!t.IsAlive)
                            {
                                t.UponDeath();

                                if (t is ItemUser)
                                {
                                    int halfLegacy = tState.Legacy / 2;
                                    UpdateLegacy(attacker, halfLegacy);
                                    UpdateLegacy(t, -halfLegacy);

                                    (t as ItemUser).CharacterLineage.SpawnSuccessor();
                                }
                                else
                                {
                                    UpdateLegacy(attacker, tState.Legacy);
                                    RankedCharacters.Remove(t);
                                }
                            }
                        },
                        (c) =>
                        {
                            return attacker.IsAlive && attacker._mode == Mode.Attack &&
                                c.IsAlive && c == attacker.Target &&
                                attacker.IsAdjacentTo(c.Row, c.Column);
                        }, true);
                }

                /// <summary>
                /// Updates the legacy of the specified Character, and refreshes the ranking.
                /// </summary>
                /// <param name="c">The Character whose legacy is to be updated.</param>
                /// <param name="legacyChange">The amount by which the legacy 
                /// should change.</param>
                private static void UpdateLegacy(Character c, int legacyChange)
                {
                    RankedCharacters.Remove(c);
                    (c._combatState as ICombat).Legacy += legacyChange;
                    RankedCharacters.Add(c);
                }
            }
        }
    }
}
