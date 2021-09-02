using Microsoft.Xna.Framework;
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
        /// Creates a new Player Character with a new Lineage, for a new game.
        /// </summary>
        /// <remarks>If a new game is being started after an existing game, then 
        /// <see cref="Reset"/> should first be called.</remarks>
        /// <param name="row">The row position of the new Player Character.</param>
        /// <param name="column">The column position of the new Player Character.</param>
        /// <param name="name">The name of the new Player Character.</param>
        /// <param name="lineageName">The name of the new Player's Lineage.</param>
        /// <exception cref="InvalidOperationException">Thrown if there is an existing 
        /// living Player Character.</exception>
        public static void Create(int row, int column, string name, string lineageName)
        {
            if (Character != null)
                throw new InvalidOperationException("The current Player Character is still " +
                    "active and cannot be replaced by a new instance.");

            Character = new Player(row, column, name, new Lineage(0, lineageName));
        }

        /// <summary>
        /// Creates a new Player Character as a successor to the current Player Character.
        /// </summary>
        /// <param name="name">The name of the succeeding Player Character.</param>
        /// <param name="initialPassiveInvestments">The succeeding Player Character's 
        /// initial investments in their Class' Passives.</param>
        /// <exception cref="InvalidOperationException">Thrown if there is no existing 
        /// Player Character or if the existing Player Character is still alive.</exception>
        public static void CreateSuccessor(string name, int[] initialPassiveInvestments = null)
        {
            if (Character == null)
                throw new InvalidOperationException("There is no existing Player from which to " +
                    "base a successor.");

            if (Character != null && Character.IsAlive)
                throw new InvalidOperationException("The current Player Character is still " +
                    "active and cannot be replaced by a new instance.");

            Lineage lineage = Character.CharacterLineage as Lineage;
            Point point = World.GetRandomPassablePosition(World.SpawnZone.Player);
            Character = new Player(point.Y, point.X, name, lineage, initialPassiveInvestments);
        }

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
        /// <param name="lineage">The Player's Lineage.</param>
        /// <param name="initialPassiveInvestments">The Player's initial investments in 
        /// their Class' Passives.</param>
        private Player(int row, int column, string name, Lineage lineage, 
            int[] initialPassiveInvestments = null) : 
            base(row, column, name, new Settings(initialPassiveInvestments), lineage)
        { }
    }
}
