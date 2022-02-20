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

        private const string RankTitle = "Rank ";
        private readonly int RankSpace = RankTitle.Length;
        private const string NameTitle = "Lineage          ";
        private readonly int NameSpace = NameTitle.Length;
        private const string LegacyTitle = "Legacy      ";
        private readonly int LegacySpace = LegacyTitle.Length;

        private readonly int TableRows;

        /// <summary>
        /// Constructs a new Ranking.
        /// </summary>
        /// <param name="height">The height of the Ranking.</param>
        /// <param name="displayGoal">Specifies whether the footer of the Ranking should 
        /// be drawn to display the goal.</param>
        public Ranking(int height, bool displayGoal = false) : base(TotalWidth, height)
        {
            TableRows = height - 2;

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
            string goal = System.Convert.ToString(GameExecution.Goal);

            SetGlyph(0, Height - 1, 222);
            Fill(1, Height - 1, TotalWidth - 2, Colors.Transparent, Colors.White, 0);
            Print(RankSpace + NameSpace + 1, Height - 1, "Goal", Colors.Black, Colors.White);
            Print(TotalWidth - goal.Length - 1, Height - 1, goal, Colors.Black, Colors.White);
            SetGlyph(TotalWidth - 1, Height - 1, 221);
        }

        /// <summary>
        /// Draws the frame of the Ranking, including the table dividers.
        /// </summary>
        private void DrawFrame()
        {
            int afterNameSpace = RankSpace + NameSpace;
            SetGlyph(0, 0, 222);
            for (int y = 1; y < Height - 1; y++)
            {
                SetGlyph(0, y, 179);
                SetGlyph(RankSpace, y, 179);
                SetGlyph(afterNameSpace, y, 179);
                SetGlyph(TotalWidth - 1, y, 179);
            }
            SetGlyph(TotalWidth - 1, 0, 221);

            SetGlyph(0, Height - 1, 192);
            for (int x = 1; x < TotalWidth - 1; x++)
                SetGlyph(x, Height - 1, 196);
            SetGlyph(RankSpace, Height - 1, 193);
            SetGlyph(afterNameSpace, Height - 1, 193);
            SetGlyph(TotalWidth - 1, Height - 1, 217);
        }

        /// <summary>
        /// Draws the header of the ranking.
        /// </summary>
        private void DrawHeader()
        {
            int offset = 1;

            Print(offset, 0, RankTitle, Colors.Black, Colors.White);
            offset += RankSpace;

            Print(offset, 0, NameTitle, Colors.Black, Colors.White);
            offset += NameSpace;

            Print(offset, 0, LegacyTitle, Colors.Black, Colors.White);
        }

        /// <summary>
        /// Draws the ranking data within the table.
        /// </summary>
        /// <param name="update">The time passed since the last draw.</param>
        public override void Draw(System.TimeSpan update)
        {
            base.Draw(update);

            World.Character[] characters = World.RankedCharactersFor(0, TableRows);
            for (int c = 0; c < TableRows; c++)
            {
                int offset = 1;
                string rank = (c + 1).ToString();

                Print(offset, c + 1, rank);
                offset += RankSpace;

                Clear(offset, c + 1, NameSpace - 1);
                Clear(offset + NameSpace, c + 1, LegacySpace - 1);
                if (c < characters.Length)
                {
                    string name = characters[c].Name;
                    string legacy = characters[c].Legacy.ToString();

                    if (characters[c] is ILineal lineal)
                    {
                        name = lineal.LineageName;
                        legacy += " [" + lineal.LegacyRecord + "]";
                    }

                    Print(offset, c + 1, name);
                    offset += NameSpace;

                    Print(offset, c + 1, legacy);
                }
            }
        }
    }
}
