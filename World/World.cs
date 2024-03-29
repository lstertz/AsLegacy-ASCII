﻿using AsLegacy.Abstractions;
using AsLegacy.Characters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using static AsLegacy.World.Character;

namespace AsLegacy
{
    /// <summary>
    /// Defines the World, which maintains the actions, characters, effects, and environment
    /// that constitute the game world.
    /// </summary>
    public static partial class World
    {
        private static readonly char[][] Map =
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
            "TTTTTTTTTTT.......TTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTT.......TTTTTTTTTTTTTTTTTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTTT.TTTT.TTT...TTTTTTTT..TTTTTTTTTTTTT".ToCharArray(),
            "TTTTTTTTTT....T..TTT...TTTTTTTT..TTTTTTTTTTTTT".ToCharArray(),
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
        private const int ExpectedBeastPopulation = 4;

        private static readonly Zone _npcZone = new(new Rectangle[] { new(10, 11, 26, 25) });
        private static readonly Zone _playerZone = new(new Rectangle[] { new(11, 9, 7, 2) });
        private static readonly Dictionary<SpawnZone, Zone> _spawnZones = new()
        {
            { SpawnZone.NPC, _npcZone },
            { SpawnZone.Player, _playerZone }
        };

        /// <summary>
        /// The number of columns within the World.
        /// </summary>
        public static readonly int ColumnCount = Map[0].Length;

        /// <summary>
        /// The number of rows within the World.
        /// </summary>
        public static readonly int RowCount = Map.Length;

        private static readonly LinkedList<IAction> Actions = new();
        private static readonly Dictionary<Character, Action> CharacterActions = new();

        private static readonly Dictionary<Character, HashSet<Effect>> ActiveEffects = new();

        /// <summary>
        /// The displayable characters of the World.
        /// </summary>
        public static TileSet<CharacterBase>.Display CharacterTiles => Characters.GetDisplay();
        private static readonly TileSet<CharacterBase> Characters =
            new((row, column) =>
        {
            return new AbsentCharacter(row, column);
        });

        private static readonly List<Character> PresentCharacters = new();
        private static readonly SortedSet<Character> RankedCharacters = new();
        private static int BeastCount = 0;

        /// <summary>
        /// The displayable effects of the World.
        /// </summary>
        public static TileSet<Tile>.Display EffectTiles => Effects.GetDisplay();
        private static readonly TileSet<Tile> Effects = new((row, column) =>
        {
            return new EffectGraphic(Color.Transparent, ' ');
        });

        /// <summary>
        /// The displayable environment of the World.
        /// </summary>
        public static TileSet<Tile>.Display EnvironmentTiles => Environment.GetDisplay();
        private static readonly TileSet<Tile> Environment = new((row, column) =>
        {
            if (Map[row][column] == 'T')
                return new Tree();

            Point point = new(column, row);
            _npcZone.RegisterOpenPosition(point);
            _playerZone.RegisterOpenPosition(point);

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
                RankedCharacters.CopyTo(characters, 0, 1);
                return characters[0];
            }
        }

        /// <summary>
        /// Initializes the World in its original new game state.
        /// This cancels all active Actions, removes all existing Characters, 
        /// and populates new starting Characters.
        /// </summary>
        public static void InitNewWorld()
        {
            while (Actions.First != null)
                Actions.First.Value.Cancel();

            for (int c = PresentCharacters.Count - 1; c >= 0; c--)
            {
                RankedCharacters.Remove(PresentCharacters[c]);
                RemoveCharacter(PresentCharacters[c], false);
            }

            Character[] performers = new Character[ActiveEffects.Count];
            ActiveEffects.Keys.CopyTo(performers, 0);
            for (int pIndex = 0, pCount = performers.Length; pIndex < pCount; pIndex++)
            {
                Effect[] effects = new Effect[ActiveEffects[performers[pIndex]].Count];
                ActiveEffects[performers[pIndex]].CopyTo(effects);

                for (int eIndex = 0, eCount = effects.Length; eIndex < eCount; eIndex++)
                    effects[eIndex].Stop();
            }

            SeedCharacters();
        }

        /// <summary>
        /// Populates the World with starting Characters.
        /// </summary>
        private static void SeedCharacters()
        {
            _ = new ItemUser(14, 12, "Goblin", 10, "Orr");

            for (int c = 0; c < ExpectedBeastPopulation; c++)
                _ = new Beast(GetRandomPassablePosition(SpawnZone.NPC), Beast.GetRandomType());
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
            CharacterBase c = Characters.Get(row, column);

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
            if (character == null)
                return Array.Empty<Character>();

            // TODO :: Optimize later, perhaps with 2D Linked Grid data structure support.

            Dictionary<Character, int> nearCharacters = new();
            foreach (Character c in PresentCharacters)
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
        /// Provides a random passable position from the map, within the specified spawn zone.
        /// </summary>
        /// <param name="spawnZone">The spawn zone from which the position should 
        /// be chosen.</param>
        /// <returns>A Point, the randomly chosen passable position.</returns>
        public static Point GetRandomPassablePosition(SpawnZone spawnZone)
        {
            Point passablePoint = _spawnZones[spawnZone].GetRandomOpenPosition();
            while (!IsPassable(passablePoint.Y, passablePoint.X))
                passablePoint = _spawnZones[spawnZone].GetRandomOpenPosition();

            return passablePoint;
        }

        /// <summary>
        /// Specifies whether the provided position can be passed through.
        /// </summary>
        /// <param name="row">The row, defining the y axis of the position.</param>
        /// <param name="column">The column, defining the x axis of the position.</param>
        /// <returns>True if the position is passable, false otherwise.</returns>
        public static bool IsPassable(int row, int column)
        {
            return Characters.IsPassable(row, column) && Environment.IsPassable(row, column);
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
            if (c > RankedCharacters.Count)
                c = RankedCharacters.Count - startIndex;

            Character[] characters = new Character[c];
            RankedCharacters.CopyTo(characters, startIndex, c);

            return characters;
        }

        /// <summary>
        /// Updates the state of the World and any of its constructs.
        /// </summary>
        /// <param name="timeDelta">The time passed, in milliseconds, 
        /// since the last update.</param>
        public static void Update(int timeDelta)
        {
            for (int c = 0, count = PresentCharacters.Count; c < count; c++)
            {
                Character character = PresentCharacters[c];
                IInternalCharacter internalCharacter = character as IInternalCharacter;

                internalCharacter.Update(timeDelta);

                IAI ai = internalCharacter.AI;
                ai.UpdateTargetOf(character);
                ai.UpdateModeOf(character);
                ai.InitiateActionFor(character);
            }

            LinkedListNode<IAction> current;
            LinkedListNode<IAction> next = Actions.First;
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
            if (Characters != null)
                Characters.ReplaceWith(row, column, c);

            if (c is Character)
                PresentCharacters.Add(c as Character);

            if (c is Beast)
                BeastCount++;
        }

        /// <summary>
        /// Removes a Character from the World.
        /// </summary>
        /// <param name="c">The Character to be removed.</param>
        private static void RemoveCharacter(Character c, bool replaceBeasts = true)
        {
            PresentCharacters.Remove(c);

            // TODO :: Handle Player death appropriately later.
            Characters.ReplaceWith(c.Row, c.Column, new AbsentCharacter(c.Row, c.Column));

            if (c is Beast)
            {
                BeastCount--;
                if (BeastCount < ExpectedBeastPopulation && replaceBeasts)
                    _ = new Action(20000, () =>
                    {
                        _ = new Beast(GetRandomPassablePosition(SpawnZone.NPC), 
                            Beast.GetRandomType());
                    });
            }
        }
    }
}
