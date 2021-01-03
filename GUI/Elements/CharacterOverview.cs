using AsLegacy.Global;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using SadConsole.Input;
using System;

namespace AsLegacy.GUI.Elements 
{
    /// <summary>
    /// Defines a Character Overview GUI Element, which displays a brief summary of 
    /// the most pertinent details of a Character.
    /// </summary>
    public class CharacterOverview : ConsoleComponent
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
        private Rectangle box;

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

        public override void OnAdded(SadConsole.Console console)
        {
            base.OnAdded(console);
            box = new Rectangle(0, y, console.Width, Height);
        }

        /// <summary>
        /// Draws the Meter, with updated cell glyphs and header colors, to its Console.
        /// </summary>
        /// <param name="console">The Console to which this Meter draws.</param>
        /// <param name="delta">The time passed since the last draw.</param>
        public override void Draw(SadConsole.Console console, TimeSpan delta)
        {
            console.Clear(box);

            if (Character == null)
                return;

            console.Print(0, y, Character.Name);
            console.Print(0, y + 1, "[Action]");
            console.Print(0, y + 2, "[Target]");

            console.DrawLine(new Point(0, y + bottomFrameIndex), 
                new Point(console.Width - 1, y + bottomFrameIndex), 
                Colors.White, Colors.Black, bottomFrameGlyph);
        }

        public override void ProcessMouse(SadConsole.Console console, MouseConsoleState state, out bool handled)
        {
            handled = false;
            if (!box.Contains(state.ConsoleCellPosition))
                return;

            handled = true;
            if (state.Mouse.LeftClicked)
                Viewer.Target = Character;
            else
                World.Character.Highlight(Character);
        }


        #region Unused Overrides
        public override void Update(SadConsole.Console console, TimeSpan delta)
        {
            return;
        }

        public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
        {
            handled = false;
            return;
        }
        #endregion
    }
}
