using AsLegacy.GUI.Screens;
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
                PlayScreen.ShowPlayerDeath();
            }
        }
    }
}
