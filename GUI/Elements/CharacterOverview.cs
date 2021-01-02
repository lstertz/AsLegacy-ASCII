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
        /// <summary>
        /// The total height of the overview.
        /// </summary>
        public const int Height = 4;

        private const int bottomFrameIndex = Height - 1;
        private const int bottomFrameGlyph = 196;

        /// <summary>
        /// The Character whose overview details are displayed by this CharacterOverview.
        /// Null if this overview should not display anything.
        /// </summary>
        public World.Character Character { get; set; }

        /// <summary>
        /// The expected Character (of the game player/viewer) that is viewing 
        /// this CharacterOverview. The exact details or display may be tailored to be 
        /// more meaningful from the perspective of this Character.
        /// </summary>
        public World.Character Viewer { get; set; }

        private readonly int y;

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
            console.Clear(new Rectangle(0, y, console.Width, Height));

            if (Character == null)
                return;

            console.Print(0, y, Character.Name);
            console.Print(0, y + 1, "[Action]");
            console.Print(0, y + 2, "[Target]");

            console.DrawLine(new Point(0, y + bottomFrameIndex), 
                new Point(console.Width - 1, y + bottomFrameIndex), 
                Color.White, Color.Black, bottomFrameGlyph);
        }
    }
}
