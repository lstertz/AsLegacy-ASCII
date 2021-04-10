using System;
using static AsLegacy.World;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Passive, a Character Talent that defines a passive alteration 
    /// for a single Character affect based on the amount of investment in the passive.
    /// </summary>
    public record Passive : Talent
    {
        /// <summary>
        /// The altered Character affect.
        /// </summary>
        public Character.Affect Affect { get; private set; }

        private Func<int, float> _algorithm;

        /// <summary>
        /// Constructs a new <see cref="Passive"/>.
        /// </summary>
        /// <param name="affect">The altered Character affect.</param>
        /// <param name="title">The title of the <see cref="Passive"/>.</param>
        /// <param name="investmentAlgorithm">The algorithm that calculates the affect amount 
        /// to be applied, for a specified investment.</param>
        public Passive(Character.Affect affect, string title,
            Func<int, float> investmentAlgorithm) : base(title)
        {
            Affect = affect;
            _algorithm = investmentAlgorithm;
        }

        /// <inheritdoc/>
        public override string GetDescription(int investment) => Affect switch
        {
            Character.Affect.AdditionalMaxHealth =>
                $"+{_algorithm(investment):N1} Max Health",
            _ => throw new NotSupportedException($"The specified attribute " +
                $"{Affect} is not supported for a passive description.")
        };

        /// <inheritdoc/>
        public override string GetDescription(int investmentCurrent, int investmentNext) => 
            Affect switch
        {
            Character.Affect.AdditionalMaxHealth =>
                $"+{_algorithm(investmentNext) - _algorithm(investmentCurrent):N1}; " + 
                "for next level",
            _ => throw new NotSupportedException($"The specified attribute " +
                $"{Affect} is not supported for a passive description.")
        };

        /// <summary>
        /// Calculates the affect to be applied to the Character affect.
        /// </summary>
        /// <param name="investment">The investment to be used to calculate 
        /// the affect amount.</param>
        /// <returns>The calculated affect amount.</returns>
        public float GetAffect(int investment)
        {
            return _algorithm(investment);
        }
    }
}
