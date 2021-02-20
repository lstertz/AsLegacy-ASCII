namespace AsLegacy
{
    public partial class Beast : World.Character
    {
        /// <summary>
        /// Defines the settings of a Beast.
        /// </summary>
        protected abstract class Settings : BaseSettings
        {
            /// <summary>
            /// The initial legacy of the Beast.
            /// </summary>
            public abstract Combat.Legacy InitialLegacy { get; }

            /// <summary>
            /// The name of the Beast.
            /// </summary>
            public abstract string Name { get; }
        }
    }
}
