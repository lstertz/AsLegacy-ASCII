using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using System;
using Console = SadConsole.Console;

namespace AsLegacy
{
    public partial class Display : DrawConsoleComponent
    {
        /// <summary>
        /// Defines the Directions aspect of a Display, 
        /// which is responsible for updating the directions 
        /// available for Player movement.
        /// </summary>
        public class Directions : DrawConsoleComponent
        {
            private static readonly Color fadedWhite = new Color(255, 255, 255, 240);

            private readonly Cell empty =
                new Cell(Color.Transparent, Color.Transparent);
            private readonly Cell up =
                new Cell(fadedWhite, Color.Transparent, 30);
            private readonly Cell right =
                new Cell(fadedWhite, Color.Transparent, 16);
            private readonly Cell down =
                new Cell(fadedWhite, Color.Transparent, 31);
            private readonly Cell left =
                new Cell(fadedWhite, Color.Transparent, 17);

            /// <summary>
            /// The Cells that make up the display of the 
            /// available directions.
            /// </summary>
            public Cell[] Cells { get; private set; }

            /// <summary>
            /// Constructs a new Directions display, which 
            /// initializes the Cells.
            /// </summary>
            public Directions()
            {
               Cells = new Cell[]
               {
                   empty, up, empty,
                   left, empty, right,
                   empty, down, empty
               };
            }

            /// <summary>
            /// Updates the cell display by changing the 
            /// visibility of the cells to indicate available movement.
            /// </summary>
            /// <param name="console">The Console to which the 
            /// Directions has been added as a component.</param>
            /// <param name="delta">The time that has passed
            /// since the last draw update.</param>
            public override void Draw(Console console, TimeSpan delta)
            {
            }
        }
    }
}
