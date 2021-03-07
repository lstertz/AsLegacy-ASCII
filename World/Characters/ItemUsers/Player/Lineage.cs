using AsLegacy.GUI.Screens;

namespace AsLegacy.Characters
{
    public partial class Player : ItemUser
    {
        /// <inheritdoc/>
        protected new class Lineage : ItemUser.Lineage
        {
            /// <inheritdoc/>
            protected override int spawnTime => CharacterRemovalTime;

            /// <inheritdoc/>
            public Lineage(string firstCharacterName, int initialLegacy, string name) : 
                base(firstCharacterName, initialLegacy, name) { }

            /// <summary>
            /// Shows the Player Death popup to let the Player decide whether to spawn 
            /// a successor, and the successor Character details if one is to be spawned.
            /// </summary>
            protected override void OnSpawnSuccessor()
            {
                PlayScreen.ShowPlayerDeath();
            }
        }
    }
}
