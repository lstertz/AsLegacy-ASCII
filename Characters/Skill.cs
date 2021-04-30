using System.Collections.ObjectModel;

namespace AsLegacy.Characters
{
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
        /// The elemental type of the skill.
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

        public ReadOnlyCollection<Concept> AugmentingConcepts { get; init; }

        public Affinity Affinity { get; init; }

        public Concept Concept { get; init; }
    }
}
