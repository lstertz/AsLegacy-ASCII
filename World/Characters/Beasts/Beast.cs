using Microsoft.Xna.Framework;

namespace AsLegacy
{
    /// <summary>
    /// Defines a Beast, which is a Present Character that cannot 
    /// use Items and has a number of nature-based skills and abilities.
    /// </summary>
    public class Beast : World.PresentCharacter
    {
        private const int normal = 224;

        protected override int attackGlyph => 229;

        protected override int defendGlyph => 239;

        protected override int normalGlyph => normal;

        public Beast(int row, int column, string name) : 
            base(row, column, Color.DarkOrange, normal, name)
        {
        }
    }
}
