using AsLegacy.Characters;
using AsLegacy.GUI;
using AsLegacy.GUI.Elements;
using Microsoft.Xna.Framework;
using SadConsole.Components;
using SadConsole.Themes;
using System;
using Console = SadConsole.Console;
using Game = SadConsole.Game;

namespace AsLegacy;

/// <summary>
/// Handles the set up and updating of game systems.
/// </summary>
[Behavior]
[Dependency<ConsoleCollection>(Binding.Unique, Fulfillment.SelfCreated, Consoles)]
[Dependency<DisplayContext>(Binding.Unique, Fulfillment.SelfCreated, Display)]
[Dependency<GameState>(Binding.Unique, Fulfillment.SelfCreated, State)]
[Dependency<PlayerDetails>(Binding.Unique, Fulfillment.SelfCreated, PlayerDetails)]
public class GameExecution : UpdateConsoleComponent
{
    private const string Consoles = "consoles";
    private const string Display = "display";
    private const string State = "state";
    private const string PlayerDetails = "playerDetails";

    private static readonly string LocalFontsFile = System.IO.Path.Combine(
        "Resources",
        "Fonts",
        "AsLegacy.font");


    #region Remove through CP refactor

    /// <summary>
    /// The current Character being focused on by the game player.
    /// While playing, this is the World Player, when dead and in 'viewer mode' this may 
    /// be any other living Character.
    /// </summary>
    public static World.Character Focus { get => Player ?? ObservedCharacter; }
    private static World.Character ObservedCharacter = null;

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


    public GameExecution(out ConsoleCollection consoles, out DisplayContext display, 
        out GameState state, out PlayerDetails playerDetails)
    {
        playerDetails = new();
        consoles = new();
        display = new();
        state = new();

        Game.Create(display.Width, display.Height);
        Console console = new(display.Width, display.Height);
        consoles.PrimaryConsole.Value = console;

        Game.OnInitialize = () =>
        {
            Library.Default.SetControlTheme(typeof(ClearingTextBox), new TextBoxTheme());
            SadConsole.Global.LoadFont(LocalFontsFile);
            SadConsole.Global.FontDefault = SadConsole.Global.Fonts["AsLegacy"]
                .GetFont(SadConsole.Font.FontSizes.One);

            SadConsole.Global.CurrentScreen = console;
            console.Components.Add(this);
        };

        Game.Instance.Run();
        Game.Instance.Dispose();
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
    /// Updates player and world state to start the play stage of the game.
    /// </summary>
    [Operation]
    [OnChange(State, nameof(GameState.CurrentStage))]
    public void StartGameplay(GameState state, PlayerDetails playerDetails)
    {
        if (state.CurrentStage != GameState.Stage.Play)
            return;

        ObservedCharacter = null;
        Player.Reset();

        Class.Init();
        World.InitNewWorld();

        Point playerPosition = World.GetRandomPassablePosition(World.SpawnZone.Player);
        Player.Create(playerPosition.Y, playerPosition.X, playerDetails.CharacterName, 
            playerDetails.LineageName);

        _isOnPlayStage = true;
    }

    private bool _isOnPlayStage = false;
    /// <inheritdoc/>
    public override void Update(Console console, TimeSpan delta)
    {
        App.Update();

        // TODO :: Phase out the below through CP refactor.
        if (_isOnPlayStage)
            World.Update(delta.Milliseconds);

        /*
        if (World.HighestRankedCharacter.Legacy < Goal)
            World.Update(delta.Milliseconds);
        else if (Display.CurrentScreen != Display.Screen.Completion)
            Display.ShowScreen(Display.Screen.Completion);
        */
    }
}