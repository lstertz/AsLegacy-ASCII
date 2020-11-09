using Microsoft.Xna.Framework;
using SadConsole;

namespace AsLegacy
{
    public class World
    {
        private static readonly char[][] env =
        {
            "TT..TT".ToCharArray(),
            "T..TTT".ToCharArray(),
            "TT..TT".ToCharArray(),
        };
        private static readonly int columnCount = env[0].Length;
        private static readonly int rowCount = env.Length;

        public Cell[] environment = new Cell[rowCount * columnCount];
        public Cell[] characters = new Cell[rowCount * columnCount];

        public World()
        {
            for (int row = 0; row < rowCount; row++)
                for (int col = 0; col < columnCount; col++)
                {
                    environment[row * columnCount + col] = 
                        new Cell(Color.White, Color.Black, env[row][col]);
                    characters[row * columnCount + col] =
                        new Cell(Color.White, Color.Transparent, 0);
                }
        }

        // TODO :: Adding characters changes characters cells.
        //          Specifies glyph and background to match env background.
        // TODO :: Moving characters swaps cells appearances.
        // TODO :: Inherit from Cell for env tile and character classes.
    }
}
