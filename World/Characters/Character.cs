using Microsoft.Xna.Framework;
using System;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Represents an abstraction of a CharacterBase that has a presence within the World.
        /// Such characters exist in a formal sense and can be interacted with.
        /// </summary>
        public abstract partial class Character : CharacterBase, IRankedCharacter
        {
            private const int characterRemovalTime = 700;
            private const float standardAttackDamage = 1.67f;
            private const int standardAttackInterval = 2000;
            private const int movementTime = 500;

            private readonly Color deadColor = Color.DarkGray;

            /// <summary>
            /// Highlights the provided Character, if it isn't null, and removes any 
            /// existing highlight.
            /// </summary>
            /// <param name="c">The Character to be highlighted.</param>
            public static void Highlight(Character c)
            {
                if (HighlightedTile != c && HighlightedTile != null)
                    HighlightedTile.Highlighted = false;

                if (c != null)
                    c.Highlighted = true;
            }

            /// <summary>
            /// Defines the glyph to be shown when the Character is in attack mode.
            /// </summary>
            protected abstract int AttackGlyph { get; }

            /// <summary>
            /// Defines the glyph to be shown when the Character is in defend mode.
            /// </summary>
            protected abstract int DefendGlyph { get; }

            /// <summary>
            /// Defines the glyph to be shown when the Character is in normal mode.
            /// </summary>
            protected abstract int NormalGlyph { get; }


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
            /// The character's present mode.
            /// </summary>
            public Mode ActiveMode
            {
                get
                {
                    return mode;
                }
                private set
                {
                    mode = value;

                    switch (value)
                    {
                        case Mode.Normal:
                            Glyph = NormalGlyph;
                            break;
                        case Mode.Attack:
                            Glyph = AttackGlyph;
                            break;
                        case Mode.Defend:
                            Glyph = DefendGlyph;
                            break;
                        default:
                            break;
                    }
                }
            }
            private Mode mode;
            private bool attackEnabled = false;
            private bool defenseEnabled = false;

            /// <summary>
            /// The current action being performed by this Character, 
            /// or null if it is performing no action.
            /// </summary>
            public World.Action CurrentAction
            {
                get
                {
                    if (characterActions.ContainsKey(this))
                        return characterActions[this];

                    return null;
                }
            }

            /// <summary>
            /// The current health of this Character, as an absolute value.
            /// </summary>
            public float CurrentHealth
            {
                get => currentHealth;
                set
                {
                    currentHealth = value;

                    if (!IsAlive)
                    {
                        GlyphColor = deadColor;
                        ActiveMode = Mode.Normal;

                        (CurrentAction as IAction)?.Cancel();
                        new World.Action(characterRemovalTime, () => RemoveCharacter(this));
                    }
                }
            }
            private float currentHealth;

            /// <summary>
            /// The health of this Character, as a percentage (0 - 1) of its maximum health.
            /// </summary>
            public float Health => CurrentHealth / MaxHealth;

            /// <summary>
            /// Specifies whether this Character is alive.
            /// </summary>
            public bool IsAlive => CurrentHealth > 0;

            /// <summary>
            /// The legacy of this Character, represented as a numerical value (points).
            /// </summary>
            public int Legacy
            {
                get => legacy;
                protected set
                {
                    legacy = value;
                    RerankCharacter(this);
                }
            }
            private int legacy;

            /// <summary>
            /// The maximum health of this Character.
            /// </summary>
            public abstract float MaxHealth { get; }

            /// <summary>
            /// The name of this Character.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Specifies the target of this Character.
            /// The target will be the recipient of certain actions performed 
            /// by this Character.
            /// </summary>
            public virtual Character Target
            {
                get => target;
                set
                {
                    if (target == value)
                        return;

                    target = value;
                    PerformForTarget();
                }
            }
            protected Character target = null;


            /// <summary>
            /// Constructs a new Character.
            /// </summary>
            /// <param name="row">The row position of the new Character.</param>
            /// <param name="column">The column position of the new Character.</param>
            /// <param name="glyphColor">The color of the glyph visually displayed to represent 
            /// the new Character.</param>
            /// <param name="glyph">The glyph visually displayed to represent 
            /// the new Character.</param>
            /// <param name="name">The string name given to the new Character.</param>
            /// <param name="legacy">The starting legacy of the new Character.</param>
            protected Character(int row, int column,
                Color glyphColor, int glyph, string name, int legacy) :
                base(row, column, Color.Transparent, glyphColor, glyph, false)
            {
                mode = Mode.Normal;
                Name = name;

                currentHealth = MaxHealth;
                Legacy = legacy;
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
            /// Initiates an appropriate action, to move towards or attack, at/towards the 
            /// Character's target, as determined by the Character's current state.
            /// </summary>
            /// <returns>Whether an action was initiated.</returns>
            public bool PerformForTarget()
            {
                if (target == null)
                    return false;

                switch (mode)
                {
                    case Mode.Normal:
                        // TODO :: Move towards if 'following' is enabled.
                        break;
                    case Mode.Attack:
                        // TODO :: Move towards if not in range of attack.
                        new TargetedAction(this, target, standardAttackInterval,
                            (c) =>
                            {
                                c.CurrentHealth -= standardAttackDamage;
                            },
                            (c) =>
                            {
                                return IsAlive && mode == Mode.Attack &&
                                    c.IsAlive && c == target &&
                                    IsAdjacentTo(c.Row, c.Column);
                            }, true);
                        return true;
                    case Mode.Defend:
                        break;
                    default:
                        break;
                }

                return false;
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
                if (mode != Mode.Normal)
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
                    new Action(this, movementTime,
                        () =>
                        {
                            Move(this, intendedRow, intendedColumn);

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
                            return IsAlive && mode == Mode.Normal &&
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
                attackEnabled = !attackEnabled;
                UpdateActiveMode();
            }

            /// <summary>
            /// Specifies whether the Defend Mode is enabled, and updates 
            /// the current active mode.
            /// </summary>
            /// <param name="enabled">Whether the Defend Mode should be enabled.</param>
            public void EnableDefense(bool enabled)
            {
                if (defenseEnabled == enabled)
                    return;

                defenseEnabled = enabled;
                UpdateActiveMode();
            }


            /// <summary>
            /// Initiates the death of this Character.
            /// </summary>
            private void Die()
            {
                GlyphColor = deadColor;
                ActiveMode = Mode.Normal;

                (CurrentAction as IAction)?.Cancel();
                new World.Action(characterRemovalTime, () => RemoveCharacter(this));
            }

            /// <summary>
            /// Updates the active mode of the character, based on the current state of 
            /// enabled defense/attack.
            /// </summary>
            private void UpdateActiveMode()
            {
                if (defenseEnabled)
                    ActiveMode = Mode.Defend;
                else if (attackEnabled)
                {
                    ActiveMode = Mode.Attack;
                    PerformForTarget();
                }
                else
                    ActiveMode = Mode.Normal;
            }
        }
    }
}
