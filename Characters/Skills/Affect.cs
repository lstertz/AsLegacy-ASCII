using Microsoft.Xna.Framework;

namespace AsLegacy.Characters.Skills
{
    /// <summary>
    /// Defines an Affect, which provides specifications for a skill's graphical and 
    /// practical effects.
    public struct Affect
    {
        /// <summary>
        /// The available properties of an affect that can be set to define 
        /// its result.
        /// </summary>
        public enum Setting
        {
            /// <summary>
            /// <see cref="BaseDamage"/>
            /// </summary>
            BaseDamage,
            /// <summary>
            /// <see cref="Range"/>
            /// </summary>
            Range
        }

        /// <summary>
        /// The color to be used for anything defined by the affect.
        /// </summary>
        public Color AffectColor { get; init; }

        /// <summary>
        /// The base damage to be dealt to those affected by the affect.
        /// </summary>
        public float BaseDamage { get; init; }

        /// <summary>
        /// The elemental type of the affect.
        /// </summary>
        public Skill.Element Element {get; init; }

        /// <summary>
        /// The origin point, from which the affect was produced.
        /// </summary>
        public Point Origin { get; init; }

        /// <summary>
        /// The range of the affect.
        /// </summary>
        public float Range { get; init; }

        /// <summary>
        /// The target point, on which the affect is focused.
        /// </summary>
        public Point Target { get; init; }

        /// <summary>
        /// The skill type of the affect.
        /// </summary>
        public Skill.Type Type { get; init; }


        // TODO :: Other affect details.
    }
}
