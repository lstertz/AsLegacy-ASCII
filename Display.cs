using Microsoft.Xna.Framework;

using SadConsole.Components;
using System;
using Console = SadConsole.Console;

namespace AsLegacy
{
    public partial class Display : DrawConsoleComponent
    {
        private static Display display;

        public static void Init(Console console)
        {
            if (display == null)
                display = new Display(console);
        }

        private Console frame;
        private Console characters;
        private Console directions;
        private Console environment;
        private Console stats;

        private Display(Console console)
        {
            // Create frame to outline around other child consoles.
            // Create stats for Player stats/inventory/equipment/legacy.

            characters = new Console(World.columnCount, World.rowCount, World.Characters);
            World.Characters.Bind(characters);
            characters.Position = new Point(1, 1);

            directions = new Console(3, 3, Directions.Cells);
            directions.Components.Add(new Directions());
            directions.Components.Add(new DirectionHandling());

            environment = new Console(World.columnCount, World.rowCount, World.Environment);
            World.Environment.Bind(environment);
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
