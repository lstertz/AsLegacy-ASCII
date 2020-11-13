﻿using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines a Player, the focal character, on the World map, of the real-life player as 
        /// they interact with the game.
        /// </summary>
        public class Player : PresentCharacter
        {
            /// <summary>
            /// Constructs a new Player at the provided row and column on the map.
            /// </summary>
            /// <param name="row">The row position of the new Player.</param>
            /// <param name="column">The column position of the new Player.</param>
            public Player(int row, int column) :
                base(row, column, Color.White, '@')
            {

            }
        }
    }
}
