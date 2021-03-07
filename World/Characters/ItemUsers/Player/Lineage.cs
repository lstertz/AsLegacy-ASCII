using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    public partial class Player : ItemUser
    {
        protected new class Lineage : ItemUser.Lineage
        {
            protected override int spawnTime => CharacterRemovalTime;

            public Lineage(string firstCharacterName, int initialLegacy, string name) : 
                base(firstCharacterName, initialLegacy, name) { }

            protected override void OnSpawnSuccessor()
            {
                // TODO :: Show pop-up, add the below as an action for 'Continue'.
                Point point = World.GetRandomPassablePosition();
                new Player(point.Y, point.X, firstCharacterName, this);
            }
        }
    }
}
