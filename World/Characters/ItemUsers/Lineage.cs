using Microsoft.Xna.Framework;
using System;

namespace AsLegacy.Characters
{
    public partial class ItemUser
    {
        /// <inheritdoc/>
        protected new class Lineage : World.Character.Lineage
        {
            /// <inheritdoc/>
            public Lineage(int initialLegacy, string name) : base(initialLegacy, name) { }

            /// <summary>
            /// Spawns a new Item User to succeed the last of this Lineage.
            /// </summary>
            /// <param name="uponSpawn">The callback to be called once a successor has 
            /// been spawned, with the succeeding Character provided as a parameter.</param>
            protected override void OnSpawnSuccessor(Action<World.Character> uponSpawn)
            {
                Point point = World.GetRandomPassablePosition();
                uponSpawn(new ItemUser(point.Y, point.X, CharacterName, new Settings(), this));
            }
        }
    }
}
