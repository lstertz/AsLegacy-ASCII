using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        protected class AbsentCharacter : Character
        {
            public AbsentCharacter(int col, int row) :
                base(col, row, Color.Transparent, Color.Transparent, 0, true)
            { }

            public AbsentCharacter(Character charToReplace) :
                base(charToReplace, Color.Transparent, Color.Transparent, 0, true)
            { }
        }
    }
}
