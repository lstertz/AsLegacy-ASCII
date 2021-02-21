using AsLegacy.Characters;

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
                    public float AttackDamage { get; }
                    public int AttackInterval { get; }

                    public float BaseMaxHealth { set; }
                    public float CurrentHealth { get; set; }

                    public int Legacy { get; set; }
                }

                /// <summary>
                /// An interface to permit the Combat system to retrieve/change the Legacy value.
                /// </summary>
                private interface ILegacy
                {
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
                    float ICombat.AttackDamage { get => baseAttackDamage; }
                    private readonly float baseAttackDamage;

                    int ICombat.AttackInterval { get => baseAttackInterval; }
                    private readonly int baseAttackInterval;

                    /// <summary>
                    /// The current health of the Character, as an absolute value.
                    /// </summary>
                    public float CurrentHealth { get; private set; }
                    float ICombat.CurrentHealth 
                    {
                        get => CurrentHealth;
                        set => CurrentHealth = value;
                    }

                    /// <summary>
                    /// The legacy of the Character, represented as a numerical value (points).
                    /// </summary>
                    public int Legacy 
                    { 
                        get => legacy.Legacy; 
                        private set => legacy.Legacy = value; 
                    }
                    int ICombat.Legacy
                    {
                        get => Legacy;
                        set => Legacy = value;
                    }
                    private readonly ILegacy legacy;

                    /// <summary>
                    /// The maximum health of the Character.
                    /// </summary>
                    public float MaxHealth { get => baseMaxHealth; }  // TODO :: Alter by other states.
                    float ICombat.BaseMaxHealth { set => baseMaxHealth = value; }
                    private float baseMaxHealth;


                    /// <summary>
                    /// Creates a new State.
                    /// </summary>
                    /// <param name="baseSettings">The Character.BaseSettings that define 
                    /// the base/initial values of the new State.</param>
                    /// <param name="legacy">The legacy of the new State.</param>
                    public State(BaseSettings baseSettings, Legacy legacy)
                    {
                        baseMaxHealth = baseSettings.InitialBaseMaxHealth;
                        baseAttackDamage = baseSettings.InitialAttackDamage;
                        baseAttackInterval = baseSettings.InitialAttackInterval;
                        CurrentHealth = baseMaxHealth;

                        this.legacy = legacy;
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
                    ICombat aState = attacker.combatState;
                    new TargetedAction(attacker, target, aState.AttackInterval,
                        (c) =>
                        {
                            ICombat cState = c.combatState;
                            cState.CurrentHealth -= aState.AttackDamage;

                            if (!c.IsAlive)
                            {
                                c.Die();
                                rankedCharacters.Remove(c);  // TODO :: Remove once custom sorting is implemented.

                                // TODO :: Handle Player death differently.
                                if (c is ItemUser)
                                {
                                    int halfLegacy = cState.Legacy / 2;
                                    UpdateLegacy(attacker, halfLegacy);
                                    cState.Legacy -= halfLegacy;

                                    (c as ItemUser).Lineage.SpawnSuccessor();
                                }
                                else
                                    UpdateLegacy(attacker, cState.Legacy);
                            }
                        },
                        (c) =>
                        {
                            return attacker.IsAlive && attacker.mode == Mode.Attack &&
                                c.IsAlive && c == attacker.Target &&
                                attacker.IsAdjacentTo(c.Row, c.Column);
                        }, true);
                }

                // TODO :: Probably make my own sorted HashSet, 
                //          one that still uses hashcode to add/remove.

                /// <summary>
                /// Updates the legacy of the specified Character, and refreshes the ranking.
                /// </summary>
                /// <param name="c">The Character whose legacy is to be updated.</param>
                /// <param name="legacyChange">The amount by which the legacy 
                /// should change.</param>
                private static void UpdateLegacy(Character c, int legacyChange)
                {
                    rankedCharacters.Remove(c);
                    (c.combatState as ICombat).Legacy += legacyChange;
                    rankedCharacters.Add(c);
                }
            }
        }
    }
}
