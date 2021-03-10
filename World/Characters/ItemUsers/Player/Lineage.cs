using AsLegacy.GUI.Screens;
using System;

namespace AsLegacy.Characters
{
    public partial class Player : ItemUser
    {
        /// <inheritdoc/>
        protected new class Lineage : ItemUser.Lineage
        {
            /// <inheritdoc/>
            protected override int spawnTime => CharacterRemovalTime;

            private Action<World.Character> uponSpawn;

            /// <inheritdoc/>
            public Lineage(int initialLegacy, string name) : base(initialLegacy, name) { }

            /// <summary>
            /// Shows the Player Death popup to let the Player decide whether to spawn 
            /// a successor, and the successor Character details if one is to be spawned.
            /// </summary>
            /// <param name="uponSpawn">The callback to be called once a successor has 
            /// been spawned, with the succeeding Character provided as a parameter.</param>
            protected override void OnSpawnSuccessor(Action<World.Character> uponSpawn)
            {
                this.uponSpawn = uponSpawn;

                PlayScreen.ShowPlayerDeath();
            }

            /// <summary>
            /// Wrapper for the uponSpawn callback that should be called 
            /// once a successor has been created.
            /// </summary>
            /// <param name="successor">The successor that was created.</param>
            public void UponSuccessorCreation(World.Character successor)
            {
                uponSpawn?.Invoke(successor);
                uponSpawn = null;
            }
        }
    }
}
