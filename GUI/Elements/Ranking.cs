using SadConsole;
using Colors = AsLegacy.Global.Colors;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines a Ranking GUI element, 
    /// which is responsible for showing a set of ordered information in a scrollable table view.
    /// </summary>
    public class Ranking : Console
    {
        private const string rankTitle = "Rank ";
        private readonly int rankSpace = rankTitle.Length;
        private const string nameTitle = "Name           ";
        private readonly int nameSpace = nameTitle.Length;
        private const string legacyTitle = "Legacy        ";
        private readonly int legacySpace = legacyTitle.Length;

        private readonly int tableRows;

        /// <summary>
        /// Constructs a new Ranking.
        /// </summary>
        /// <param name="width">The width of the Ranking.</param>
        /// <param name="height">The height of the Ranking.</param>
        public Ranking(int width, int height) : base(width, height)
        {
            tableRows = height - 2;

            DrawHeader();
            DrawFrame();

            // TODO :: Add filter options.
            // TODO :: Add a scroll bar (child ControlsConsole).
        }

        private void DrawFrame()
        {
            int afterNameSpace = rankSpace + nameSpace;
            SetGlyph(0, 0, 222);
            for (int y = 1; y < Height - 1; y++)
            {
                SetGlyph(0, y, 179);
                SetGlyph(rankSpace, y, 179);
                SetGlyph(afterNameSpace, y, 179);
                SetGlyph(Width - 1, y, 179);
            }
            SetGlyph(Width - 1, 0, 221);

            SetGlyph(0, Height - 1, 192);
            for (int x = 1; x < Width - 1; x++)
                SetGlyph(x, Height - 1, 196);
            SetGlyph(rankSpace, Height - 1, 193);
            SetGlyph(afterNameSpace, Height - 1, 193);
            SetGlyph(Width - 1, Height - 1, 217);
        }

        private void DrawHeader()
        {
            int offset = 1;

            Print(offset, 0, rankTitle, Colors.Black, Colors.White);
            offset += rankSpace;

            Print(offset, 0, nameTitle, Colors.Black, Colors.White);
            offset += nameSpace;

            Print(offset, 0, legacyTitle, Colors.Black, Colors.White);
        }

        public override void Draw(System.TimeSpan update)
        {
            base.Draw(update);

            World.Character[] characters = World.RankedCharactersFor(0, tableRows);
            for (int c = 0;  c < tableRows; c++)
            {
                int offset = 1;
                string rank = (c + 1).ToString();

                Print(offset, c + 1, rank);
                offset += rankSpace;

                Clear(offset, c + 1, nameSpace - 1);
                if (c < characters.Length)
                    Print(offset, c + 1, characters[c].Name);
                offset += nameSpace;

                Clear(offset, c + 1, legacySpace - 1);
                if (c < characters.Length)
                    Print(offset, c + 1, characters[c].Legacy.ToString());
            }
        }
    }
}
