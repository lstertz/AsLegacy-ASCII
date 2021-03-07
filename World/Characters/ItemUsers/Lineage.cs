using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    public partial class ItemUser
    {
        /// <inheritdoc/>
        protected new class Lineage : World.Character.Lineage
        {
            /// <inheritdoc/>
            public Lineage(string firstCharacterName, int initialLegacy, string name) : 
                base(firstCharacterName, initialLegacy, name) { }

            /// <summary>
            /// Spawns a new Item User to succeed the last of this Lineage.
            /// </summary>
            protected override void OnSpawnSuccessor()
            {
                Point point = World.GetRandomPassablePosition();
                new ItemUser(point.Y, point.X, firstCharacterName, new Settings(), this);
            }
        }
    }
}
