namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Affinity, which is a practical application of a <see cref="Concept"/> and 
    /// an abstraction of a <see cref="Skill"/>.
    /// </summary>
    public record Affinity : DescribableAffect
    {
        /// <summary>
        /// The time required for any effects of this <see cref="Affinity"/> to be realized.
        /// </summary>
        public float Activation { get; init; }

        /// <summary>
        /// The cooldown that follows this <see cref="Affinity"/>'s effects.
        /// </summary>
        public float Cooldown { get; init; }

        /// <summary>
        /// The elemental classification of this <see cref="Affinity"/> when 
        /// considered as a <see cref="Skill"/>.
        /// </summary>
        public Skill.Element Element { get; init; }

        // TODO :: Define attributes with base influencer type, modifier influencer types, and base modifier value.
        //          Attribute results are retrieved from the Character when outputting description.

        /// <inheritdoc/>
        public override string GetDescription(World.Character character)
        {
            return _descriptionFormat + 
                $"\n \nActivation: {character.GetAffect(Influence.Attribute.Activation, Activation)}" +
                $"\nCooldown: {character.GetAffect(Influence.Attribute.Cooldown, Cooldown)}";
        }
    }
}
