using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using System;
using System.Collections.Generic;

using AsLegacy.GUI.HUDs;
using AsLegacy.Input;
using AsLegacy.GUI.Popups;
using AsLegacy.Progression;

namespace AsLegacy.GUI.Screens
{
    /// <summary>
    /// Handles the initialization and visibility of the play screen, which servers as a 
    /// visual output controller for the active play rendering and GUI.
    /// </summary>
    [Behavior]
    [Dependency<ConsoleCollection>(Binding.Unique, Fulfillment.Existing, Consoles)]
    [Dependency<Display>(Binding.Unique, Fulfillment.Existing, Display)]
    [Dependency<GameState>(Binding.Unique, Fulfillment.Existing, State)]
    public class PlayScreenDisplaying : DrawConsoleComponent
    {
        private const string Consoles = "consoles";
        private const string Display = "display";
        private const string State = "state";

        public const int MapViewPortWidth = 20;
        public const int MapViewPortHeight = 20;
        public const int MapViewPortHalfWidth = MapViewPortWidth / 2;
        public const int MapViewPortHalfHeight = MapViewPortHeight / 2;

        private static PlayScreenDisplaying Screen;


        /// <summary>
        /// Whether the help popup is currently showing.
        /// </summary>
        public static bool IsShowingHelp => Screen._helpPopup.IsVisible;

        /// <summary>
        /// Whether the screen is currently showing a popup.
        /// </summary>
        public static bool IsShowingPopup =>
            Screen._helpPopup.IsVisible ||
            Screen._itemsPopup.IsVisible ||
            Screen._talentsPopup.IsVisible ||
            Screen._playerDeathPopup.IsVisible ||
            Screen._successorPopup.IsVisible;

        /// <summary>
        /// The current position and size of the map viewport.
        /// </summary>
        public static Rectangle MapViewPort => Screen._characters.ViewPort;


        /// <summary>
        /// Shows the PlayScreen's Help Popup.
        /// </summary>
        /// <param name="helpText">The text to be displayed in the popup.</param>
        /// <param name="requestingPopup">The popup requesting the help popup 
        /// to be displayed. This is referenced to orient the help popup respective to 
        /// its requester.</param>
        public static void ShowHelp(string helpText, Popup requestingPopup)
        {
            NotificationPopup help = Screen._helpPopup;

            help.Content = helpText;
            help.Position = new(
                requestingPopup.Position.X + requestingPopup.Width / 2 - help.Width / 2,
                requestingPopup.Position.Y + requestingPopup.Height / 2 - help.Height / 2);
            help.OnDismissal += () => requestingPopup.IsFocused = true;
            help.IsVisible = true;
        }

        /// <summary>
        /// Shows the PlayScreen's Items Popup.
        /// </summary>
        public static void ShowItems()
        {
            if (Screen._playerDeathPopup.IsVisible || Screen._successorPopup.IsVisible)
                return;

            Screen._itemsPopup.IsVisible = true;
            Screen._talentsPopup.IsVisible = false;
        }

        /// <summary>
        /// Shows the PlayScreen's Player Death Popup.
        /// </summary>
        public static void ShowPlayerDeath()
        {
            if (Screen._successorPopup.IsVisible)
                return;

            Screen._playerDeathPopup.IsVisible = true;

            Screen._itemsPopup.IsVisible = false;
            Screen._talentsPopup.IsVisible = false;
        }

        /// <summary>
        /// Shows the PlayScreen's Successor Popup.
        /// </summary>
        public static void ShowSuccessorDetails(string successorName)
        {
            if (!Screen._playerDeathPopup.IsVisible)
                return;

            Screen._successorPopup.SuccessorName = successorName;
            Screen._successorPopup.IsVisible = true;

            Screen._itemsPopup.IsVisible = false;
            Screen._talentsPopup.IsVisible = false;
            Screen._playerDeathPopup.IsVisible = false;
        }

        /// <summary>
        /// Shows the PlayScreen's Talents Popup.
        /// </summary>
        public static void ShowTalents()
        {
            if (Screen._playerDeathPopup.IsVisible || Screen._successorPopup.IsVisible)
                return;

            Screen._talentsPopup.IsVisible = true;
            Screen._itemsPopup.IsVisible = false;
        }


        /// <summary>
        /// Sets the frame, defined by the frame map, for the provided Console.
        /// </summary>
        /// <param name="console">The Console to which the frame will be added.
        /// It is expected that this Console's width/height will match the width/height 
        /// expected by the frame map.</param>
        private static void SetConsoleFrame(Console console)
        {
            int width = console.Width;
            int height = console.Height;
            Dictionary<int, Dictionary<int, int>> frameMap = new()
            {
                {0, new() {
                    {0, 201},
                    {height - 4, 204},
                    {height - 1, 200},
                    {-1, 186}
                } },
                {21, new() {
                    {0, 205},
                    {height - 1, 205},
                    {-1, 179}
                } },
                {width / 2, new()
                {
                    {0, 203},
                    {height - 4, 185},
                    {height - 1, 202},
                    {-1, 186}
                } },
                {width - 1, new()
                {
                    {0, 187},
                    {height - 1, 188},
                    {-1, 186}
                } },
                {-1, new Dictionary<int, int>()
                {
                    {0, 205},
                    {height - 1, 205}
                } }
            };

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


        private readonly NotificationPopup _helpPopup;
        private readonly Popup _itemsPopup;
        private readonly PlayerDeathPopup _playerDeathPopup;
        private readonly SuccessorPopup _successorPopup;
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

        private PlayScreenDisplaying()
        {
            // Workarounds for dependencies not being injected to constructors.
            Display display = GetContext<Display>();
            ConsoleCollection consoles = GetContext<ConsoleCollection>();
            GameState state = GetContext<GameState>();

            Screen = this;

            int width = display.Width;
            int height = display.Height;

            _console = new Console(width, height);
            consoles.ScreenConsoles.Add(_console);

            SetConsoleFrame(_console);

            _helpPopup = new("Help", width / 2, width / 2, height)
            {
                Position = new(MapViewPortHalfWidth + width / 2 + 1, height / 2),
                IsVisible = false
            };
            _itemsPopup = new ItemsPopup(width - MapViewPortWidth - 1, height)
            {
                Position = new Point(MapViewPortWidth + 1, 0),
                IsVisible = false
            };
            _playerDeathPopup = new PlayerDeathPopup(width / 2, height / 2)
            {
                Position = new Point(width / 4, height / 4),
                IsVisible = false
            };
            _successorPopup = new SuccessorPopup(width / 2, 3 * height / 4)
            {
                Position = new Point(width / 4, height / 8),
                IsVisible = false
            };
            _talentsPopup = new TalentsPopup(width - MapViewPortWidth - 1, height)
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

            _playerHUD = new FocusHUD(width / 2 - 1)
            {
                Position = new Point(1, height - 4)
            };

            _targetHUD = new TargetHUD(width / 2 - 1)
            {
                Position = new Point(1, height - 7)
            };

            _effects = World.EffectTiles;
            _effects.Position = new Point(1, 1);
            _effects.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            _effects.CenterViewPortOnPoint(new Point(0, 0));

            _environment = World.EnvironmentTiles;
            _environment.Position = new Point(1, 1);
            _environment.ViewPort = new Rectangle(0, 0, MapViewPortWidth, MapViewPortHeight);
            _environment.CenterViewPortOnPoint(new Point(0, 0));

            _nearbyPanel = new NearbyPanel(width / 2 - MapViewPortWidth - 2,
                MapViewPortHeight)
            {
                Position = new Point(MapViewPortWidth + 2, 1)
            };

            _characterPanel = new CharacterPanel(width / 2 - 2, height - 2)
            {
                Position = new Point(width / 2 + 1, 1)
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
            _console.Children.Add(_successorPopup);
            _console.Children.Add(_helpPopup);

            _console.Components.Add(this);
            _console.IsVisible = state.CurrentStage == GameStageMap.Stage.Play;
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
            if (!console.IsVisible)
                return;

            if (!IsShowingPopup)
                _commands.IsFocused = true;

            Point center = new(0, 0); // TODO :: Support disembodied center when there is no focus.
            if (GameExecution.Focus != null)
                center = GameExecution.Focus.Point;

            _characters.CenterViewPortOnPoint(center);
            _effects.CenterViewPortOnPoint(center);
            _environment.CenterViewPortOnPoint(center);

            _characterPanel.Draw(delta);
        }


        /// <summary>
        /// Updates whether this console is visible based on the current stage of the game.
        /// </summary>
        [Operation]
        [OnChange(State, nameof(GameState.CurrentStage))]
        public void UpdateVisibility(GameState state)
        {
            bool isVisible = state.CurrentStage == GameStageMap.Stage.Play;

            _console.IsVisible = isVisible;
            _commands.IsFocused = isVisible;

            if (!isVisible)
            {
                _helpPopup.IsVisible = false;
                _itemsPopup.IsVisible = false;
                _talentsPopup.IsVisible = false;
                _playerDeathPopup.IsVisible = false;
                _successorPopup.IsVisible = false;
            }
            else
                _successorPopup.Reset();

            Refresh();
        }
    }
}
