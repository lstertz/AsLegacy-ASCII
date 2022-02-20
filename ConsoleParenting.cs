using AsLegacy.GUI;

namespace AsLegacy;

/// <summary>
/// Handles the parenting of consoles.
/// </summary>
[Behavior]
[Dependency<ConsoleCollection>(Binding.Unique, Fulfillment.Existing, Consoles)]
public class ConsoleParenting
{
    // TODO :: Replace ConsoleCollection with collective dependencies on contexts that 
    //          wrap the specific type of console, e.g. PrimaryConsole[], ScreenConsole[], etc.
    //          Then operations can be made specific to created/destroyed console contexts.


    private const string Consoles = "consoles";


    [Operation]
    [OnChange(Consoles, nameof(ConsoleCollection.ScreenConsoles))]
    public void ParentNewScreens(ConsoleCollection consoles)
    {
        for (int c = 0, count = consoles.ScreenConsoles.Count; c < count; c++)
            if (!consoles.PrimaryConsole.Value.Children.Contains(consoles.ScreenConsoles[c]))
                consoles.PrimaryConsole.Value.Children.Add(consoles.ScreenConsoles[c]);
    }
}
