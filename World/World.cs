using AsLegacy.Abstractions;
using AsLegacy.Characters;
using Microsoft.Xna.Framework;
using System;
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
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT.T....T.................TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTT....T....T.TTTTT.TT..TTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTT..TTT..TTT...TTTTTTTT..TTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTT...T..TTT...TTTTTTTT..TTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTT..............TTTTTT....TTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT...TT...TTTT.......TTT..TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT..TTT..TTTTT.T..T..TTT.TTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT...TT...TTTTTT..T..TTT.TTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTT.TTTT.TTTTTT......TTT..TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT..TTT..TTTTT.......TTT..TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT...TT...TTTTTT..T..TTTT.TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTT.TTTT.TTTTTT......TTTT.TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT..TTT..TTTTT.......TTT..TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT...TT...TTTTTT..T..TTT.TTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT.T....T.................TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT...T....T.TTTTT.TT..TTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTT..TTT...TTTTTTTT..TTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTT....T........TTTTT.....TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTT.TTTT.TTTTTT......TTTT.TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT..TTT..TTTTT.......TTT..TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT...TT...TTTTTT..T..TTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTT.TTTT.TTTTTT......TTTT.TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT..TTT..TTTTT............TTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTT.......TTTTTTT..TTTTT.TTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTT.........TTTT...TTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTT..TTTTTTTTTTTTTTTTT...TTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTT...................TTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
        };
        private const int expectedBeastPopulation = 4;

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
        public static TileSet<CharacterBase>.Display Characters => characters.GetDisplay();
        private static readonly TileSet<CharacterBase> characters = new TileSet<CharacterBase>((row, column) =>
        {
            return new AbsentCharacter(row, column);
        });

        private static readonly List<Character> presentCharacters = new List<Character>();
        private static readonly SortedSet<Character> rankedCharacters =
            new SortedSet<Character>();
        private static int beastCount = 0;

        private static readonly List<Point> openPositions = new List<Point>();

        /// <summary>
        /// The displayable environment of the World.
        /// </summary>
        public static TileSet<Tile>.Display Environment => environment.GetDisplay();
        private static readonly TileSet<Tile> environment = new TileSet<Tile>((row, column) =>
        {
            if (map[row][column] == 'T')
                return new Tree();

            openPositions.Add(new Point(column, row));
            return new Path();
        });

        /// <summary>
        /// Provides the currently highest ranked Character.
        /// </summary>
        public static Character HighestRankedCharacter
        {
            get
            {
                Character[] characters = new Character[1];
                rankedCharacters.CopyTo(characters, 0, 1);
                return characters[0];
            }
        }

        /// <summary>
        /// The Player Character.
        /// </summary>
        public static Player Player { get; private set; }

        /// <summary>
        /// Initializes the World with first-time starting Characters.
        /// Initialization only works once, if the World needs to be re-initialized, use Reset.
        /// </summary>
        public static void Init()
        {
            if (Player != null)
                return;

            SeedCharacters();
        }

        /// <summary>
        /// Populates the World with starting Characters.
        /// </summary>
        private static void SeedCharacters()
        {
            Player = new Player(12, 11);
            new ItemUser(14, 15, "Goblin", 20, "Orr");

            for (int c = 0; c < expectedBeastPopulation; c++)
                new Beast(GetRandomPassablePosition(), Beast.GetRandomType());
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
        /// <param name="character">The Character used as a reference to 
        /// find the nearby Characters.</param>
        /// <param name="withinHorizontal">The distance horizontally that a 
        /// nearby Character may be.</param>
        /// <param name="withinVertical">The distance vertically that a 
        /// nearby Character may be.</param>
        /// <returns>A list of nearby characters, or an empty list if none were found.</returns>
        public static Character[] CharactersNear(Character character, int withinHorizontal, 
            int withinVertical)
        {
            // TODO :: Optimize later, perhaps with 2D Linked Grid data structure support.

            Dictionary<Character, int> nearCharacters = new Dictionary<Character, int>();
            foreach (Character c in presentCharacters)
            {
                int columnDiff = character.Column - c.Column;
                int rowDiff = character.Row - c.Row;

                if (columnDiff == 0 && rowDiff == 0)
                    continue;

                if (columnDiff <= withinHorizontal && columnDiff > -withinHorizontal && 
                    rowDiff <= withinVertical && rowDiff > -withinVertical)
                    nearCharacters.Add(c, character.SquaredDistanceTo(c));
            }

            Character nextNearest = null;
            Character[] sortedCharacters = new Character[nearCharacters.Count];
            for (int i = 0; i < sortedCharacters.Length; i++)
            {
                int nextDistance = int.MaxValue;
                foreach (Character nearCharacter in nearCharacters.Keys)
                {
                    int d = nearCharacters[nearCharacter];
                    if (d < nextDistance)
                    {
                        nextNearest = nearCharacter;
                        nextDistance = d;
                    }
                }

                nearCharacters.Remove(nextNearest);
                sortedCharacters[i] = nextNearest;
            }

            return sortedCharacters;
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
        /// Provides the ranked Characters within the specified range.
        /// </summary>
        /// <param name="startIndex">The index from which the range begins.</param>
        /// <param name="count">The number of ranks to be included in the range, if available.</param>
        /// <returns>The requested range of ranked Characters, or as many Characters 
        /// after the start rank that exist if there are not enough to fill the range.</returns>
        public static Character[] RankedCharactersFor(int startIndex, int count)
        {
            int c = count;
            if (c > rankedCharacters.Count)
                c = rankedCharacters.Count - startIndex;

            Character[] characters = new Character[c];
            rankedCharacters.CopyTo(characters, startIndex, c);

            return characters;
        }

        /// <summary>
        /// Resets the World.
        /// This cancels all active Actions, removes all existing Characters, 
        /// and populates new starting Characters.
        /// </summary>
        public static void Reset()
        {
            while (actions.First != null)
                actions.First.Value.Cancel();

            for (int c = presentCharacters.Count - 1; c >= 0; c--)
                RemoveCharacter(presentCharacters[c], false);

            SeedCharacters();
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
        /// Provides a random passable position from the map.
        /// </summary>
        /// <returns>A Point, the randomly chosen passable position.</returns>
        private static Point GetRandomPassablePosition()
        {
            Random r = new Random();

            Point passablePoint = openPositions[r.Next(0, openPositions.Count)];
            while (!IsPassable(passablePoint.Y, passablePoint.X))
                passablePoint = openPositions[r.Next(0, openPositions.Count)];

            return passablePoint;
        }


        /// <summary>
        /// Adds a Character to the World.
        /// </summary>
        /// <param name="row">The row, defining the y axis 
        /// of the character's position.</param>
        /// <param name="column">The column, defining the x axis 
        /// of the character's position.</param>
        /// <param name="c">The Character to be added.</param>
        private static void AddCharacter(int row, int column, CharacterBase c)
        {
            if (characters != null)
                characters.ReplaceWith(row, column, c);

            if (c is Character)
                presentCharacters.Add(c as Character);

            if (c is Beast)
                beastCount++;
        }

        /// <summary>
        /// Removes a Character from the World.
        /// </summary>
        /// <param name="c">The Character to be removed.</param>
        private static void RemoveCharacter(Character c, bool replaceBeasts = true)
        {
            presentCharacters.Remove(c);
            rankedCharacters.Remove(c);

            // TODO :: Handle Player death appropriately later.
            characters.ReplaceWith(c.Row, c.Column, new AbsentCharacter(c.Row, c.Column));

            if (c == Player.Target)
                Player.Target = null;

            if (c is Beast)
            {
                beastCount--;
                if (beastCount < expectedBeastPopulation && replaceBeasts)
                    new Action(20000, () =>
                    {
                        new Beast(GetRandomPassablePosition(), Beast.GetRandomType());
                    });
            }
        }
    }
}
