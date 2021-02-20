namespace AsLegacy
{
    public partial class World
    {
        public partial class Character
        {
            /// <summary>
            /// Defines the interface for classes that define certain settings of 
            /// a Character, as well as what action a Character should perform, 
            /// but not how the Character completes that that action.
            /// </summary>
            protected interface IAI
            {
                /// <summary>
                /// Determines and updates the Mode of the provided Character.
                /// </summary>
                /// <remarks>This should occur prior to the AI initiating an Action.</remarks>
                /// <param name="character">The Character whose Mode is being updated.</param>
                void UpdateModeOf(Character character);

                /// <summary>
                /// Determines and updates the Target of the provided Character.
                /// </summary>
                /// <remarks>This should occur prior to any other AI functionality.</remarks>
                /// <param name="character">The Character whose Target is being updated.</param>
                void UpdateTargetOf(Character character);

                /// <summary>
                /// Determines and initiates an Action for the provided Character to perform.
                /// </summary>
                /// <param name="character">The Character that will perform an Action determined 
                /// to be appropriate for its current state.</param>
                void InitiateActionFor(Character character);
            }
        }
    }
}
