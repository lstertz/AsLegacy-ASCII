using Microsoft.Xna.Framework;

using SadConsole;
using SadConsole.Components;
using SadConsole.Debug;
using SadConsole.Input;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Console = SadConsole.Console;

namespace AsLegacy
{
    public class Display : DrawConsoleComponent
    {
        private static Display display;

        public static void Init(Console console, World world)
        {
            if (display == null)
                display = new Display(console, world);
        }

        private Console frame;
        private Console environment;
        private Console characters;
        private Console stats;
        private Console interaction;

        private Display(Console console, World world)
        {
            // Create frame to outline around other child consoles.
            // Create stats for Player stats/inventory/equipment/legacy.
            // Create interaction for displaying/receiving commands.

            environment = new Console(World.columnCount, World.rowCount, 
                world.environment.CastTo((element) => { return (Cell)element; }));
            environment.Position = new Point(1, 1);
            characters = new Console(World.columnCount, World.rowCount,
                world.characters.CastTo((element) => { return (Cell)element; }));
            characters.Position = new Point(1, 1);
            

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
