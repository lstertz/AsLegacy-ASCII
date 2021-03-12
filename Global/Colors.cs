using Microsoft.Xna.Framework;

namespace AsLegacy.Global
{
    /// <summary>
    /// The global cache of commonly used Colors and Color Themes.
    /// </summary>
    public static class Colors
    {
        public static SadConsole.Themes.Colors StandardTheme
        {
            get
            {
                SadConsole.Themes.Colors colors = SadConsole.Themes.Colors.CreateDefault();
                colors.ControlBack = Transparent;
                colors.Text = FadedWhite;
                colors.TextDark = White;
                colors.TextLight = White;
                colors.TextFocused = FadedWhite;
                colors.TextSelected = White;
                colors.TextSelectedDark = White;
                colors.ControlBackLight = Transparent;
                colors.ControlBackSelected = Transparent;
                colors.ControlBackDark = Transparent;
                colors.RebuildAppearances();
                return colors;
            }
        }

        public static readonly Color Highlighted = Color.White;
        public static readonly Color Selected = Color.SkyBlue;

        public static readonly Color FadedWhite = new Color(255, 255, 255, 235);
        public static readonly Color Transparent = Color.Transparent;
        public static readonly Color White = Color.White;
        public static readonly Color Black = Color.Black;
    }
}
