using AsLegacy.Characters;

namespace AsLegacy // TODO :: Define more fitting namespace.
{
    /// <summary>
    /// Defines an Effect, which provides specifications for a skill's graphical and 
    /// practical effects.
    public record Effect
    {
        public Skill.Type Type { get; init; }

        public Effect SubsequentEffect { get; init; }

        // TODO :: Other effect details.
    }
}
