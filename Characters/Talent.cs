using Microsoft.Xna.Framework;
using System;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Talent, the basic representation to define a quality of 
    /// a Character <see cref="Class"/>.
    /// </summary>
    public abstract record Talent
    {
        /// <summary>
        /// The color of the affect produced by this Talent, being applied in any textual 
        /// or graphical display of the affect.
        /// </summary>
        public Color AffectColor { get; init; } = Color.White;

        /// <summary>
        /// The algorithm that converts an investment in this Talent to its quantified affect.
        /// </summary>
        public Func<float, float> Algorithm { private get; init; }
        
        /// <summary>
        /// The formatted string description to textually describe this Talent.
        /// </summary>
        public string DescriptionFormat { get; private set; }

        /// <summary>
        /// The formattable string to create the formatted string description 
        /// to textually describe this Talent.
        /// </summary>
        public FormattableString FormattableDescription
        { 
            private get
            {
                return _formattableDescription;
            }
            init
            {
                _formattableDescription = value;
                DescriptionFormat = value.Format;
            }
        }
        private FormattableString _formattableDescription = $"";


        /// <summary>
        /// The display name of the <see cref="Talent"/>.
        /// </summary>
        public string Name { get; init; } = "";

        /// <summary>
        /// Provides a description of the <see cref="Talent"/> as it should be with the 
        /// specified investment.
        /// </summary>
        /// <param name="investment">The theoretical amount of points invested in 
        /// the <see cref="Talent"/>, to be used in generating a matching description.</param>
        /// <returns>A description, for displaying details of the <see cref="Talent"/> when 
        /// it has the specified investment.</returns>
        public string GetDescription(int investment) =>
            string.Format(DescriptionFormat, $"{Algorithm(investment):N1}");

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

            char sign = b - a > 0 ? '+' : '-';
            return $"{sign}{(b - a):N1}";
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
