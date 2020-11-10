using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        public class Player : PresentCharacter
        {
            public Player(int col, int row) :
                base(GetAbsentCharacter(col, row), Color.White, '@')
            {

            }
        }
    }
}
