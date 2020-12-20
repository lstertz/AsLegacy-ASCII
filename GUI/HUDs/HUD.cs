using AsLegacy.GUI.Elements;
using Microsoft.Xna.Framework;
using System;

using Console = SadConsole.Console;

namespace AsLegacy.GUI.HUDs
{
    /// <summary>
    /// Defines the HUD aspect of a Display, 
    /// which is responsible for showing the details of a focused Character.
    /// </summary>
    public abstract class HUD : Console
    {
        private const int height = 3;

        private int frameGlyph = 0;
        protected World.Character focus;

        private Meter healthMeter;

        /// <summary>
        /// Constructs a new HUD.
        /// The constructed HUD has a frame defined by the parameters.
        /// </summary>
        /// <param name="width">The width of the HUD.</param>
        /// <param name="frameGlyph">The glyph used for the frame.</param>
        /// <param name="focus">The focus of the HUD, and whose name will be 
        /// used as the title of the frame.</param>
        public HUD(int width, int frameGlyph, World.Character focus) : base(width, height)
        {
            this.frameGlyph = frameGlyph;
            this.focus = focus;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    SetBackground(x, y, Color.Black);
            SetFrame();

            healthMeter = new Meter(1, 2, RetrieveHealth, Color.Red, Color.DarkRed, 3);
            Components.Add(healthMeter);
        }

        /// <summary>
        /// Updates the visuals of this HUD.
        /// </summary>
        /// <param name="timeElapsed">The time passed since the last update.</param>
        public override void Update(TimeSpan timeElapsed)
        {
            base.Update(timeElapsed);

            // TODO :: Update other target details, if target is not null.
        }

        /// <summary>
        /// Sets the frame of this HUD, using the current focus's name 
        /// as the title of the frame, or with no title if the current focus is null.
        /// </summary>
        protected void SetFrame()
        {
            for (int x = 0; x < Width; x++)
                SetGlyph(x, 0, frameGlyph);

            if (focus != null)
                Print(1, 0, " " + focus.Name + " ");
        }

        /// <summary>
        /// Retrieves the health of the current focus of the HUD, if there is one.
        /// </summary>
        /// <returns>The health of the current focus, 0 otherwise.</returns>
        private float RetrieveHealth()
        {
            if (focus != null)
                return focus.Health;
            return 0.0f;
        }
    }
}
