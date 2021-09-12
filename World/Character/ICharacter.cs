using AsLegacy.Characters;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines the external interface of a World Character.
        /// </summary>
        public interface ICharacter
        {
            /// <summary>
            /// The number of skill points that this character has available 
            /// for investing in skills.
            /// </summary>
            int AvailableSkillPoints { get; }


            /// <summary>
            /// Provides the complete affect, based on the Character's investments, 
            /// for the specified attribute.
            /// </summary>
            /// <param name="affectAttribute">The attribute whose affect is being provided.</param>
            /// <returns>The totaled (additive influencers) and scaled (scaling influencers) 
            /// affect for the specified attribute, or the attribute's default calculated value 
            /// if this Character has not invested in any of the attribute's 
            /// related <see cref="Talent"/>s.</returns>
            float GetAffect(Attribute affectAttribute);

            /// <summary>
            /// Provides the amount of investment that this Character has 
            /// in the provided <see cref="Talent"/>.
            /// </summary>
            /// <param name="talent">The <see cref="Talent"/> whose investment 
            /// is to be retrieved.</param>
            /// <returns>The amount of investment; 0 if there has been no investment.</returns>
            int GetInvestment(Talent talent);

            /// <summary>
            /// Invests the specified amount in the provided <see cref="Talent"/>.
            /// </summary>
            /// <param name="talent">The <see cref="Talent"/> to be invested in.</param>
            /// <param name="amount">The amount to attempt to invest.</param>
            /// <returns>The actual amount invested.</returns>
            int InvestInTalent(Talent talent, int amount);
        }
    }
}
