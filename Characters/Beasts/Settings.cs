﻿using AsLegacy.Characters;

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
            /// Defines the default AI of a Beast.
            /// </summary>
            public override IAI AI => new BasicAI();

            /// <summary>
            /// Defines the default type of the Class of a Beast.
            /// </summary>
            public override Class.Type ClassType => Class.Type.Spellcaster; // TODO :: Update for appropriate Beast Classes.

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
