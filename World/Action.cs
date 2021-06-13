using System;
using System.Collections.Generic;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines the Action interface for World-only accessible functionality.
        /// </summary>
        protected interface IAction
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
            /// <summary>
            /// Provides the time until the Action is activated, as a percentage (0 - 1), 
            /// of the required activation time.
            /// </summary>
            public float Activation => _passedActivationTime / _requiredActivationTime;
            private float _passedActivationTime = 0.0f;
            private readonly float _requiredActivationTime;

            private readonly System.Action _action;
            private readonly Func<bool> _meetsConditions;
            private readonly LinkedListNode<IAction> _node;
            private readonly bool _repeats;


            /// <summary>
            /// Constructs a new Action.
            /// </summary>
            /// <param name="requiredActivationTime">The time, in milliseconds, that must pass 
            /// before the action is to be performed.</param>
            /// <param name="repeats">Whether the Action re-initializes 
            /// itself after is resolves.</param>
            /// <param name="action">The Action to be performed upon resolution of this Action.</param>
            /// <param name="conditionalCheck">A Func to ensure that required conditions to 
            /// resolve this Action continue to be met.</param>
            public Action(int requiredActivationTime, System.Action action,
                Func<bool> conditionalCheck = null, bool repeats = false)
            {
                _action = action;
                _requiredActivationTime = requiredActivationTime;
                _repeats = repeats;

                _meetsConditions = conditionalCheck;

                // Add to the head so new Actions during an Update aren't 
                // evaluated during that Update.
                _node = Actions.AddFirst(this);
            }

            void IAction.Evaluate(int timeDelta)
            {
                if (_meetsConditions != null)
                    if (!_meetsConditions())
                    {
                        Destroy();
                        return;
                    }

                _passedActivationTime += timeDelta;
                if (_passedActivationTime >= _requiredActivationTime)
                {
                    _action();

                    if (_repeats)
                    {
                        _passedActivationTime -= _requiredActivationTime;
                        while (_passedActivationTime >= _requiredActivationTime)
                        {
                            _action();
                            _passedActivationTime -= _requiredActivationTime;
                        }
                    }
                    else
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
                if (Actions.Contains(_node.Value))
                    Actions.Remove(_node);
            }
        }

        public abstract partial class Character
        {
            /// <summary>
            /// Defines a Character Action, an Action that is performed by a Character.
            /// </summary>
            public class Action : World.Action
            {
                private readonly Character _performer;

                /// <summary>
                /// Constructs a new Character Action.
                /// </summary>
                /// <param name="character">The performing Character.</param>
                /// <param name="requiredActivationTime">The time, in milliseconds, that must pass 
                /// before the action is to be performed.</param>
                /// <param name="repeats">Whether the Action re-initializes 
                /// itself after is resolves.</param>
                /// <param name="action">The Action to be performed by the Character.</param>
                /// <param name="conditionalCheck">A Func to ensure that required conditions to 
                /// resolve this Action continue to be met.</param>
                public Action(Character performer, int requiredActivationTime,
                    System.Action action, Func<bool> conditionalCheck = null, 
                    bool repeats = false) : base(
                        requiredActivationTime, action, conditionalCheck, repeats)
                {
                    _performer = performer;

                    if (CharacterActions.ContainsKey(performer))
                    {
                        (CharacterActions[performer] as IAction).Cancel();
                        CharacterActions.Remove(performer);
                    }

                    CharacterActions.Add(performer, this);
                }

                protected override void Destroy()
                {
                    base.Destroy();
                    CharacterActions.Remove(_performer);
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
                /// <param name="performer">The performing Character.</param>
                /// <param name="target">The targeted Character.</param>
                /// <param name="requiredActivationTime">The time, in milliseconds, that must pass 
                /// before the action is to be performed.</param>
                /// <param name="repeats">Whether the Action re-initializes 
                /// itself after is resolves.</param>
                /// <param name="action">The Action to be performed upon the targeted Character,
                /// provided as a parameter, by the performing Character.</param>
                /// <param name="conditionalCheck">A Func to ensure that required conditions to 
                /// resolve this Action for the targeted Character, provided as a parameter,
                /// continue to be met.</param>
                public TargetedAction(Character performer, Character target,
                    int requiredActivationTime, Action<Character> action,
                    Func<Character, bool> conditionalCheck, bool repeats = false) : base(
                        performer, requiredActivationTime,
                        () => action(target),
                        () => { return conditionalCheck(target); },
                        repeats)
                { }
            }
        }
    }
}
