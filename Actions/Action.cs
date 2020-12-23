using System;
using System.Collections.Generic;

namespace AsLegacy.Actions
{
    /// <summary>
    /// Defines an Action, which encapsulates an action to be performed.
    /// </summary>
    public class Action
    {
        private static LinkedList<Action> actions = new LinkedList<Action>();

        /// <summary>
        /// Defines an Action Manager, which evaluates valid Actions 
        /// until they are resolved and updates the tracking of them.
        /// </summary>
        public static class Manager
        {
            // TODO :: 18 : Attacking should create a TargetedAction.
            // TODO :: 18 : AsLegacy should call this Update to update.
            public static void Update(float timeDelta)
            {
                LinkedListNode<Action> next = actions.First;
                while (next != null)
                {
                    LinkedListNode<Action> current = next;
                    next = current.Next;

                    if (!current.Value.Evaluate(timeDelta))
                        actions.Remove(current);
                }
            }
        }


        private System.Action action;
        private Func<bool> meetsConditions;

        /// <summary>
        /// Provides the time until the Action is activated, as a percentage (0 - 1), 
        /// of the required activation time.
        /// </summary>
        public float Activation => passedActivationTime / requiredActivationTime;
        protected float passedActivationTime = 0.0f;
        protected float requiredActivationTime;


        /// <summary>
        /// Constructs a new Action.
        /// </summary>
        /// <param name="action">The Action to be performed upon resolution of this Action.</param>
        /// <param name="requiredActivationTime">The time that must pass before the 
        /// action is to be performed.</param>
        /// <param name="conditionalCheck">A Func to ensure that required conditions to 
        /// resolve this Action continue to be met.</param>
        public Action(System.Action action, float requiredActivationTime, 
            Func<bool> conditionalCheck = null)
        {
            this.action = action;
            this.requiredActivationTime = requiredActivationTime;

            meetsConditions = conditionalCheck;

            // Add to the head so new Actions during an Update aren't evaluated during that Update.
            actions.AddFirst(this);  
        }

        /// <summary>
        /// Updates the state of this Action and resolves it if the required activation 
        /// time has been satisfied.
        /// </summary>
        /// <param name="timeDelta">The time that has passed since the last update.</param>
        /// <returns>Whether this Action is still valid. If false, then 
        /// either the Action has been resolved or it has failed to meet the conditions 
        /// required for resolution.</returns>
        private bool Evaluate(float timeDelta)
        {
            if (meetsConditions != null)
                if (!meetsConditions())
                    return false;

            passedActivationTime += timeDelta;
            if (passedActivationTime >= requiredActivationTime)
            {
                action();
                return false;
            }

            return true;
        }
    }
}
