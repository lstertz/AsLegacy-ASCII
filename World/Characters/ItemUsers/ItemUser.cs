﻿using Microsoft.Xna.Framework;

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
        /// Defines BaseSettings, the basic attributes of an ItemUser, 
        /// prior to instance-specific adjustments.
        /// </summary>
        protected new class BaseSettings : World.Character.BaseSettings
        {
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
        /// <param name="row">The row position of the new Character.</param>
        /// <param name="column">The column position of the new Character.</param>
        /// <param name="name">The string name given to the new Character.</param>
        /// <param name="legacy">The starting legacy of the new Character.</param>
        public ItemUser(int row, int column, string name, int legacy) :
            this(row, column, name, new Lineage(name, legacy,
                (r, c, n, l) => new ItemUser(r, c, n, l)))
        {
        }
        private ItemUser(int row, int column, string name, Lineage lineage) :
            base(row, column, name, new BaseSettings(), lineage)
        {
            this.lineage = lineage;
        }

        /// <summary>
        /// Constructs a new ItemUser.
        /// </summary>
        /// <param name="row">The row position of the new Character.</param>
        /// <param name="column">The column position of the new Character.</param>
        /// <param name="name">The string name given to the new Character.</param>
        /// <param name="baseSettings">The base settings that define various 
        /// aspects of the new Character.</param>
        /// <param name="legacy">The starting legacy of the new Character.</param>
        protected ItemUser(int row, int column, 
            string name, BaseSettings baseSettings, int legacy) : 
            this(row, column, name, baseSettings, new Lineage(name, legacy, 
                (r, c, n, l) => new ItemUser(r, c, n, baseSettings, l)))
        {
        }
        private ItemUser(int row, int column, 
            string name, BaseSettings baseSettings, Lineage lineage) :
            base(row, column, name, baseSettings, lineage)
        {
            this.lineage = lineage;
        }

        protected override void Die()
        {
            base.Die();
            lineage.UponCurrentsDeath();
        }
    }
}
