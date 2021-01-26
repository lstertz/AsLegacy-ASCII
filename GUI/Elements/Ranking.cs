using AsLegacy.Characters;
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
        /// <summary>
        /// The total width of a Ranking.
        /// </summary>
        public const int TotalWidth = 36;

        private const string rankTitle = "Rank ";
        private readonly int rankSpace = rankTitle.Length;
        private const string nameTitle = "Name             ";
        private readonly int nameSpace = nameTitle.Length;
        private const string legacyTitle = "Legacy      ";
        private readonly int legacySpace = legacyTitle.Length;

        private readonly int tableRows;

        /// <summary>
        /// Constructs a new Ranking.
        /// </summary>
        /// <param name="height">The height of the Ranking.</param>
        /// <param name="displayGoal">Specifies whether the footer of the Ranking should 
        /// be drawn to display the goal.</param>
        public Ranking(int height, bool displayGoal = false) : base(TotalWidth, height)
        {
            tableRows = height - 2;

            DrawHeader();
            DrawFrame();
            if (displayGoal)
                DrawFooter();

            // TODO :: Add filter options.
            // TODO :: Add a scroll bar (child ControlsConsole).
        }

        /// <summary>
        /// Draws a footer, over the bottom of the frame, to display 
        /// the legacy goal of the game.
        /// </summary>
        private void DrawFooter()
        {
            string goal = System.Convert.ToString(AsLegacy.Goal);

            SetGlyph(0, Height - 1, 222);
            Fill(1, Height - 1, TotalWidth - 2, Colors.Transparent, Colors.White, 0);
            Print(rankSpace + nameSpace + 1, Height - 1, "Goal", Colors.Black, Colors.White);
            Print(TotalWidth - goal.Length - 1, Height - 1, goal, Colors.Black, Colors.White);
            SetGlyph(TotalWidth - 1, Height - 1, 221);
        }

        /// <summary>
        /// Draws the frame of the Ranking, including the table dividers.
        /// </summary>
        private void DrawFrame()
        {
            int afterNameSpace = rankSpace + nameSpace;
            SetGlyph(0, 0, 222);
            for (int y = 1; y < Height - 1; y++)
            {
                SetGlyph(0, y, 179);
                SetGlyph(rankSpace, y, 179);
                SetGlyph(afterNameSpace, y, 179);
                SetGlyph(TotalWidth - 1, y, 179);
            }
            SetGlyph(TotalWidth - 1, 0, 221);

            SetGlyph(0, Height - 1, 192);
            for (int x = 1; x < TotalWidth - 1; x++)
                SetGlyph(x, Height - 1, 196);
            SetGlyph(rankSpace, Height - 1, 193);
            SetGlyph(afterNameSpace, Height - 1, 193);
            SetGlyph(TotalWidth - 1, Height - 1, 217);
        }

        /// <summary>
        /// Draws the header of the ranking.
        /// </summary>
        private void DrawHeader()
        {
            int offset = 1;

            Print(offset, 0, rankTitle, Colors.Black, Colors.White);
            offset += rankSpace;

            Print(offset, 0, nameTitle, Colors.Black, Colors.White);
            offset += nameSpace;

            Print(offset, 0, legacyTitle, Colors.Black, Colors.White);
        }

        /// <summary>
        /// Draws the ranking data within the table.
        /// </summary>
        /// <param name="update">The time passed since the last draw.</param>
        public override void Draw(System.TimeSpan update)
        {
            base.Draw(update);

            World.Character[] characters = World.RankedCharactersFor(0, tableRows);
            for (int c = 0; c < tableRows; c++)
            {
                int offset = 1;
                string rank = (c + 1).ToString();

                Print(offset, c + 1, rank);
                offset += rankSpace;

                Clear(offset, c + 1, nameSpace - 1);
                Clear(offset + nameSpace, c + 1, legacySpace - 1);
                if (c < characters.Length)
                {
                    string name = characters[c].Name;
                    string legacy = characters[c].Legacy.ToString();

                    ILineal lineal = characters[c] as ILineal;
                    if (lineal != null)
                    {
                        name = lineal.LineageName;
                        legacy += " [" + lineal.LegacyRecord + "]";
                    }

                    Print(offset, c + 1, name);
                    offset += nameSpace;

                    Print(offset, c + 1, legacy);
                }
            }
        }
    }
}
