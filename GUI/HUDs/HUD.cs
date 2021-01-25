using AsLegacy.GUI.Elements;
using Microsoft.Xna.Framework;
using System;

using Console = SadConsole.Console;

namespace AsLegacy.GUI.HUDs
{
    /// <summary>
    /// Defines the HUD aspect of a Display, 
    /// which is responsible for showing the details of a specific Character.
    /// </summary>
    public abstract class HUD : Console
    {
        private const int height = 3;

        private readonly int frameGlyph = 0;

        protected World.Character character { get => characterGetter(); }
        private Func<World.Character> characterGetter;

        private readonly Meter activationMeter;
        private readonly Meter healthMeter;

        /// <summary>
        /// Constructs a new HUD.
        /// The constructed HUD has a frame defined by the parameters.
        /// </summary>
        /// <param name="width">The width of the HUD.</param>
        /// <param name="frameGlyph">The glyph used for the frame.</param>
        /// <param name="characterGetter">Provides the character of the HUD, whose name will be 
        /// used as the title of the frame.</param>
        public HUD(int width, int frameGlyph, Func<World.Character> characterGetter) : 
            base(width, height)
        {
            this.frameGlyph = frameGlyph;
            this.characterGetter = characterGetter;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    SetBackground(x, y, Color.Black);
            SetFrame();

            healthMeter = new Meter(1, 2, RetrieveHealth, Color.Red, Color.DarkRed, 3);
            Components.Add(healthMeter);

            activationMeter = new Meter(14, 2, RetrieveActivation, Color.Goldenrod, 
                new Color(138, 105, 20, 255), 15);
            Components.Add(activationMeter);
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
        /// Sets the frame of this HUD, using the current character's name 
        /// as the title of the frame, or with no title if the current character is null.
        /// </summary>
        protected void SetFrame()
        {
            for (int x = 0; x < Width; x++)
                SetGlyph(x, 0, frameGlyph);

            if (character != null)
                Print(1, 0, " " + character.Name + " ");
        }

        /// <summary>
        /// Retrieves the activation progress of the action currently being 
        /// performed by the current character of the HUD, if there is one.
        /// </summary>
        /// <returns>The activation progress of the current action 
        /// of the current character, 0 otherwise.</returns>
        private float RetrieveActivation()
        {
            if (character != null)
            {
                World.Action action = character.CurrentAction;
                if (action != null)
                    return action.Activation;
            }
            return 0.0f;
        }

        /// <summary>
        /// Retrieves the health of the current character of the HUD, if there is one.
        /// </summary>
        /// <returns>The health of the current character, 0 otherwise.</returns>
        private float RetrieveHealth()
        {
            if (character != null)
                return character.Health;
            return 0.0f;
        }
    }
}
