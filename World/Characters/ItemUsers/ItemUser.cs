using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines an Item User, which is a Present Character that is 
    /// capable of holding and using Items.
    /// </summary>
    public abstract class ItemUser : World.PresentCharacter
    {
        protected ItemUser(int row, int column, Color glyphColor, int glyph) : 
            base(row, column, glyphColor, glyph)
        {
        }
    }
}
