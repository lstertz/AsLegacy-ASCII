using Microsoft.Xna.Framework;

using SadConsole.Components;
using System;
using Console = SadConsole.Console;

namespace AsLegacy
{
    public class Display : DrawConsoleComponent
    {
        private static Display display;

        public static void Init(Console console)
        {
            if (display == null)
                display = new Display(console);
        }

        private Console frame;
        private Console environment;
        private Console characters;
        private Console stats;
        private Console interaction;

        private Display(Console console)
        {
            // Create frame to outline around other child consoles.
            // Create stats for Player stats/inventory/equipment/legacy.
            // Create interaction for displaying/receiving commands.

            environment = new Console(World.columnCount, World.rowCount, World.Environment);
            environment.Position = new Point(1, 1);
            characters = new Console(World.columnCount, World.rowCount, World.Characters);
            characters.Position = new Point(1, 1);

            // TODO : 5 :: Create directions console.
            // TODO : 5 :: (Create) Add DirectionsHandler as a component.
            // TODO : 5 :: Bind Player to the DirectionsHandler.
            // TODO : 5 :: Update directions console with Player Directions.

            console.Children.Add(environment);
            console.Children.Add(characters);
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
