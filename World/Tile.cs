using Microsoft.Xna.Framework;
using SadConsole;

namespace AsLegacy
{
    public class Tile
    {
        public static implicit operator Cell(Tile tile)
        {
            return tile.cell;
        }

        private Cell cell;

        public Tile(Color foreground, Color background, int glyph)
        {
            cell = new Cell(foreground, background, glyph);
        }
    }
}
