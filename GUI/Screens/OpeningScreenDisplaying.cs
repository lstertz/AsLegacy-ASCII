﻿using Microsoft.Xna.Framework;
using SadConsole;

using AsLegacy.Global;
using SadConsole.Controls;
using AsLegacy.Configs;
using AsLegacy.Progression;

namespace AsLegacy.GUI.Screens;

/// <summary>
/// Handles the initialization and visibility of the start screen.
/// </summary>
[Behavior]
[Dependency<ConfigurationSet>(Binding.Unique, Fulfillment.Existing, Configurations)]
[Dependency<ConsoleCollection>(Binding.Unique, Fulfillment.Existing, Consoles)]
[Dependency<Display>(Binding.Unique, Fulfillment.Existing, Display)]
[Dependency<GameState>(Binding.Unique, Fulfillment.Existing, State)]
public class OpeningScreenDisplaying : ControlsConsole
{
    private const string Configurations = "configurations";
    private const string Consoles = "consoles";
    private const string Display = "display";
    private const string State = "state";

    private const int GameModeInitialYOffset = -8;
    private const int GameModeOffsetShift = 2;

    private const string TitleMessage = "As Legacy";
    private const int TitleY = 10;

    /// <summary>
    /// Sets up the opening screen.
    /// </summary>
    public OpeningScreenDisplaying() :
        // Workaround for dependencies not injected to constructors.
        base(GetContext<Display>().Width, GetContext<Display>().Height)
    {
        // More workarounds for dependencies not being injected to constructors.
        ConfigurationSet configurations = GetContext<ConfigurationSet>();
        ConsoleCollection consoles = GetContext<ConsoleCollection>();

        ThemeColors = Colors.StandardTheme;
        consoles.ScreenConsoles.Add(this);

        SetUpTitle();
        SetUpConfigurationButtons(configurations);
    }

    /// <summary>
    /// Sets up the configuration buttons of the opening screen.
    /// </summary>
    private void SetUpConfigurationButtons(ConfigurationSet configurations)
    {
        string[] gameConfigs = configurations.ConfigurationNames;
        for (int c = 0, count = gameConfigs.Length; c < count; c++)
        {
            int option = c;

            int width = gameConfigs[option].Length + 2;
            int yOffset = GameModeInitialYOffset + (GameModeOffsetShift * option);
            string config = gameConfigs[option];

            Button button = new(width, 1)
            {
                Position = new Point(Width / 2 - width / 2, Height + yOffset),
                Text = config
            };
            button.Click += (s, e) => Contextualize(new GameStartMessage()
            {
                SelectedConfigurationOption = option
            });
            Add(button);
        }
    }

    /// <summary>
    /// Sets up the title of the opening screen.
    /// </summary>
    private void SetUpTitle()
    {
        Add(new Label(Width)
        {
            Alignment = HorizontalAlignment.Center,
            DisplayText = TitleMessage,
            Position = new Point(0, TitleY),
            TextColor = Colors.White
        });
    }


    /// <summary>
    /// Updates whether this console is visible based on the current stage of the game.
    /// </summary>
    /// <remarks>
    /// The console will gain focus if it becomes visible and lose focus if it is hidden.
    /// </remarks>
    [Operation]
    [OnChange(State, nameof(GameState.CurrentStage))]
    public void UpdateVisibility(GameState state)
    {
        bool isVisible = state.CurrentStage == GameStageMap.Stage.Opening;

        IsVisible = isVisible;
        IsFocused = isVisible;
    }
}
