using AsLegacy.Characters.Skills;
using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines the expanding ring effect, which is an <see cref="Effect"/> that simulates 
        /// an expanding ring (an area of effect) up to a specified range, 
        /// of some elemental type, dealing damage to those caught in the ring.
        /// </summary>
        private class ExpandingRingEffect : Effect
        {
            private const int TimePerRangeUnit = 500;

            public float BaseDamage { get; init; }

            public Skill.Element Element { get; init; }

            public float Range { get; init; }

            private int _passedTime = 0;

            protected override void Update(int timeDelta)
            {
                _passedTime += timeDelta;

                if (_passedTime > TimePerRangeUnit)
                {
                    Stop();
                    return;
                }

                SetGraphic(Target.X, Target.Y, 219);


                // TODO :: Perform actual ring graphical effect.

                // TODO : 78 :: Perform practical effect (apply damage to affected characters).
            }
        }
    }
}
