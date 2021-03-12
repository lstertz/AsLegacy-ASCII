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
            protected override void OnSpawnSuccessor()
            {
                Point point = World.GetRandomPassablePosition();
                new ItemUser(point.Y, point.X, CharacterName, new Settings(), this);
            }
        }
    }
}
