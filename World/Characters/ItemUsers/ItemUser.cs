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
        protected ItemUser(int row, int column, Color glyphColor, int glyph, string name) : 
            base(row, column, glyphColor, glyph, name)
        {
        }
    }
}
