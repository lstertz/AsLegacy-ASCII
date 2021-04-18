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
        public Color AffectTextColor { get; init; } = Color.White;

        public Func<int, float> Algorithm { private get; init; }
        
        public string DescriptionFormat { get; private set; }

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
        /// The display title of the <see cref="Talent"/>.
        /// </summary>
        public string Title { get; init; } = "";


        /// <summary>
        /// Constructs a new <see cref="Talent"/>.
        /// </summary>
        public Talent()
        {
        }

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
