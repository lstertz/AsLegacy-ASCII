using System;
using System.Collections.Generic;
using System.Reflection;

namespace ContextualProgramming
{
    /// <summary>
    /// Handles the state and behavior of the running application by managing 
    /// registered <see cref="Context"/>s and the resulting behaviors 
    /// (<see cref="BehaviorAttribute"/>).
    /// </summary>
    public static class App
    {
        private static List<Context> _contexts = new();
        private static List<object> _independentBehaviors = new();

        /// <summary>
        /// Initializes the contextual execution of the application by registering all 
        /// declared behaviors (<see cref="BehaviorAttribute"/>).
        /// </summary>
        /// <remarks>Behaviors without any dependencies will be instantiated.</remarks>
        public static void Initialize()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int c = 0, count = types.Length; c < count; c++)
            {
                BehaviorAttribute attr = types[c].GetCustomAttribute<BehaviorAttribute>(true);
                if (attr == null)
                    continue;

                // TODO :: Check for dependencies.

                _independentBehaviors.Add(Activator.CreateInstance(types[c]));
            }
        }

        /// <summary>
        /// Registers a new context for the execution context of the application.
        /// </summary>
        /// <param name="context">The context to be registered.</param>
        public static void Register(Context context)
        {
            _contexts.Add(context);
        }
    }
}
