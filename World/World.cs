using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines the World, which maintains the environment, characters, and 
    /// effects that constitute the game world.
    /// </summary>
    public static partial class World
    {
        private static readonly char[][] env =
        {
            "TT..TT".ToCharArray(),
            "T..TTT".ToCharArray(),
            "TT..TT".ToCharArray(),
        };

        /// <summary>
        /// The number of columns within the World.
        /// </summary>
        public static readonly int columnCount = env[0].Length;

        /// <summary>
        /// The number of rows within the World.
        /// </summary>
        public static readonly int rowCount = env.Length;

        /// <summary>
        /// The displayable environment of the World.
        /// </summary>
        public static TileSet<Tile>.Display Environment
        {
            get
            {
                return environment.GetDisplay();
            }
        }
        private static TileSet<Tile> environment = new TileSet<Tile>((row, column) =>
        {
            if (env[row][column] == 'T')
                return new Tree();
            return new Path();
        });

        /// <summary>
        /// The displayable characters of the World.
        /// </summary>
        public static TileSet<Character>.Display Characters
        {
            get
            {
                return characters.GetDisplay();
            }
        }
        private static TileSet<Character> characters = new TileSet<Character>((row, column) =>
        {
            return new AbsentCharacter(row, column);
        });

        private static Player player;

        /// <summary>
        /// Initializes the World with expected Characters.
        /// </summary>
        public static void Init()
        {
            if (player != null)
                return;

            player = new Player(1, 1);
        }
    }
}
