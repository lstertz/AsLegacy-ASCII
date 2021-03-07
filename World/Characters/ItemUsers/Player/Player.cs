using System;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Player, the focal Character, on the World map, of the real-life player as 
    /// they interact with the game.
    /// </summary>
    public partial class Player : ItemUser
    {
        /// <summary>
        /// The current Player Character.
        /// Null if there is no active Player Character.
        /// </summary>
        public static Player Character { get; private set; } = null;

        /// <summary>
        /// Resets the known state of the Player, to where there is no active Player Character.
        /// </summary>
        public static void Reset()
        {
            Character = null;
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
        {
            if (Character != null && Character.IsAlive)
                throw new InvalidOperationException("The current Player Character is still " +
                    "active and cannot be replaced by a new instance.");

            Character = this;
        }
    }
}
