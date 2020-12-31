using Microsoft.Xna.Framework;
using SadConsole.Components;
using System;

namespace AsLegacy.GUI.Elements
{
    /// <summary>
    /// Defines a Character Overview GUI Element, which displays a brief summary of 
    /// the most pertinent details of a Character.
    /// </summary>
    public class CharacterOverview : DrawConsoleComponent
    {
        public const int OverviewHeight = 4;

        private const int bottomFrameIndex = OverviewHeight - 1;
        private const int bottomFrameGlyph = 196;

        public World.Character Character { get; set; }
        public World.Character Viewer { get; set; }

        private int y;

        /// <summary>
        /// Constructs a new CharacterOverview.
        /// </summary>
        /// <param name="y">The local y position of the CharacterOverview in its Console.</param>
        /// <param name="viewer">The World Character who is viewing the overview.</param>
        /// <param name="character">The World Character whose details are to be shown.</param>
        public CharacterOverview(int y, World.Character viewer, 
            World.Character character = null)
        {
            this.y = y;

            Character = character;
            Viewer = viewer;
        }

        /// <summary>
        /// Draws the Meter, with updated cell glyphs and header colors, to its Console.
        /// </summary>
        /// <param name="console">The Console to which this Meter draws.</param>
        /// <param name="delta">The time passed since the last draw.</param>
        public override void Draw(SadConsole.Console console, TimeSpan delta)
        {
            // TODO :: Use actual Character data, or hide entirely if Character is null.
            console.Print(0, y, "Character Name");
            console.Print(0, y + 1, "Character Action"); // TODO :: Center
            console.Print(0, y + 2, "Character Target"); // TODO :: Center

            console.DrawLine(new Point(0, y + bottomFrameIndex), 
                new Point(console.Width - 1, y + bottomFrameIndex), 
                Color.White, Color.Black, bottomFrameGlyph);
        }
    }
}
