using System.Collections.Generic;

namespace ContextualProgramming
{
    /// <summary>
    /// Handles the state and behavior of the running application by managing 
    /// registered <see cref="Context"/>s and the resulting behaviors.
    /// </summary>
    public static class App
    {
        private static List<Context> _contexts = new();

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
