using System;
using static AsLegacy.World;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Passive, a Character Talent that defines a passive affect for a single Character 
    /// attribute based on the amount of investment in the passive.
    /// </summary>
    public record Passive : Talent
    {
        /// <summary>
        /// The affected Character attribute.
        /// </summary>
        public Character.Attribute AffectedAttribute { get; private set; }

        private Func<int, float> _algorithm;

        /// <summary>
        /// Constructs a new <see cref="Passive"/>.
        /// </summary>
        /// <param name="affectedAttribute">The affected Character attribute.</param>
        /// <param name="title">The title of the <see cref="Passive"/>.</param>
        /// <param name="investmentAlgorithm">The algorithm that calculates the affect 
        /// to be applied, for a specified investment, to the Character attribute.</param>
        public Passive(Character.Attribute affectedAttribute, string title,
            Func<int, float> investmentAlgorithm) : base(title)
        {
            AffectedAttribute = affectedAttribute;
            _algorithm = investmentAlgorithm;
        }

        /// <inheritdoc/>
        public override string GetDescription(int investment)
        {
            return AffectedAttribute switch
            {
                Character.Attribute.BaseHealth =>
                    $"Base Health +{_algorithm(investment).ToString("N1")}",
                _ => throw new NotSupportedException($"The specified attribute " +
                    $"{AffectedAttribute.ToString()} is not supported for a passive description.")
            };
        }

        /// <inheritdoc/>
        public override string GetDescription(int investmentCurrent, int investmentNext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the affect to be applied to the Character attribute.
        /// </summary>
        /// <param name="investment">The investment to be used to calculate the affect.</param>
        /// <returns>The calculated affect.</returns>
        public float GetAffect(int investment)
        {
            return _algorithm(investment);
        }
    }
}
