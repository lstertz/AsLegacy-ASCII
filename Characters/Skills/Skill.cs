using System.Collections.ObjectModel;

namespace AsLegacy.Characters.Skills
{
    /// <summary>
    /// Defines a Skill, which produces visual and practical effects, 
    /// based upon its <see cref="Affinity"/> and <see cref="Concept"/>s, 
    /// when performed by a Character.
    /// </summary>
    public record Skill
    {
        /// <summary>
        /// The category that defines how a skill is considered relative to other skills.
        /// </summary>
        public enum Category
        {
            Primary,
            Secondary,
            Tertiary
        }

        /// <summary>
        /// The elemental classification of the skill.
        /// </summary>
        public enum Element
        {
            Lightning
        }

        /// <summary>
        /// The type of a skill, which defines the effects of the skill and its visuals.
        /// </summary>
        public enum Type
        {
            AdjacentAttack,
            AreaOfEffect,
            Debuff,
            EnvironmentEffect,
            Movement,
            Projectile,
            Shield,
            Summon,
            TargetEffect
        }

        /// <summary>
        /// The time, in seconds, required for this Skill to be activated.
        /// </summary>
        public float Activation
        {
            get => Affinity.Activation; // TODO :: Incorporate augmenting concepts.
        }

        /// <summary>
        /// The underlying affinity that defines some of the core functionality 
        /// of this <see cref="Skill"/>.
        /// </summary>
        public Affinity Affinity { get; init; }

        /// <summary>
        /// The <see cref="Concept"/>s that augment this <see cref="Skill"/> with additional 
        /// functionality and affects.
        /// </summary>
        public ReadOnlyCollection<Concept> AugmentingConcepts { get; init; }

        /// <summary>
        /// The underlying <see cref="Concept"/> that defines some of the core functionality 
        /// of this <see cref="Skill"/>.
        /// </summary>
        public Concept Concept { get; init; }


        /// <summary>
        /// Provides the <see cref="Affect"/>s that define the effects of this <see cref="Skill"/>.
        /// </summary>
        /// <remarks>Affects should be realized in the order provided, one after the other 
        /// as their effects complete.</remarks>
        public Affect[] GetAffects()
        {
            // TODO :: Evaluate this Skill's Affinity, Concept, and Augmenting Concepts to 
            //          determine the affect(s) it should produce.

            return new Affect[0];
        }
    }
}
