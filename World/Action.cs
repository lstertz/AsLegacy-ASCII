using System;
using System.Collections.Generic;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines the Action interface for World-only accessible functionality.
        /// </summary>
        private interface IAction
        {
            /// <summary>
            /// Initiates any operations to be performed upon the cancellation of this Action.
            /// </summary>
            void Cancel();

            /// <summary>
            /// Updates the state of this Action and resolves it if the required activation 
            /// time has been satisfied or cancels it if the Action is no longer valid.
            /// </summary>
            /// <param name="timeDelta">The time, in milliseconds, that has passed 
            /// since the last update.</param>
            void Evaluate(int timeDelta);
        }

        /// <summary>
        /// Defines an Action, which encapsulates an action to be performed.
        /// </summary>
        public class Action : IAction
        {
            private readonly System.Action action;
            private readonly Func<bool> meetsConditions;
            private readonly LinkedListNode<IAction> node;

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
            /// <param name="requiredActivationTime">The time, in milliseconds, that must pass 
            /// before the action is to be performed.</param>
            /// <param name="conditionalCheck">A Func to ensure that required conditions to 
            /// resolve this Action continue to be met.</param>
            public Action(int requiredActivationTime, System.Action action,
                Func<bool> conditionalCheck = null)
            {
                this.action = action;
                this.requiredActivationTime = requiredActivationTime;

                meetsConditions = conditionalCheck;

                // Add to the head so new Actions during an Update aren't 
                // evaluated during that Update.
                node = actions.AddFirst(this);
            }

            void IAction.Evaluate(int timeDelta)
            {
                if (meetsConditions != null)
                    if (!meetsConditions())
                    {
                        Destroy();
                        return;
                    }

                passedActivationTime += timeDelta;
                if (passedActivationTime >= requiredActivationTime)
                {
                    action();
                    Destroy();
                }
            }

            void IAction.Cancel()
            {
                Destroy();
            }

            /// <summary>
            /// Performs any operations that should occur when this Action is to be destroyed.
            /// </summary>
            protected virtual void Destroy()
            {
                actions.Remove(node);
            }
        }

        public abstract partial class Character
        {
            /// <summary>
            /// Defines a Character Action, an Action that is performed by a Character.
            /// </summary>
            public class Action : World.Action
            {
                private readonly Character performer;

                /// <summary>
                /// Constructs a new Character Action.
                /// </summary>
                /// <param name="character">The performing Character.</param>
                /// <param name="action">The Action to be performed by the Character.</param>
                /// <param name="requiredActivationTime">The time, in milliseconds, that must pass 
                /// before the action is to be performed.</param>
                /// <param name="conditionalCheck">A Func to ensure that required conditions to 
                /// resolve this Action continue to be met.</param>
                public Action(Character performer, int requiredActivationTime, 
                    System.Action action, Func<bool> conditionalCheck = null) : base(
                         requiredActivationTime, action, conditionalCheck)
                {
                    this.performer = performer;
                    characterActions.Add(performer, this);
                }

                protected override void Destroy()
                {
                    base.Destroy();
                    characterActions.Remove(performer);
                }
            }

            /// <summary>
            /// Defines a Character TargetedAction, an Action that is performed by a Character,
            /// upon a targeted Character.
            /// </summary>
            public class TargetedAction : Action
            {
                /// <summary>
                /// Constructs a new Character TargetedAction.
                /// </summary>
                /// <param name="character">The performing Character.</param>
                /// <param name="action">The Action to be performed upon the targeted Character, 
                /// by the performing Character.</param>
                /// <param name="target">The targeted Character.</param>
                /// <param name="requiredActivationTime">The time, in milliseconds, that must pass 
                /// before the action is to be performed.</param>
                /// <param name="conditionalCheck">A Func to ensure that required conditions to 
                /// resolve this Action for the targeted Character continue to be met.</param>
                public TargetedAction(Character performer, Character target, 
                    int requiredActivationTime, Action<Character> action,
                    Func<Character, bool> conditionalCheck) : base(
                        performer, requiredActivationTime,
                        () => action(target),
                        () => { return conditionalCheck(target); })
                { }
            }
        }
    }
}
