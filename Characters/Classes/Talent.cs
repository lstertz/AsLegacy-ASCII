using System;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Talent, the basic representation to define an attribute affect of 
    /// a Character <see cref="Class"/>.
    /// </summary>
    public abstract record Talent : DescribableAffect
    {
        /// <summary>
        /// The algorithm that converts an investment in this Talent to its quantified affect.
        /// </summary>
        public Func<float, float> Algorithm { private get; init; }

        /// <summary>
        /// The influence of this <see cref="Talent"/>; how it is applied respective to 
        /// other Character concepts.
        /// </summary>
        public Influence Influence {get; init;}

        /// <inheritdoc/>
        public override string GetDescription(World.Character character) =>
            string.Format(_descriptionFormat, $"{Algorithm(character.GetInvestment(this)):N1}");

        /// <summary>
        /// Provides the affect difference between the provided investments.
        /// </summary>
        /// <param name="investmentA">The original investment.</param>
        /// <param name="investmentB">The investment whose difference from the 
        /// original is to be described.</param>
        /// <returns>A string describing the difference in value.</returns>
        public string GetDifferenceDescription(int investmentA, int investmentB)
        {
            float a = Algorithm(investmentA);
            float b = Algorithm(investmentB);

            string sign = b - a > 0 ? "+" : ""; // Negative numbers add '-' on their own.
            string modifier = Influence.AffectOnAspect == Influence.Purpose.Scale ? "%" : "";
            return $"{sign}{(b - a):N1}{modifier}";
        }

        /// <summary>
        /// Calculates the affect to be applied to the Character affect.
        /// </summary>
        /// <param name="investment">The investment to be used to calculate 
        /// the affect amount.</param>
        /// <returns>The calculated affect amount.</returns>
        public float GetAffect(int investment)
        {
            return Algorithm(investment);
        }
    }
}
