using AsLegacy.Characters.Skills;

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
            public float BaseDamage { get; init; }

            public Skill.Element Element { get; init; }

            public float Range { get; init; }

            protected override void Update()
            {
                // TODO :: Perform graphical effect (update effect tiles).

                // TODO : 78 :: Perform practical effect (apply damage to affected characters).
            }
        }
    }
}
