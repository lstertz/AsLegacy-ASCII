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
        private static readonly Dictionary<Type, List<Context>> Contexts = new();
        private static readonly List<object> IndependentBehaviors = new();


        /// <summary>
        /// Initializes the contextual execution of the application by registering all 
        /// declared behaviors (<see cref="BehaviorAttribute"/>).
        /// </summary>
        /// <remarks>
        /// Behaviors without any dependencies will be instantiated.
        /// </remarks>
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
            Type type = context.GetType();

            Type[] interfaces = type.GetInterfaces();
            for (int c = 0, count = interfaces.Length; c < count; c++)
                Register(type, context);

            while (!type.IsAbstract && type != typeof(Context))
            {
                Register(type, context);
                type = type.BaseType;
            }
        }


        /// <summary>
        /// Registers a context of the specified type for the execution context of the application.
        /// </summary>
        /// <param name="type">The type identifying the context.</param>
        /// <param name="context">The context to be registered.</param>
        private static void Register(Type type, Context context)
        {
            if (!Contexts.ContainsKey(type))
                Contexts.Add(type, new());

            Contexts[type].Add(context);
        }
    }
}
