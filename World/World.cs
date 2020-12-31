using System.Collections.Generic;

namespace AsLegacy
{
    /// <summary>
    /// Defines the World, which maintains the actions, characters, effects, and environment
    /// that constitute the game world.
    /// </summary>
    public static partial class World
    {
        private static readonly char[][] map =
        {
            "TTTT.T....T.................".ToCharArray(),
            "TTT....T....T.TTTTT.TT..TTTT".ToCharArray(),
            "TT..TTT..TTT...TTTTTTTT..TTT".ToCharArray(),
            "TTT...T..TTT...TTTTTTTT..TTT".ToCharArray(),
            "TTT..............TTTTTT....T".ToCharArray(),
            "TTTT...TT...TTTT.......TTT..".ToCharArray(),
            "TTTT..TTT..TTTTT.T..T..TTT.T".ToCharArray(),
            "TTTT...TT...TTTTTT..T..TTT.T".ToCharArray(),
            "TTTTT.TTTT.TTTTTT......TTT..".ToCharArray(),
            "TTTT..TTT..TTTTT.......TTT..".ToCharArray(),
            "TTTT...TT...TTTTTT..T..TTTT.".ToCharArray(),
            "TTTTT.TTTT.TTTTTT......TTTT.".ToCharArray(),
            "TTTT..TTT..TTTTT.......TTT..".ToCharArray(),
            "TTTT...TT...TTTTTT..T..TTT.T".ToCharArray(),
            "TTTT.T....T.................".ToCharArray(),
            "TTTT...T....T.TTTTT.TT..TTTT".ToCharArray(),
            "TTTTTTT..TTT...TTTTTTTT..TTT".ToCharArray(),
            "TTTTT....T........TTTTT.....".ToCharArray(),
            "TTTTT.TTTT.TTTTTT......TTTT.".ToCharArray(),
            "TTTT..TTT..TTTTT.......TTT..".ToCharArray(),
            "TTTT...TT...TTTTTT..T..TTTTT".ToCharArray(),
            "TTTTT.TTTT.TTTTTT......TTTT.".ToCharArray(),
            "TTTT..TTT..TTTTT............".ToCharArray(),
            "TTTT.......TTTTTTT..TTTTT.TT".ToCharArray(),
            "TTTTTTTTTT.........TTTT...TT".ToCharArray(),
            "TT..TTTTTTTTTTTTTTTTT...TTTT".ToCharArray(),
            "TTT...................TTTTTT".ToCharArray(),
        };

        /// <summary>
        /// The number of columns within the World.
        /// </summary>
        public static readonly int ColumnCount = map[0].Length;

        /// <summary>
        /// The number of rows within the World.
        /// </summary>
        public static readonly int RowCount = map.Length;

        private static readonly LinkedList<IAction> actions = new LinkedList<IAction>();
        private static readonly Dictionary<Character, Action> characterActions = 
            new Dictionary<Character, Action>();

        /// <summary>
        /// The displayable characters of the World.
        /// </summary>
        public static TileSet<CharacterBase>.Display Characters
        {
            get
            {
                return characters.GetDisplay();
            }
        }
        private static readonly TileSet<CharacterBase> characters = new TileSet<CharacterBase>((row, column) =>
        {
            return new AbsentCharacter(row, column);
        });

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
        private static readonly TileSet<Tile> environment = new TileSet<Tile>((row, column) =>
        {
            if (map[row][column] == 'T')
                return new Tree();
            return new Path();
        });

        /// <summary>
        /// The Player Character.
        /// </summary>
        public static Player Player { get; private set; }

        /// <summary>
        /// Initializes the World with expected Characters.
        /// </summary>
        public static void Init()
        {
            if (Player != null)
                return;

            Player = new Player(3, 3);
            new Beast(7, 5, "Wolf");
            new Beast(19, 19, "Giant Rat");
            new Beast(0, 21, "Bear");
        }

        /// <summary>
        /// Provides the Character at the specified position.
        /// </summary>
        /// <param name="row">The row, defining the y axis of the position.</param>
        /// <param name="column">The column, defining the x axis of the position.</param>
        /// <returns>The Character at the specified position, if there is one, 
        /// null if there is not.</returns>
        public static Character CharacterAt(int row, int column)
        {
            CharacterBase c = characters.Get(row, column);

            if (c == null || c is AbsentCharacter)
                return null;
            return c as Character;
        }

        /// <summary>
        /// Provides the Characters near the specified Character (excluding 
        /// the specified Character), and that are within the specified range, up to the 
        /// number specified.
        /// </summary>
        /// <param name="c">The Character used as a reference to 
        /// find the nearby Characters.</param>
        /// <param name="withinHorizontal">The distance horizontally that a 
        /// nearby Character may be.</param>
        /// <param name="withinVertical">The distance vertically that a 
        /// nearby Character may be.</param>
        /// <returns>A list of nearby characters, or an empty list if none were found.</returns>
        public static List<Character> CharactersNear(Character c, int withinHorizontal, 
            int withinVertical)
        {
            return new List<Character>();
        }

        /// <summary>
        /// Specifies whether the provided position can be passed through.
        /// </summary>
        /// <param name="row">The row, defining the y axis of the position.</param>
        /// <param name="column">The column, defining the x axis of the position.</param>
        /// <returns>True if the position is passable, false otherwise.</returns>
        public static bool IsPassable(int row, int column)
        {
            return characters.IsPassable(row, column) && environment.IsPassable(row, column);
        }

        /// <summary>
        /// Updates the state of the World and any of its constructs.
        /// </summary>
        /// <param name="timeDelta">The time passed, in milliseconds, 
        /// since the last update.</param>
        public static void Update(int timeDelta)
        {
            LinkedListNode<IAction> current;
            LinkedListNode<IAction> next = actions.First;
            while (next != null)
            {
                current = next;
                next = current.Next;

                current.Value.Evaluate(timeDelta);
            }
        }


        /// <summary>
        /// Removes a Character from the World.
        /// </summary>
        /// <param name="c">The Character to be removed.</param>
        private static void RemoveCharacter(Character c)
        {
            if (c != Player) // TODO :: Handle Player death appropriately later.
                characters.ReplaceWith(c.Row, c.Column, new AbsentCharacter(c.Row, c.Column));

            if (c == Player.Target)
                Player.Target = null;
        }
    }
}
