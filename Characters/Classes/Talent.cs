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

        /// <summary>
        /// Specifies whether the affect on the aspect is to scale.
        /// </summary>
        private bool IsScaleBased =>
            Influence.AffectOnAspect == Influence.Purpose.ScaleUp ||
            Influence.AffectOnAspect == Influence.Purpose.ScaleDown;


        /// <inheritdoc/>
        public override string GetDescription(World.Character character) =>
            string.Format(_descriptionFormat, $"{GetDescriptionAffect(character):0.##}");

        /// <summary>
        /// Provides a description of the <see cref="DescribableAffect"/> as it should be for the 
        /// specified investment.
        /// </summary>
        /// <param name="investment">The investment to be used to create the description.</param>
        /// <returns>A description, for displaying details of the <see cref="DescribableAffect"/> 
        /// for the specified investment.</returns>
        public string GetDescription(int investment) =>
            string.Format(_descriptionFormat, $"{GetDescriptionAffect(investment):0.##}");

        /// <summary>
        /// Provides the affect as it should be used for a description.
        /// </summary>
        /// <param name="character">The <see cref="World.Character"/> whose 
        /// affect is being calculated.</param>
        /// <returns>The affect.</returns>
        private float GetDescriptionAffect(World.Character character)
        {
            return GetDescriptionAffect(character.GetInvestment(this));
        }

        /// <summary>
        /// Provides the affect as it should be used for a description.
        /// </summary>
        /// <param name="character">The amount of investment used for calculation.</param>
        /// <returns>The affect.</returns>
        private float GetDescriptionAffect(int investment)
        {
            return IsScaleBased ? Algorithm(investment) * 100.0f :
                Algorithm(investment);
        }

        /// <summary>
        /// Provides the affect difference between the provided investments.
        /// </summary>
        /// <param name="investmentA">The original investment.</param>
        /// <param name="investmentB">The investment whose difference from the 
        /// original is to be described.</param>
        /// <returns>A string describing the difference in value.</returns>
        public string GetDifferenceDescription(int investmentA, int investmentB)
        {
            float a = GetDescriptionAffect(investmentA);
            float b = GetDescriptionAffect(investmentB);

            string sign = b - a > 0 ? "+" : ""; // Negative numbers add '-' on their own.
            string modifier = IsScaleBased ? "%" : "";
            float value = b - a;

            return $"{sign}{value:0.##}{modifier}";
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
