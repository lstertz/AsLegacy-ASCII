using AsLegacy.Characters;
using AsLegacy.Global;
using AsLegacy.GUI.Screens;
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

        private const int BottomFrameIndex = Height - 1;
        private const int BottomFrameGlyph = 196;

        /// <summary>
        /// The Character whose overview details are displayed by this CharacterOverview.
        /// Null if this overview should not display anything.
        /// </summary>
        public World.Character Character { get; set; }

        private readonly int _y;
        private Rectangle _box;

        /// <summary>
        /// Constructs a new CharacterOverview.
        /// </summary>
        /// <param name="y">The local y position of the CharacterOverview in its Console.</param>
        /// <param name="character">The World Character whose details are to be shown.</param>
        public CharacterOverview(int y, World.Character character = null)
        {
            _y = y;
            Character = character;
        }

        /// <inheritdoc/>
        public override void OnAdded(SadConsole.Console console)
        {
            base.OnAdded(console);
            _box = new Rectangle(0, _y, console.Width, Height);
        }

        /// <summary>
        /// Draws the Meter, with updated cell glyphs and header colors, to its Console.
        /// </summary>
        /// <param name="console">The Console to which this Meter draws.</param>
        /// <param name="delta">The time passed since the last draw.</param>
        public override void Draw(SadConsole.Console console, TimeSpan delta)
        {
            console.Clear(_box);

            if (Character == null)
                return;

            // TODO :: Refer to AsLegacy.Focus, comparing to World.Player when appropriate,
            //          for displaying details relevant to the viewer.

            string name = Character.Name;
            if (Character is ILineal lineal)
                name = lineal.FullName;

            Color foreground = Character.Selected ? Colors.Selected : 
                Character.Highlighted ? Colors.Highlighted : Colors.FadedWhite;
            console.Print(0, _y, name, foreground);
            console.Print(0, _y + 1, "[Action]", foreground);
            console.Print(0, _y + 2, "[Target]", foreground);

            console.DrawLine(new Point(0, _y + BottomFrameIndex), 
                new Point(console.Width - 1, _y + BottomFrameIndex), 
                Colors.White, Colors.Black, BottomFrameGlyph);
        }

        /// <inheritdoc/>
        public override void ProcessMouse(SadConsole.Console console, MouseConsoleState state, 
            out bool handled)
        {
            handled = false;
            if (PlayScreen.IsShowingPopup || !_box.Contains(state.ConsoleCellPosition))
            {
                World.Character.Highlight(null);
                return;
            }

            handled = true;
            if (state.Mouse.LeftClicked)
                AsLegacy.SelectCharacter(Character);
            else
                World.Character.Highlight(Character);
        }


        #region Unused Overrides
        /// <inheritdoc/>
        public override void Update(SadConsole.Console console, TimeSpan delta)
        {
            return;
        }

        /// <inheritdoc/>
        public override void ProcessKeyboard(SadConsole.Console console, Keyboard info, out bool handled)
        {
            handled = false;
            return;
        }
        #endregion
    }
}
