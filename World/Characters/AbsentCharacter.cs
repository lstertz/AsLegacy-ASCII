﻿using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Represents an abstraction of a Character that has no presence within the World.
        /// Such characters do not exist in a formal sense and can not be interacted with, they 
        /// are essentially placeholders on the map, to be replaced by actual Characters 
        /// when appropriate.
        /// </summary>
        protected class AbsentCharacter : Character
        {
            /// <summary>
            /// Constructs a new Abstract Character at the provided row and column on the map.
            /// </summary>
            /// <param name="row">The row position of the new Abstract Character.</param>
            /// <param name="column">The column position of the new Abstract Character.</param>
            public AbsentCharacter(int row, int column) :
                base(row, column, Color.Transparent, Color.Transparent, 0, true)
            { }
        }
    }
}
