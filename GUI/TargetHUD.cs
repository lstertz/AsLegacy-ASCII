using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Components;
using SadConsole.Controls;
using SadConsole.Themes;

namespace AsLegacy.GUI
{
    /// <summary>
    /// Defines the TargetHUD aspect of a Display, 
    /// which is responsible for showing the details of a targeted Character.
    /// </summary>
    public class TargetHUD : Console
    {
        private const int height = 3;

        /// <summary>
        /// Constructs a new TargetHUD.
        /// </summary>
        /// <param name="width">The width of the TargetHUD.</param>
        public TargetHUD(int width) : base(width, height)
        {

        }
    }
}
