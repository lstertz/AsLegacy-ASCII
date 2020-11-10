using Microsoft.Xna.Framework;
using SadConsole;

namespace AsLegacy
{
    public static partial class World
    {
        public class Tile
        {
            public static implicit operator Cell(Tile tile)
            {
                return tile.cell;
            }

            public int Glyph
            {
                get { return cell.Glyph; }
                set { cell.Glyph = value; }
            }

            public Color GlyphColor
            {
                get { return cell.Foreground; }
                set { cell.Foreground = value; }
            }

            public Color Background
            {
                get { return cell.Background; }
                set { cell.Background = value; }
            }

            private Cell cell;

            public bool Passable
            {
                get;
                private set;
            }

            public Tile(Color background, Color glyphColor, int glyph, bool passable)
            {
                cell = new Cell(glyphColor, background, glyph);
                Passable = passable;
            }

            protected Tile(Tile original, bool passable)
            {
                cell = original.cell;
                Passable = passable;
            }
        }
    }
}
