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
        private Console interaction;

        private Display(Console console)
        {
            // Create frame to outline around other child consoles.
            // Create stats for Player stats/inventory/equipment/legacy.
            // Create interaction for displaying/receiving commands.

            characters = new Console(World.columnCount, World.rowCount, World.Characters);
            characters.Position = new Point(1, 1);

            Directions dirs = new Directions();
            directions = new Console(3, 3, dirs.Cells);
            directions.Position = new Point(World.Player.Column - 1, World.Player.Row - 1);
            directions.Components.Add(dirs);

            environment = new Console(World.columnCount, World.rowCount, World.Environment);
            environment.Position = new Point(1, 1);

            // TODO : 6 :: (Create) Add DirectionsHandler as a component.
            // TODO : 6 :: Bind Player to the DirectionsHandler.

            console.Children.Add(environment);
            console.Children.Add(characters);
            characters.Children.Add(directions);
            console.Components.Add(this);
        }

        // TODO :: Update map viewport.
        // TODO :: Update stats console.
        // TODO :: Update interaction console.

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
