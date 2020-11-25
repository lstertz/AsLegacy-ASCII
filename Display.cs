using Microsoft.Xna.Framework;
using SadConsole;
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
        public const int MapViewPortWidth = 6;
        public const int MapViewPortHeight = 6;
        public const int MapViewPortHalfWidth = MapViewPortWidth / 2;
        public const int MapViewPortHalfHeight = MapViewPortHeight / 2;

        private static Display display;

        public static void Init(Console console)
        {
            if (display == null)
                display = new Display(console);
        }

        private ScrollingConsole characters;
        private Console directions;
        private ScrollingConsole environment;
        private Console stats;

        private Rectangle MapViewPort => characters.ViewPort;

        private Display(Console console)
        {
            // Create frame to outline around other child consoles.
            // Create stats for Player stats/inventory/equipment/legacy.

            characters = World.Characters;
            characters.Position = new Point(1, 1);
            characters.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            characters.CenterViewPortOnPoint(World.Player.Point);

            directions = new Console(3, 3, PlayerCommands.Cells);
            directions.Components.Add(new PlayerCommands());
            directions.Components.Add(new PlayerCommandHandling());
            directions.IsFocused = true;

            environment = World.Environment;
            environment.Position = new Point(1, 1);
            environment.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            environment.CenterViewPortOnPoint(World.Player.Point);

            console.Children.Add(environment);
            console.Children.Add(characters);
            characters.Children.Add(directions);
            console.Components.Add(this);
        }

        // TODO :: Update map viewport.
        // TODO :: Update stats console.

        public override void Draw(Console console, TimeSpan delta)
        {
            characters.CenterViewPortOnPoint(World.Player.Point);
            environment.CenterViewPortOnPoint(World.Player.Point);
        }
    }
}
