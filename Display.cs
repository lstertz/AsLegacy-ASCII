using Microsoft.Xna.Framework;

using SadConsole.Components;
using System;
using Console = SadConsole.Console;

namespace AsLegacy
{
    /// <summary>
    /// Defines Display, which serves as the primary visual output controller.
    /// </summary>
    public partial class Display : DrawConsoleComponent
    {
        private static Display display;

        public static void Init(Console console)
        {
            if (display == null)
                display = new Display(console);
        }

        private Console characters;
        private Console directions;
        private Console environment;
        private Console stats;

        private Display(Console console)
        {
            // Create frame to outline around other child consoles.
            // Create stats for Player stats/inventory/equipment/legacy.

            characters = World.Characters;
            characters.Position = new Point(1, 1);

            directions = new Console(3, 3, Directions.Cells);
            directions.Components.Add(new Directions());
            directions.Components.Add(new DirectionHandling());
            directions.IsFocused = true;

            environment = World.Environment;
            environment.Position = new Point(1, 1);

            console.Children.Add(environment);
            console.Children.Add(characters);
            characters.Children.Add(directions);
            console.Components.Add(this);
        }

        // TODO :: Update map viewport.
        // TODO :: Update stats console.

        public override void Draw(Console console, TimeSpan delta)
        {
            //console.Fill(ColorAnsi.White, ColorAnsi.White, 0);

            //environment.Fill(ColorAnsi.Red, ColorAnsi.Blue, 0);
            //characters.Fill(ColorAnsi.White, ColorAnsi.Blue.ClearAlpha(), 0);
            //environment.SetGlyph(1, 2, 9);
            //characters.SetGlyph(1, 1, 1, ColorAnsi.White, ColorAnsi.Black);
        }
    }
}
