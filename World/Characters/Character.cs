using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Represents an abstraction of a CharacterBase that has a presence within the World.
        /// Such characters exist in a formal sense and can be interacted with.
        /// </summary>
        public abstract class Character : CharacterBase
        {
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
            protected abstract int attackGlyph { get; }

            /// <summary>
            /// Defines the glyph to be shown when the Character is in defend mode.
            /// </summary>
            protected abstract int defendGlyph { get; }

            /// <summary>
            /// Defines the glyph to be shown when the Character is in normal mode.
            /// </summary>
            protected abstract int normalGlyph { get; }


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
                            Glyph = normalGlyph;
                            break;
                        case Mode.Attack:
                            Glyph = attackGlyph;
                            break;
                        case Mode.Defend:
                            Glyph = defendGlyph;
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
            /// The current health of this Character, as an absolute value.
            /// </summary>
            public float CurrentHealth { get; protected set; }

            /// <summary>
            /// The health of this Character, as a percentage (0 - 1) of its maximum health.
            /// </summary>
            public float Health => CurrentHealth / MaxHealth;

            /// <summary>
            /// Specifies whether this Character is alive.
            /// </summary>
            public bool IsAlive => CurrentHealth > 0;

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
                set { target = value; }
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
            protected Character(int row, int column,
                Color glyphColor, int glyph, string name) :
                base(row, column, Color.Transparent, glyphColor, glyph, false)
            {
                mode = Mode.Normal;
                Name = name;

                CurrentHealth = MaxHealth;
            }

            /// <summary>
            /// Performs an appropriate action, to move towards or attack, at/towards the 
            /// specified position, as determined by the Character's current state.
            /// </summary>
            /// <param name="row">The row position, the location on the y-axis.</param>
            /// <param name="column">The column position, the location on the x-axis.</param>
            /// <returns>Whether an action was performed.</returns>
            public bool PerformForPosition(int row, int column)
            {
                switch (mode)
                {
                    case Mode.Normal:
                        // TODO :: Move towards if 'following' is enabled.
                        break;
                    case Mode.Attack:
                        // TODO :: Move towards if not in range of attack.
                        // TODO :: Attack if in range.
                        break;
                    case Mode.Defend:
                        break;
                    default:
                        break;
                }

                return false;
            }

            /// <summary>
            /// Attempts to move the Character in the specified direction.
            /// </summary>
            /// <param name="direction">The direction in which the Character is 
            /// to attempt to move.</param>
            /// <returns>Whether the Character was able to move.</returns>
            public bool MoveInDirection(Direction direction)
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

                if (World.IsPassable(intendedRow, intendedColumn))
                {
                    Move(this, intendedRow, intendedColumn);
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
            /// Updates the active mode of the character, based on the current state of 
            /// enabled defense/attack.
            /// </summary>
            private void UpdateActiveMode()
            {
                if (defenseEnabled)
                    ActiveMode = Mode.Defend;
                else if (attackEnabled)
                    ActiveMode = Mode.Attack;
                else
                    ActiveMode = Mode.Normal;
            }
        }
    }
}
