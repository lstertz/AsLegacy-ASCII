using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Beast, which is a Character that cannot 
    /// use Items and has a number of nature-based skills and abilities.
    /// </summary>
    public class Beast : World.Character
    {
        private const int normal = 224;

        /// <summary>
        /// Defines the glyph to be shown when the Beast is in attack mode.
        /// </summary>
        protected override int AttackGlyph => 229;

        /// <summary>
        /// Defines the glyph to be shown when the Beast is in defend mode.
        /// </summary>
        protected override int DefendGlyph => 239;

        /// <summary>
        /// Defines the glyph to be shown when the Beast is in normal mode.
        /// </summary>
        protected override int NormalGlyph => normal;


        /// <summary>
        /// The maximum health of this Beast.
        /// </summary>
        public override float MaxHealth => 6.0f;

        /// <summary>
        /// Constructs a new Beast at the provided row and column on the map.
        /// </summary>
        /// <param name="row">The row position of the new Beast.</param>
        /// <param name="column">The column position of the new Beast.</param>
        /// <param name="name">The name of the new Beast.</param>
        /// <param name="legacy">The starting legacy of the new Beast.</param>
        public Beast(int row, int column, string name, int legacy) : 
            base(row, column, Color.DarkOrange, normal, name, legacy)
        {
        }
    }
}
