using AsLegacy.Characters.Skills;
using System;
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
        /// The time, in seconds, required for any effects 
        /// of this <see cref="Affinity"/> to be realized.
        /// </summary>
        public float BaseActivation
        {
            init => _activation = _activation with
            {
                BaseValue = value
            };
        }
        private Attribute _activation = new()
        {
            Aspects = new(new Aspect[]
                {
                    Aspect.Activation
                }),
            Name = ActivationName
        };

        /// <summary>
        /// Any additional <see cref="Aspect"/>s to be applied to the <see cref="BaseActivation"/>.
        /// </summary>
        public Aspect[] ActivationAdditionalAspects
        {
            init => _activation = _activation with
            {
                Aspects = new(value.With(Aspect.Cooldown))
            };
        }

        /// <summary>
        /// The cooldown that follows this <see cref="Affinity"/>'s effects.
        /// </summary>
        public float BaseCooldown
        {
            init => _cooldown = _cooldown with
            {
                BaseValue = value
            };
        }
        private Attribute _cooldown = new()
        {
            Aspects = new(new Aspect[]
                {
                    Aspect.Cooldown
                }),
            Name = CooldownName
        };

        /// <summary>
        /// Any additional <see cref="Aspect"/>s to be applied to the <see cref="BaseCooldown"/>.
        /// </summary>
        public Aspect[] CooldownAdditionalAspects
        {
            init => _cooldown = _cooldown with
            {
                Aspects = new(value.With(Aspect.Cooldown))
            };
        }

        /// <summary>
        /// Any additional <see cref="Attribute"/>s that define this <see cref="Affinity"/>.
        /// </summary>
        public ReadOnlyCollection<Attribute> CustomAttributes { get; init; } =
            new(Array.Empty<Attribute>());

        /// <summary>
        /// The elemental classification of this <see cref="Affinity"/> when 
        /// considered as a <see cref="Skill"/>.
        /// </summary>
        public Skill.Element Element { get; init; }


        /// <summary>
        /// Provides the activation, in seconds, for performing the <see cref="Skill"/> 
        /// defined by this affinity.
        /// </summary>
        /// <param name="character">The <see cref="World.Character"/> performing 
        /// the <see cref="Skill"/>.</param>
        /// <returns>The activation time, in seconds.</returns>
        public float GetActivation(World.Character character)
        {
            return character.GetAffect(_activation);
        }

        /// <summary>
        /// Provides the cooldown, in seconds, for performing the <see cref="Skill"/> 
        /// defined by this affinity.
        /// </summary>
        /// <param name="character">The <see cref="World.Character"/> performing 
        /// the <see cref="Skill"/>.</param>
        /// <returns>The cooldown time, in seconds.</returns>
        public float GetCooldown(World.Character character)
        {
            return character.GetAffect(_cooldown);
        }

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
