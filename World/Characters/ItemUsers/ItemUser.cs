using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines an Item User, which is a Character that is 
    /// capable of holding and using Items, the functionality of which 
    /// is the responsibility of this class.
    /// </summary>
    public abstract class ItemUser : World.Character
    {
        /// <summary>
        /// Constructs a new ItemUser.
        /// </summary>
        /// <param name="row">The row position of the new Character.</param>
        /// <param name="column">The column position of the new Character.</param>
        /// <param name="glyphColor">The color of the glyph visually displayed to represent 
        /// the new Character.</param>
        /// <param name="glyph">The glyph visually displayed to represent 
        /// the new Character.</param>
        /// <param name="name">The string name given to the new Character.</param>
        /// <param name="legacy">The starting legacy of the new Character.</param>
        protected ItemUser(int row, int column, Color glyphColor, int glyph, 
            string name, int legacy) : base(row, column, glyphColor, glyph, name, legacy)
        {
        }
    }
}
