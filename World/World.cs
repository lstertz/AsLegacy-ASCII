using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines the World, which maintains the environment, characters, and 
    /// effects that constitute the game world.
    /// </summary>
    public static partial class World
    {
        private static readonly char[][] map =
        {
            ".T.........".ToCharArray(),
            "...T.TTTTT.".ToCharArray(),
            "TTT...TTTTT".ToCharArray(),
            "TTT...TTTTT".ToCharArray(),
            "........TTT".ToCharArray(),
            "...TTTT....".ToCharArray(),
            "..TTTTT.T..".ToCharArray(),
            "...TTTTTT..".ToCharArray(),
            "T.TTTTTT...".ToCharArray(),
            "..TTTTT....".ToCharArray(),
            "...TTTTTT..".ToCharArray(),
        };

        /// <summary>
        /// The number of columns within the World.
        /// </summary>
        public static readonly int ColumnCount = map[0].Length;

        /// <summary>
        /// The number of rows within the World.
        /// </summary>
        public static readonly int RowCount = map.Length;

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
            if (map[row][column] == 'T')
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

        public static Player Player { get; private set; }

        private static Beast beast;

        /// <summary>
        /// Initializes the World with expected Characters.
        /// </summary>
        public static void Init()
        {
            if (Player != null)
                return;

            Player = new Player(3, 3);
            beast = new Beast(7, 1, "Wolf");
        }

        /// <summary>
        /// Specifies whether the provided position can be passed through.
        /// </summary>
        /// <param name="row">The row, defining the y axis of the position..</param>
        /// <param name="column">The column, defining the x axis of the position.</param>
        /// <returns>True if the position is passable, false otherwise.</returns>
        public static bool IsPassable(int row, int column)
        {
            return characters.IsPassable(row, column) && environment.IsPassable(row, column);
        }
    }
}
