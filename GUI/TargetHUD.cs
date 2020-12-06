using Microsoft.Xna.Framework;
using System;

using Console = SadConsole.Console;

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
        /// The last targeted Character, cached to prevent excess processing during updating.
        /// </summary>
        private World.Character lastTarget = null;

        /// <summary>
        /// Constructs a new TargetHUD.
        /// The constructed TargetHUD has a default frame and is not visible.
        /// </summary>
        /// <param name="width">The width of the TargetHUD.</param>
        public TargetHUD(int width) : base(width, height)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    SetBackground(x, y, Color.Black);

            SetFrame(null);
            IsVisible = false;
        }

        /// <summary>
        /// Updates the visuals of this TargetHUD.
        /// </summary>
        /// <param name="timeElapsed">The time passed since the last update.</param>
        public override void Update(TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);

            World.Character target = World.Player.Target;
            if (target != lastTarget)
            {
                SetFrame(target);
                IsVisible = target != null;

                lastTarget = target;
            }

            // TODO :: Update other target details, if target is not null.
        }

        /// <summary>
        /// Sets the frame of this TargetHUD, using the provided Character's name 
        /// as the title of the frame.
        /// </summary>
        /// <param name="target">The Character whose name will be the title 
        /// of the frame, null if there is not target and there should be no title.</param>
        private void SetFrame(World.Character target)
        {
            for (int x = 0; x < Width; x++)
                SetGlyph(x, 0, '-');

            if (target != null)
                Print(1, 0, " " + target.Name + " ");
        }
    }
}
