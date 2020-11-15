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
            public enum Direction
            {
                Left,
                Right,
                Up,
                Down
            }

            // TODO : 6 :: Mode Property.
            
            public Direction[] AvailableDirections { get; private set; }


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
                // TODO : 5 :: Determine available directions from position.
            }

            public void ActivateSkill() { }

            public void PerformInDirection(Direction direction)
            { 
                // TODO : 6 :: Move and update available direction from new position.
            }
        }
    }
}
