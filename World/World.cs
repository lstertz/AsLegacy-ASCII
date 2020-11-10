using Microsoft.Xna.Framework;
using SadConsole;
using System;

namespace AsLegacy
{
    public static partial class World
    {
        private static readonly char[][] env =
        {
            "TT..TT".ToCharArray(),
            "T..TTT".ToCharArray(),
            "TT..TT".ToCharArray(),
        };
        public static readonly int columnCount = env[0].Length;
        public static readonly int rowCount = env.Length;

        public static Cell[] Environment
        {
            get
            {
                return environment.CastTo((element) => { return (Cell)element; });
            }
        }
        private static Tile[] environment = new Tile[rowCount * columnCount];

        public static Cell[] Characters
        {
            get
            {
                return characters.CastTo((element) => { return (Cell)element; });
            }
        }
        private static Character[] characters = new Character[rowCount * columnCount];

        public static void Init()
        {
            for (int row = 0; row < rowCount; row++)
                for (int col = 0; col < columnCount; col++)
                {
                    environment[row * columnCount + col] = 
                        new Tile(Color.Black, Color.White, env[row][col], true);
                    new AbsentCharacter(col, row);
                }

            new Player(1, 1);
        }

        private static AbsentCharacter GetAbsentCharacter(int col, int row)
        {
            Character c = characters[row * columnCount + col];
            if (!(c is AbsentCharacter))
                throw new InvalidOperationException("An absent character cannot be retrieved " + 
                    "from (" + col + ", " + row + "), as a character is present there.");

            return c as AbsentCharacter;
        }

        // TODO :: Adding characters changes characters cells.
        //          Specifies glyph and background to match env background.
        // TODO :: Moving characters swaps cells appearances.
        // TODO :: Inherit from Cell for env tile and character classes.
    }
}
