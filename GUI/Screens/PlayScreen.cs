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
        private static readonly Dictionary<int, Dictionary<int, int>> FrameMap =
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

        private static PlayScreen Screen;

        /// <summary>
        /// Whether the screen is currently showing a popup.
        /// </summary>
        public static bool IsShowingPopup => Screen._itemsPopup.IsVisible ||
            Screen._talentsPopup.IsVisible || Screen._playerDeathPopup.IsVisible;

        /// <summary>
        /// Whether the screen is currently visible.
        /// </summary>
        public static bool IsVisible
        {
            get => Screen._console.IsVisible;
            set
            {
                Screen._console.IsVisible = value;
                Screen._commands.IsFocused = value;

                if (!value)
                {
                    Screen._itemsPopup.IsVisible = false;
                    Screen._talentsPopup.IsVisible = false;
                    Screen._playerDeathPopup.IsVisible = false;
                }

                Screen.Refresh();
            }
        }

        /// <summary>
        /// The current position and size of the map viewport.
        /// </summary>
        public static Rectangle MapViewPort => Screen._characters.ViewPort;


        /// <summary>
        /// Initializes the PlayScreen.
        /// Required for any screen output to be rendered, or for any 
        /// child consoles to be created for interaction.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// initialized PlayScreen's Console's parent Console.</param>
        public static void Init(Console parentConsole)
        {
            if (Screen == null)
                Screen = new PlayScreen(parentConsole);
        }

        /// <summary>
        /// Shows the PlayScreen's Items Popup.
        /// </summary>
        public static void ShowItems()
        {
            if (Screen._playerDeathPopup.IsVisible)
                return;

            Screen._itemsPopup.IsVisible = true;
            Screen._talentsPopup.IsVisible = false;
        }

        /// <summary>
        /// Shows the PlayScreen's Player Death Popup.
        /// </summary>
        public static void ShowPlayerDeath()
        {
            Screen._playerDeathPopup.IsVisible = true;

            Screen._itemsPopup.IsVisible = false;
            Screen._talentsPopup.IsVisible = false;
        }

        /// <summary>
        /// Shows the PlayScreen's Talents Popup.
        /// </summary>
        public static void ShowTalents()
        {
            if (Screen._playerDeathPopup.IsVisible)
                return;

            Screen._talentsPopup.IsVisible = true;
            Screen._itemsPopup.IsVisible = false;
        }


        private readonly Popup _itemsPopup;
        private readonly PlayerDeathPopup _playerDeathPopup;
        private readonly Popup _talentsPopup;

        private readonly ScrollingConsole _characters;
        private readonly Console _commands;
        private readonly ScrollingConsole _effects;
        private readonly ScrollingConsole _environment;
        private readonly NearbyPanel _nearbyPanel;
        private readonly CharacterPanel _characterPanel;

        private readonly FocusHUD _playerHUD;
        private readonly TargetHUD _targetHUD;

        private readonly Console _console;

        /// <summary>
        /// Constructs a new PlayScreen for the given Console.
        /// </summary>
        /// <param name="parentConsole">The Console to become the 
        /// new PlayScreen's Console's parent Console.</param>
        private PlayScreen(Console parentConsole)
        {
            _console = new Console(Display.Width, Display.Height);
            parentConsole.Children.Add(_console);

            SetConsoleFrame(_console);

            _itemsPopup = new ItemsPopup(Display.Width - MapViewPortWidth - 1, Display.Height)
            {
                Position = new Point(MapViewPortWidth + 1, 0),
                IsVisible = false
            };
            _playerDeathPopup = new PlayerDeathPopup(Display.Width / 2, Display.Height / 2)
            {
                Position = new Point(Display.Width / 4, Display.Height / 4),
                IsVisible = false
            };
            _talentsPopup = new TalentsPopup(Display.Width - MapViewPortWidth - 1, Display.Height)
            {
                Position = new Point(MapViewPortWidth + 1, 0),
                IsVisible = false
            };

            _characters = World.CharacterTiles;
            _characters.Position = new Point(1, 1);
            _characters.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            _characters.CenterViewPortOnPoint(new Point(0, 0));
            _characters.Components.Add(new CharacterSelectionHandling());

            _commands = new Commands();
            _commands.Components.Add(new PlayerCommandHandling());
            _commands.IsFocused = true;

            _playerHUD = new FocusHUD(Display.Width / 2 - 1)
            {
                Position = new Point(1, Display.Height - 4)
            };

            _targetHUD = new TargetHUD(Display.Width / 2 - 1)
            {
                Position = new Point(1, Display.Height - 7)
            };

            _effects = World.EffectTiles;
            _effects.Position = new Point(1, 1);
            _effects.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            _effects.CenterViewPortOnPoint(new Point(0, 0));

            _environment = World.EnvironmentTiles;
            _environment.Position = new Point(1, 1);
            _environment.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            _environment.CenterViewPortOnPoint(new Point(0, 0));

            _nearbyPanel = new NearbyPanel(Display.Width / 2 - MapViewPortWidth - 2,
                MapViewPortHeight)
            {
                Position = new Point(MapViewPortWidth + 2, 1)
            };

            _characterPanel = new CharacterPanel(Display.Width / 2 - 2, Display.Height - 2)
            {
                Position = new Point(Display.Width / 2 + 1, 1)
            };

            _console.Children.Add(_environment);
            _console.Children.Add(_effects);
            _console.Children.Add(_characters);
            _characters.Children.Add(_commands);
            _console.Children.Add(_nearbyPanel);
            _console.Children.Add(_playerHUD);
            _console.Children.Add(_targetHUD);
            _console.Children.Add(_characterPanel);

            _console.Children.Add(_itemsPopup);
            _console.Children.Add(_talentsPopup);
            _console.Children.Add(_playerDeathPopup);

            _console.Components.Add(this);
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
                    if (FrameMap.ContainsKey(x))
                    {
                        if (FrameMap[x].ContainsKey(y))
                            console.SetGlyph(x, y, FrameMap[x][y]);
                        else if (FrameMap[x].ContainsKey(-1))
                            console.SetGlyph(x, y, FrameMap[x][-1]);
                    }
                    else if (FrameMap.ContainsKey(-1))
                    {
                        if (FrameMap[-1].ContainsKey(y))
                            console.SetGlyph(x, y, FrameMap[-1][y]);
                    }
                }
        }

        /// <summary>
        /// Forces a redraw of this PlayScreen and its children.
        /// </summary>
        public void Refresh()
        {
            _characterPanel.Refresh();
        }

        /// <summary>
        /// Updates the rendering of the PlayScreen, primarily by adjusting viewport centers.
        /// </summary>
        /// <param name="console">The Console of the PlayScreen.</param>
        /// <param name="delta">The time passed since the last Draw call.</param>
        public override void Draw(Console console, TimeSpan delta)
        {
            if (!IsVisible)
                return;

            if (!IsShowingPopup)
                _commands.IsFocused = true;

            Point center = new Point(0, 0); // TODO :: Support disembodied center when ther is no focus.
            if (AsLegacy.Focus != null)
                center = AsLegacy.Focus.Point;

            _characters.CenterViewPortOnPoint(center);
            _effects.CenterViewPortOnPoint(center);
            _environment.CenterViewPortOnPoint(center);

            _characterPanel.Draw(delta);
        }
    }
}
