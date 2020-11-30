﻿using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using SadConsole.Controls;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Defines a map of the frame for the main console.
        /// The map is structured as {x, {y, glyph} }, where a -1 for x or y indicates that 
        /// the glyph applies for all values of that axis.
        /// </summary>
        private static readonly Dictionary<int, Dictionary<int, int>> frameMap =
            new Dictionary<int, Dictionary<int, int>>()
            {
                {0, new Dictionary<int, int>() {
                    {0, 201}, 
                    {AsLegacy.Height - 1, 200}, 
                    {-1, 186} 
                } },
                {AsLegacy.Width / 2, new Dictionary<int, int>()
                {
                    {0, 203},
                    {AsLegacy.Height - 1, 202},
                    {-1, 186}
                } },
                {AsLegacy.Width - 1, new Dictionary<int, int>()
                {
                    {0, 187},
                    {AsLegacy.Height - 1, 188},
                    {-1, 186}
                } },
                {-1, new Dictionary<int, int>()
                {
                    {0, 205},
                    {AsLegacy.Height - 1, 205}
                } }
            };

        private static Display display;

        /// <summary>
        /// Initializes the Display.
        /// Required for any display output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="console">The Console to become the initialized Display's Console.</param>
        public static void Init(Console console)
        {
            if (display == null)
                display = new Display(console);
        }

        private ScrollingConsole characters;
        private Console playerCommands;
        private ScrollingConsole environment;
        private ControlsConsole characterPanel;

        private Rectangle MapViewPort => characters.ViewPort;

        /// <summary>
        /// Constructs a new Display for the given Console.
        /// </summary>
        /// <param name="console">The Console to be the new Display's Console.</param>
        private Display(Console console)
        {
            SetConsoleFrame(console);
            // Create stats for Player stats/inventory/equipment/legacy.

            characters = World.Characters;
            characters.Position = new Point(1, 1);
            characters.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            characters.CenterViewPortOnPoint(World.Player.Point);

            playerCommands = new Console(3, 3, PlayerCommands.Cells);
            playerCommands.Components.Add(new PlayerCommands());
            playerCommands.Components.Add(new PlayerCommandHandling());
            playerCommands.IsFocused = true;

            environment = World.Environment;
            environment.Position = new Point(1, 1);
            environment.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            environment.CenterViewPortOnPoint(World.Player.Point);

            characterPanel = new CharacterPanel(AsLegacy.Width / 2 - 2, AsLegacy.Height - 2);
            characterPanel.Position = new Point(AsLegacy.Width / 2 + 1, 1);

            console.Children.Add(environment);
            console.Children.Add(characters);
            characters.Children.Add(playerCommands);
            console.Children.Add(characterPanel);

            console.Components.Add(this);
        }

        /// <summary>
        /// Sets the frame, defined by the frame map, for the provided Console.
        /// </summary>
        /// <param name="console">The Console to which the frame will be added.
        /// It is expected that this Console's width/height will match the width/height 
        /// expected by the frame map.</param>
        private void SetConsoleFrame(Console console)
        {
            int width = console.Width;
            int height = console.Height;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (frameMap.ContainsKey(x))
                    {
                        if (frameMap[x].ContainsKey(y))
                            console.SetGlyph(x, y, frameMap[x][y]);
                        else if (frameMap[x].ContainsKey(-1))
                            console.SetGlyph(x, y, frameMap[x][-1]);
                    }
                    else if (frameMap.ContainsKey(-1))
                    {
                        if (frameMap[-1].ContainsKey(y))
                            console.SetGlyph(x, y, frameMap[-1][y]);
                    }
                }
        }

        /// <summary>
        /// Updates the rendering of the Display, primarily by adjusting viewport centers.
        /// </summary>
        /// <param name="console">The Console of the display.</param>
        /// <param name="delta">The time passed since the last Draw call.</param>
        public override void Draw(Console console, TimeSpan delta)
        {
            characters.CenterViewPortOnPoint(World.Player.Point);
            environment.CenterViewPortOnPoint(World.Player.Point);
        }
    }
}