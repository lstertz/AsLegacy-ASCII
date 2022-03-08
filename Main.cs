global using static AsLegacy.App;
global using ContextualProgramming;

global using Console = SadConsole.Console;
using SadConsole;
using AsLegacy;


GameExecution.Initialize();

Game.Instance.Run();
Game.Instance.Dispose();