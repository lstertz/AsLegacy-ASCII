using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Item User, which is a Character that is 
    /// capable of holding and using Items, the functionality of which 
    /// is the responsibility of this class.
    /// </summary>
    public partial class ItemUser : World.Character, ILineal
    {
        protected new class Lineage : World.Character.Lineage
        {
            public Lineage(string firstCharacterName, int initialLegacy, string name) : 
                base(firstCharacterName, initialLegacy, name) { }

            protected override void OnSpawnSuccessor()
            {
                Point point = World.GetRandomPassablePosition();
                new ItemUser(point.Y, point.X, firstCharacterName, new Settings(), this);
            }
        }


        /// <inheritdoc/>
        public string FullName => Name + " of " + lineage.Name;

        /// <inheritdoc/>
        public string LineageName => lineage.Name;

        /// <summary>
        /// The highest recorded legacy of this Item User's Lineage, 
        /// represented as a numerical value (points)
        /// </summary>
        public override int LegacyRecord => lineage.LegacyRecord;

        /// <summary>
        /// The Lineage of this ItemUser.
        /// </summary>
        public ILineage CharacterLineage { get => lineage; }
        private readonly Lineage lineage;


        /// <summary>
        /// Constructs a new ItemUser.
        /// </summary>
        /// <param name="row">The row position of the ItemUser.</param>
        /// <param name="column">The column position of the ItemUser.</param>
        /// <param name="name">The string name given to the ItemUser.</param>
        /// <param name="legacy">The starting legacy of the ItemUser.</param>
        /// <param name="lineageName">The name of the ItemUser's Lineage.</param>
        public ItemUser(int row, int column, string name, int legacy, string lineageName) :
            this(row, column, name, new Settings(), legacy, lineageName)
        {
        }

        /// <summary>
        /// Constructs a new ItemUser.
        /// </summary>
        /// <param name="row">The row position of the ItemUser.</param>
        /// <param name="column">The column position of the ItemUser.</param>
        /// <param name="name">The string name given to the ItemUser.</param>
        /// <param name="baseSettings">The base settings that define various 
        /// aspects of the ItemUser.</param>
        /// <param name="legacy">The starting legacy of the ItemUser.</param>
        /// <param name="lineageName">The name of the ItemUser's Lineage.</param>
        protected ItemUser(int row, int column, 
            string name, Settings baseSettings, int legacy, string lineageName) : 
            this(row, column, name, baseSettings, new Lineage(name, legacy, 
                lineageName))
        {
        }
        protected ItemUser(int row, int column, 
            string name, Settings baseSettings, Lineage lineage) :
            base(row, column, name, baseSettings, lineage)
        {
            this.lineage = lineage;
        }

        /// <inheritdoc/>
        protected override void Die()
        {
            base.Die();
            lineage.UponCurrentsDeath();
        }
    }
}
