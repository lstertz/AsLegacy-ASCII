using System;

namespace AsLegacy.Actions
{
    /// <summary>
    /// Defines a TargetedAction, and Action that targets a Character.
    /// </summary>
    public class TargetedAction : Action
    {
        /// <summary>
        /// Constructs new TargetedAction.
        /// </summary>
        /// <param name="action">The Action to be performed for a targeted Character.</param>
        /// <param name="target">The targeted Character.</param>
        /// <param name="requiredActivationTime">The time that must pass before the 
        /// action is to be performed.</param>
        /// <param name="conditionalCheck">A Func to ensure that required conditions to 
        /// resolve this Action for the targeted Character continue to be met.</param>
        public TargetedAction(Action<World.Character> action, World.Character target,
            float requiredActivationTime, Func<World.Character, bool> conditionalCheck) : base(
                () => action(target), requiredActivationTime, 
                () => { return conditionalCheck(target); }) { }
    }
}
