namespace AsLegacy
{
    /// <summary>
    /// A wrapper for a singleton instance of a Contextual Programming App, 
    /// enabling the app to be used globally.
    /// </summary>
    public static class App
    {
        private static readonly ContextualProgramming.App app = new();


        /// <summary>
        /// Initializes the Contextual Programming App.
        /// </summary>
        public static void Initialize() => app.Initialize();

        /// <summary>
        /// Updates the Contextual Programming App.
        /// </summary>
        public static void Update() => app.Update();


        /// <summary>
        /// Contextualizes a context for the Contextual Programming App.
        /// </summary>
        /// <typeparam name="T">The type of the context.</typeparam>
        /// <param name="context">The context to be contextualized.</param>
        public static void Contextualize<T>(T context) where T : class => 
            app.Contextualize(context);

        /// <summary>
        /// Decontextualizes a context for the Contextual Programming App.
        /// </summary>
        /// <typeparam name="T">The type of the context.</typeparam>
        /// <param name="context">The context to be decontextualized.</param>
        public static void Decontextualize<T>(T context) where T : class => 
            app.Decontextualize(context);

        /// <summary>
        /// Provides the first known context of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of context to be provided.</typeparam>
        /// <returns>The first known context of the specified type.</returns>
        public static T GetContext<T>() where T : class => app.GetContext<T>();

        /// <summary>
        /// Provides all known contexts of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of contexts to be provided.</typeparam>
        /// <returns>All known contexts of the specified type.</returns>
        public static T[] GetContexts<T>() where T : class => app.GetContexts<T>();
    }
}
