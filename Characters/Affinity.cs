namespace AsLegacy.Characters
{
    public record Affinity : Talent
    {
        public float ActivationTime { get; init; }

        public float Cooldown { get; init; }

        public Skill.Element Element { get; init; }
    }
}
