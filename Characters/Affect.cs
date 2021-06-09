namespace AsLegacy.Characters.Skills
{
    /// <summary>
    /// Defines an Affect, which provides specifications for a skill's graphical and 
    /// practical effects.
    public struct Affect
    {
        public float BaseDamage { get; init; }

        public Skill.Element Element {get; init;}

        public float Range { get; init; }

        public Skill.Effect Type { get; init; }


        // TODO :: Other effect details.
    }
}
