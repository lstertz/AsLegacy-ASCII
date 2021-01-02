﻿using AsLegacy.Abstractions;
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

        private static readonly List<Character> presentCharacters = new List<Character>();

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

            Player = new Player(12, 12);
            new Beast(18, 13, "Wolf");
            new Beast(31, 29, "Giant Rat");
            new Beast(10, 32, "Bear");
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
        }

        /// <summary>
        /// Removes a Character from the World.
        /// </summary>
        /// <param name="c">The Character to be removed.</param>
        private static void RemoveCharacter(Character c)
        {
            presentCharacters.Remove(c);

            if (c != Player) // TODO :: Handle Player death appropriately later.
                characters.ReplaceWith(c.Row, c.Column, new AbsentCharacter(c.Row, c.Column));

            if (c == Player.Target)
                Player.Target = null;
        }
    }
}
