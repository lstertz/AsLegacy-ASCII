using System.Collections.ObjectModel;
using System.Text;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines an Affinity, which is a practical application of a <see cref="Concept"/> and 
    /// an abstraction of a <see cref="Skill"/>.
    /// </summary>
    public record Affinity : DescribableAffect
    {
        /// <summary>
        /// The desciption name for the activation attribute.
        /// </summary>
        private const string ActivationName = "Activation";

        /// <summary>
        /// The desciption name for the cooldown attribute.
        /// </summary>
        private const string CooldownName = "Cooldown";

        /// <summary>
        /// The time required for any effects of this <see cref="Affinity"/> to be realized.
        /// </summary>
        public float Activation
        {
            get => _activation.BaseValue;
            init => _activation = new Attribute()
            {
                Aspects = new(new Aspect[]
                {
                        Aspect.Activation
                }),
                BaseValue = value,
                Name = ActivationName
            };
        }
        private Attribute _activation;

        public ReadOnlyCollection<Attribute> CustomAttributes { get; init; } = 
            new(System.Array.Empty<Attribute>());

        /// <summary>
        /// The cooldown that follows this <see cref="Affinity"/>'s effects.
        /// </summary>
        public float Cooldown
        {
            get => _cooldown.BaseValue;
            init => _cooldown = new Attribute()
            {
                Aspects = new(new Aspect[]
                    {
                        Aspect.Cooldown
                    }),
                BaseValue = value,
                Name = CooldownName
            };
        }
        private Attribute _cooldown;

        /// <summary>
        /// The elemental classification of this <see cref="Affinity"/> when 
        /// considered as a <see cref="Skill"/>.
        /// </summary>
        public Skill.Element Element { get; init; }

        /// <inheritdoc/>
        public override string GetDescription(World.Character character)
        {
            StringBuilder builder = new(_descriptionFormat + "\n ");
            for (int c = 0, count = CustomAttributes.Count; c < count; c++)
                builder.Append($"\n{CustomAttributes[c].Name}: " +
                    $"{character.GetAffect(CustomAttributes[c])}");

            builder.Append($"\n{_activation.Name}: {character.GetAffect(_activation)}");
            builder.Append($"\n{_cooldown.Name}: {character.GetAffect(_cooldown)}");
            return builder.ToString();
        }
    }
}
