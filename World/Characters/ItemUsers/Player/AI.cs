using static AsLegacy.World;

namespace AsLegacy
{
    public partial class Player
    {
        /// <summary>
        /// Defines the AI for the Player Character.
        protected class AI : Character.IAI
        {
            /// <inheritdoc/>
            public void UpdateModeOf(Character character) { }

            /// <inheritdoc/>
            public void UpdateTargetOf(Character character) { }

            /// <inheritdoc/>
            public void InitiateActionFor(Character character) { }
        }
    }
}
