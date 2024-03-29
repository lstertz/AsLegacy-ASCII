﻿using AsLegacy.Characters;
using AsLegacy.GUI;
using AsLegacy.GUI.Elements;
using AsLegacy.Progression;
using SadConsole.Components;
using SadConsole.Themes;
using System;
using Game = SadConsole.Game;


namespace AsLegacy;

/// <summary>
/// Handles the set up and updating of game systems.
/// </summary>
[Behavior]
[Dependency<ConsoleCollection>(Binding.Unique, Fulfillment.Existing, Consoles)]
[Dependency<Display>(Binding.Unique, Fulfillment.Existing, Display)]
[Dependency<GameState>(Binding.Unique, Fulfillment.SelfCreated, State)]
public class GameExecution : UpdateConsoleComponent
{
    private const string Consoles = "consoles";
    private const string Display = "display";
    private const string State = "state";

    private static readonly string LocalFontsFile = System.IO.Path.Combine(
        "Resources",
        "Fonts",
        "AsLegacy.font");


    #region Remove through CP refactor

    /// <summary>
    /// The number of legacy points required for a game to be won.
    /// </summary>
    public const int Goal = 25;


    /// <summary>
    /// The current Character being focused on by the game player.
    /// While playing, this is the World Player, when dead and in 'viewer mode' this may 
    /// be any other living Character.
    /// </summary>
    public static World.Character Focus { get => Player ?? ObservedCharacter; }

    /// <summary>
    /// The character for the player to observer while they have no character of their own.
    /// </summary>
    /// <remarks>
    /// Temporarily exposed during CP refactor.
    /// </remarks>
    public static World.Character ObservedCharacter = null;

    /// <summary>
    /// Specifies whether the game has a current Player Character.
    /// </summary>
    public static bool HasPlayer => Player != null;

    /// <summary>
    /// The Player Character of the game.
    /// This is null if the Player is in 'viewer mode'.
    /// </summary>
    public static Player Player => Player.Character;


    /// <summary>
    /// Selects the provided Character, either as the target of 
    /// the Player Character or the focus Character of the game.
    /// </summary>
    /// <param name="character">The Character being selected.</param>
    public static void SelectCharacter(World.Character character)
    {
        if (Player != null)
            Player.Target = character;
        else
            ObservedCharacter = character;
    }

    #endregion


    /// <summary>
    /// Initializes the game with SadConsole, which executes within an embedded loop 
    /// after <see cref="Game.OnInitialize"/>.
    /// </summary>
    public static void Initialize()
    {
        ConsoleCollection consoles = new();
        Display display = new();

        int width = display.Width;
        int height = display.Height;
        ContextState<Console> primaryConsole = consoles.PrimaryConsole;

        Game.Create(width, height);

        Game.OnInitialize = () =>
        {
            Library.Default.SetControlTheme(typeof(ClearingTextBox), new TextBoxTheme());
            SadConsole.Global.LoadFont(LocalFontsFile);
            SadConsole.Global.FontDefault = SadConsole.Global.Fonts["AsLegacy"]
                .GetFont(SadConsole.Font.FontSizes.One);

            Console console = new(width, height);
            primaryConsole.Value = console;

            SadConsole.Global.CurrentScreen = console;

            App.Initialize();
            Contextualize(consoles);
            Contextualize(display);
        };
    }


    public GameExecution(out GameState state)
    {
        ConsoleCollection consoles = GetContext<ConsoleCollection>();
        state = new();

        consoles.PrimaryConsole.Value.Components.Add(this);
    }

    /// <summary>
    /// Exits the game if the state specifies that it should no longer continue running.
    /// </summary>
    [Operation]
    [OnChange(State, nameof(GameState.ContinueRunning))]
    public void CloseGame(GameState state)
    {
        //if (!state.ContinueRunning)
        // TODO :: Teardown and close the game.
    }


    // TODO :: Phase out most of the below operation through CP refactor.

    /// <summary>
    /// Updates whether the game is in its play stage.
    /// </summary>
    [Operation]
    [OnChange(State, nameof(GameState.CurrentStage))]
    public void EvaluateCurrentStage(GameState state)
    {
        _isOnPlayStage = state.CurrentStage == GameStageMap.Stage.Play;
    }
    private bool _isOnPlayStage = false;

    /// <inheritdoc/>
    public override void Update(Console console, TimeSpan delta)
    {
        App.Update();

        // TODO :: Phase out the below through CP refactor.
        //          World updating should be handled as part of the app update.
        if (_isOnPlayStage)
        {
            if (World.HighestRankedCharacter.Legacy < Goal)
                World.Update(delta.Milliseconds);
            else
                Contextualize(new GameEndMessage());
        }
    }
}