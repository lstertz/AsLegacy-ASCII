using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Represents an abstraction of a Character that has a presence within the World.
        /// Such characters exist in a formal sense and can be interacted with.
        /// </summary>
        public abstract class PresentCharacter : Character
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
                Move,
                Attack,
                Defend
            }

            /// <summary>
            /// The character's present mode.
            /// </summary>
            private Mode mode;
            

            /// <summary>
            /// Constructs a new Present Character.
            /// </summary>
            /// <param name="row">The row position of the new Present Character.</param>
            /// <param name="column">The column position of the new Present Character.</param>
            /// <param name="glyphColor">The color of the glyph visually displayed to represent 
            /// the new Present Character.</param>
            /// <param name="glyph">The glyph visually displayed to represent 
            /// the new Present Character.</param>
            protected PresentCharacter(int row, int column, Color glyphColor, int glyph) :
                base(row, column, Color.Transparent, glyphColor, glyph, false)
            {
                mode = Mode.Move;
            }

            public void ActivateSkill() { }

            /// <summary>
            /// Performs the appropriate action for the character's present state, in the 
            /// specified direction, if possible.
            /// </summary>
            /// <param name="direction">The direction in which the action is 
            /// to be performed.</param>
            public void PerformInDirection(Direction direction)
            {
                switch (mode)
                {
                    case Mode.Move:
                        switch (direction)
                        {
                            case Direction.Left:
                                if (World.IsPassable(Row, Column - 1))
                                    Move(this, Row, Column - 1);
                                break;
                            case Direction.Right:
                                if (World.IsPassable(Row, Column + 1))
                                    Move(this, Row, Column + 1);
                                break;
                            case Direction.Up:
                                if (World.IsPassable(Row - 1, Column))
                                    Move(this, Row - 1, Column);
                                break;
                            case Direction.Down:
                                if (World.IsPassable(Row + 1, Column))
                                    Move(this, Row + 1, Column);
                                break;
                            default:
                                break;
                        }
                        break;
                    case Mode.Attack:
                        break;
                    case Mode.Defend:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
