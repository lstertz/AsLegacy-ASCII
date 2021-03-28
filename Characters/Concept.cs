using System;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Concept, a Character Talent that is a singular concept defining the 
    /// underlying purpose and properties of a set of modular skills.
    /// </summary>
    public record Concept : Talent
    {
        /// <summary>
        /// Constructs a new <see cref="Concept"/>.
        /// </summary>
        /// <param name="title">The title of the <see cref="Concept"/>.</param>
        public Concept(string title) :
            base(title)
        {
        }

        /// <inheritdoc/>
        public override string GetDescription(int investment)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override string GetDescription(int investmentCurrent, int investmentNext)
        {
            throw new NotImplementedException();
        }
    }
}
