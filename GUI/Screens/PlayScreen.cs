using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using System;
using System.Collections.Generic;
using Console = SadConsole.Console;

using AsLegacy.GUI.HUDs;
using AsLegacy.Input;
using AsLegacy.GUI.Popups;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Defines PlayScreen, which serves as visual output controller for the active play 
    /// rendering and GUI.
    /// </summary>
    public class PlayScreen : DrawConsoleComponent
    {
        public const int MapViewPortWidth = 20;
        public const int MapViewPortHeight = 20;
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
                    {Display.Height - 4, 204},
                    {Display.Height - 1, 200},
                    {-1, 186}
                } },
                {21, new Dictionary<int, int>() {
                    {0, 205},
                    {Display.Height - 1, 205},
                    {-1, 179}
                } },
                {Display.Width / 2, new Dictionary<int, int>()
                {
                    {0, 203},
                    {Display.Height - 4, 185},
                    {Display.Height - 1, 202},
                    {-1, 186}
                } },
                {Display.Width - 1, new Dictionary<int, int>()
                {
                    {0, 187},
                    {Display.Height - 1, 188},
                    {-1, 186}
                } },
                {-1, new Dictionary<int, int>()
                {
                    {0, 205},
                    {Display.Height - 1, 205}
                } }
            };

        private static PlayScreen screen;

        /// <summary>
        /// The current position and size of the map viewport.
        /// </summary>
        public static Rectangle MapViewPort => screen.characters.ViewPort;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static bool IsVisible
        {
            get => screen.console.IsVisible;
            set
            {
                screen.console.IsVisible = value;
                screen.commands.IsFocused = value;
            }
        }

        /// <summary>
        /// Shows the PlayScreen's Items Popup.
        /// </summary>
        public static void ShowItems()
        {
            screen.itemsPopup.IsVisible = true;
            screen.skillsPopup.IsVisible = false;
        }

        /// <summary>
        /// Shows the PlayScreen's Skills Popup.
        /// </summary>
        public static void ShowSkills()
        {
            screen.skillsPopup.IsVisible = true;
            screen.itemsPopup.IsVisible = false;
        }

        private readonly Popup itemsPopup;
        private readonly Popup skillsPopup;

        private readonly ScrollingConsole characters;
        private readonly Console commands;
        private readonly ScrollingConsole environment;
        private readonly NearbyPanel nearbyPanel;
        private readonly CharacterPanel characterPanel;

        private readonly FocusHUD playerHUD;
        private readonly TargetHUD targetHUD;

        private readonly Console console;


        /// <summary>
        /// Initializes the PlayScreen.
        /// Required for any screen output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// initialized PlayScreen's Console's parent Console.</param>
        public static void Init(Console parentConsole)
        {
            if (screen == null)
                screen = new PlayScreen(parentConsole);
        }

        /// <summary>
        /// Constructs a new PlayScreen for the given Console.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// new PlayScreen's Console's parent Console.</param>
        private PlayScreen(Console parentConsole)
        {
            console = new Console(Display.Width, Display.Height);
            parentConsole.Children.Add(console);

            SetConsoleFrame(console);
            // Create stats for Player stats/inventory/equipment/legacy.

            itemsPopup = new Popup("Items", "Items to be managed here.",
                Display.Width - MapViewPortWidth - 1, Display.Height)
            {
                Position = new Point(MapViewPortWidth + 1, 0),
                IsVisible = false
            };
            skillsPopup = new Popup("Skills", "Skills to be managed here.",
                Display.Width - MapViewPortWidth - 1, Display.Height)
            {
                Position = new Point(MapViewPortWidth + 1, 0),
                IsVisible = false
            };

            characters = World.Characters;
            characters.Position = new Point(1, 1);
            characters.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            characters.CenterViewPortOnPoint(AsLegacy.Focus.Point);
            characters.Components.Add(new PlayerTargetHandling());

            commands = new Commands();
            commands.Components.Add(new PlayerCommandHandling());
            commands.IsFocused = true;

            playerHUD = new FocusHUD(Display.Width / 2 - 1)
            {
                Position = new Point(1, Display.Height - 4)
            };

            targetHUD = new TargetHUD(Display.Width / 2 - 1)
            {
                Position = new Point(1, Display.Height - 7)
            };

            environment = World.Environment;
            environment.Position = new Point(1, 1);
            environment.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            environment.CenterViewPortOnPoint(AsLegacy.Focus.Point);

            nearbyPanel = new NearbyPanel(Display.Width / 2 - MapViewPortWidth - 2,
                MapViewPortHeight)
            {
                Position = new Point(MapViewPortWidth + 2, 1)
            };

            characterPanel = new CharacterPanel(Display.Width / 2 - 2, Display.Height - 2)
            {
                Position = new Point(Display.Width / 2 + 1, 1)
            };

            console.Children.Add(environment);
            console.Children.Add(characters);
            characters.Children.Add(commands);
            console.Children.Add(nearbyPanel);
            console.Children.Add(playerHUD);
            console.Children.Add(targetHUD);
            console.Children.Add(characterPanel);

            console.Children.Add(itemsPopup);
            console.Children.Add(skillsPopup);

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
        /// Updates the rendering of the PlayScreen, primarily by adjusting viewport centers.
        /// </summary>
        /// <param name="console">The Console of the PlayScreen.</param>
        /// <param name="delta">The time passed since the last Draw call.</param>
        public override void Draw(Console console, TimeSpan delta)
        {
            characters.CenterViewPortOnPoint(AsLegacy.Focus.Point);
            environment.CenterViewPortOnPoint(AsLegacy.Focus.Point);
        }
    }
}
