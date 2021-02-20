namespace AsLegacy
{
    public partial class World
    {
        public partial class Character
        {
            /// <summary>
            /// Defines the BasicAI that performs minimal AI functionality for any Character.
            protected class BasicAI : IAI
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
}
