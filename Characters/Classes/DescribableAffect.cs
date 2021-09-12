using Microsoft.Xna.Framework;
using System;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a describable affect, which provides a textual representation of 
    /// an affect that can be produced by or can influence a <see cref="World.Character"/>.
    /// </summary>
    public abstract record DescribableAffect
    {
        /// <summary>
        /// The color of the affect produced, being applied in any textual 
        /// or graphical display of the affect.
        /// </summary>
        public Color AffectColor { get; init; } = Color.White;

        /// <summary>
        /// The formattable string to create the formatted string description 
        /// to textually describe the affect.
        /// </summary>
        public FormattableString FormattableDescription
        {
            init
            {
                _descriptionFormat = value.Format;
            }
        }
        protected string _descriptionFormat;

        /// <summary>
        /// The display name of the <see cref="DescribableAffect"/>.
        /// </summary>
        public string Name { get; init; } = "";

        /// <summary>
        /// Provides a description of the <see cref="DescribableAffect"/> as it should be for the 
        /// state of the provided <see cref="World.ICharacter"/>.
        /// </summary>
        /// <param name="character">The Character whose state is referenced 
        /// to create the description.</param>
        /// <returns>A description, for displaying details of the <see cref="DescribableAffect"/> 
        /// for the state of the provided <see cref="World.ICharacter"/>.</returns>
        public abstract string GetDescription(World.ICharacter character);
    }
}
