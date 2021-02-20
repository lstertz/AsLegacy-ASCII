using Microsoft.Xna.Framework;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Item User, which is a Character that is 
    /// capable of holding and using Items, the functionality of which 
    /// is the responsibility of this class.
    /// </summary>
    public class ItemUser : World.Character, ILineal
    {
        /// <summary>
        /// Defines Settings, the basic attributes of an ItemUser, 
        /// prior to instance-specific adjustments.
        /// </summary>
        protected class Settings : BaseSettings
        {
            /// <summary>
            /// Defines the default AI of an ItemUser.
            /// </summary>
            public override IAI AI => new BasicAI();

            /// <summary>
            /// Defines the color of an ItemUser's Glyphs.
            /// </summary>
            public override Color GlyphColor => Color.DarkSalmon;
            /// <summary>
            /// Defines the glyph to be shown when an ItemUser is in attack mode.
            /// </summary>
            public override int AttackGlyph => 231;//'τ'
            /// <suary>
            /// Defines the glyph to be shown when an ItemUser is in defend mode.
            /// </summary>
            public override int DefendGlyph => 232;//'φ'
            /// <suary>
            /// Defines the glyph to be shown when an ItemUser is in normal mode.
            /// </summary>
            public override int NormalGlyph => 227;//'π'

            /// <summary>
            /// The initial base attack damage of an ItemUser.
            /// </summary>
            public override float InitialAttackDamage => 1.67f;
            /// <summary>
            /// The initial base attack interval of an ItemUser.
            /// </summary>
            public override int InitialAttackInterval => 2000;
            /// <summary>
            /// The initial base maximum health of an ItemUser.
            /// </summary>
            public override float InitialBaseMaxHealth => 10.0f;
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
        public new ILineage Lineage { get => lineage; }
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
            this(row, column, name, new Lineage(name, legacy, lineageName,
                (r, c, n, l) => new ItemUser(r, c, n, l)))
        {
        }
        private ItemUser(int row, int column, string name, Lineage lineage) :
            base(row, column, name, new Settings(), lineage)
        {
            this.lineage = lineage;
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
            this(row, column, name, baseSettings, new Lineage(name, legacy, lineageName, 
                (r, c, n, l) => new ItemUser(r, c, n, baseSettings, l)))
        {
        }
        private ItemUser(int row, int column, 
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
