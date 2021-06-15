using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines an Effect, a graphical and practical application of a performed 
        /// <see cref="Characters.Skills.Skill"/>.
        /// </summary>
        private abstract class Effect
        {
            /// <summary>
            /// The graphical color of the effect.
            /// </summary>
            public Color Color { get; init; }

            /// <summary>
            /// The <see cref="Effect"/> to be performed after the resolution of this effect.
            /// </summary>
            public Effect Followup { get; init; } = null;

            /// <summary>
            /// The point at which the effect begins.
            /// </summary>
            public Point Origin { get; init; }

            /// <summary>
            /// The <see cref="Character"/> performer of the effect.
            /// </summary>
            public Character Performer { get; init; }

            /// <summary>
            /// The point that the effect is targeting.
            /// </summary>
            /// <remarks>If there isn't a particular target point, this should 
            /// be the same as the <see cref="Origin"/>.</remarks>
            public Point Target { get; init; }

            private IAction _action = null;
            private HashSet<Point> _graphicLocations = new();

            /// <summary>
            /// Clears the graphics of the effect, essentially erasing any current 
            /// visual changes it is applying to the <see cref="World"/>.
            /// </summary>
            protected void ClearGraphics()
            {
                foreach(Point p in _graphicLocations)
                    Effects.ReplaceWith(p.Y, p.X, new EffectGraphic(Color.Transparent, ' '));

                _graphicLocations.Clear();
            }

            /// <summary>
            /// Sets the graphic of the effect for the specific position.
            /// </summary>
            /// <param name="x">The x location of the position.</param>
            /// <param name="y">The y location of the position.</param>
            /// <param name="glyph">The graphic to be set.</param>
            protected void SetGraphic(int x, int y, int glyph)
            {
                Effects.ReplaceWith(y, x, new EffectGraphic(Color, glyph));
                _graphicLocations.Add(new(x, y));
            }

            /// <summary>
            /// Starts the updating of this effect.
            /// </summary>
            public void Start()
            {
                _action = new Action(0, OnEffectUpdate, null, true);
            }

            /// <summary>
            /// Stops the updating of this effect, and clears its graphics.
            /// </summary>
            public void Stop()
            {
                ClearGraphics();

                _action.Cancel();
                _action = null;
            }

            /// <summary>
            /// Updates the effect's graphics and potential practical results.
            /// </summary>
            /// <param name="timeDelta">The time passed since the last update.</param>
            protected abstract void Update(int timeDelta);

            /// <summary>
            /// Initiates an update call.
            /// </summary>
            private void OnEffectUpdate()
            {
                Update(_action.LastTimeDelta);
            }
        }
    }
}
