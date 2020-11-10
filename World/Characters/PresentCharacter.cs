using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        public abstract class PresentCharacter : Character
        {
            protected PresentCharacter(Character charToReplace, Color glyphColor, int glyph) :
                base(charToReplace, Color.Black, glyphColor, glyph, false) { }
        }
    }
}
