using AsLegacy.Characters;
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
        private const int DisplayHeight = 3;

        private readonly int _frameGlyph = 0;

        protected World.Character Character { get => _characterGetter(); }
        private readonly Func<World.Character> _characterGetter;

        private readonly Meter _activationMeter;
        private readonly Meter _cooldownMeter;
        private readonly Meter _healthMeter;

        /// <summary>
        /// Constructs a new HUD.
        /// The constructed HUD has a frame defined by the parameters.
        /// </summary>
        /// <param name="width">The width of the HUD.</param>
        /// <param name="frameGlyph">The glyph used for the frame.</param>
        /// <param name="characterGetter">Provides the character of the HUD, whose name will be 
        /// used as the title of the frame.</param>
        protected HUD(int width, int frameGlyph, Func<World.Character> characterGetter) : 
            base(width, DisplayHeight)
        {
            _frameGlyph = frameGlyph;
            _characterGetter = characterGetter;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < DisplayHeight; y++)
                    SetBackground(x, y, Color.Black);

            _healthMeter = new Meter(1, 2, RetrieveHealth, Color.Red, Color.DarkRed, 3);
            Components.Add(_healthMeter);

            _activationMeter = new Meter(14, 2, RetrieveActivation, Color.Goldenrod, 
                new Color(138, 105, 20, 255), 15);
            Components.Add(_activationMeter);

            _cooldownMeter = new Meter(27, 2, RetrieveCooldown, Color.CornflowerBlue, 
                Color.RoyalBlue, 247);
            Components.Add(_cooldownMeter);
        }

        /// <inheritdoc/>
        public override void Draw(TimeSpan timeElapsed)
        {
            base.Draw(timeElapsed);

            SetFrame();
        }

        /// <inheritdoc/>
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
                SetGlyph(x, 0, _frameGlyph);

            if (Character != null)
            {
                string name = Character.Name;
                if (Character is ILineal lineal)
                    name = lineal.FullName;

                Print(1, 0, " " + name + " ");
            }
        }

        /// <summary>
        /// Retrieves the activation progress of the action currently being 
        /// performed by the current character of the HUD, if there is one.
        /// </summary>
        /// <returns>The activation progress of the current action 
        /// of the current character, 0 otherwise.</returns>
        private float RetrieveActivation()
        {
            if (Character != null)
            {
                World.Action action = Character.CurrentAction;
                if (action != null)
                    return action.Activation;
            }
            return 0.0f;
        }

        /// <summary>
        /// Retrieves the cooldown of the current character of the HUD, if there is one.
        /// </summary>
        /// <returns>The cooldown of the current character, 0 otherwise.</returns>
        private float RetrieveCooldown()
        {
            if (Character != null)
                return Character.Cooldown;
            return 0.0f;
        }

        /// <summary>
        /// Retrieves the health of the current character of the HUD, if there is one.
        /// </summary>
        /// <returns>The health of the current character, 0 otherwise.</returns>
        private float RetrieveHealth()
        {
            if (Character != null)
                return Character.Health;
            return 0.0f;
        }
    }
}
