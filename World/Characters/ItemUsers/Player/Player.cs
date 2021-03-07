using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Player, the focal Character, on the World map, of the real-life player as 
    /// they interact with the game.
    /// </summary>
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
                // TODO :: Update AsLegacy's Player instance.
                Point point = World.GetRandomPassablePosition();
                new Player(point.Y, point.X, firstCharacterName, this);
            }
        }


        /// <summary>
        /// Specifies the target of the Player.
        /// The target will be selected, and will be the recipient of 
        /// certain actions performed by the Player.
        /// </summary>
        public override World.Character Target
        {
            get => base.Target;
            set
            {
                UpdateSelected(base.Target, value);
                base.Target = value;
            }
        }


        /// <summary>
        /// Constructs a new Player at the provided row and column on the map.
        /// </summary>
        /// <param name="row">The row position of the new Player.</param>
        /// <param name="column">The column position of the new Player.</param>
        /// <param name="name">The name of the current Player Character.</param>
        /// <param name="lineageName">The name of the Player's Lineage.</param>
        public Player(int row, int column, string name, string lineageName) :
            this(row, column, name, new Lineage(name, 0, lineageName))
        { }

        private Player(int row, int column, string name, Lineage lineage) : 
            base(row, column, name, new Settings(), lineage)
        { }
    }
}
