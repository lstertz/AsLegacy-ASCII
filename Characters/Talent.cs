namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Talent, the basic representation to define a quality of 
    /// a Character <see cref="Class"/>.
    /// </summary>
    public abstract record Talent
    {
        /// <summary>
        /// The unique ID of the <see cref="Talent"/>, to be used when any results of 
        /// the <see cref="Talent"/> are referenced elsewhere.
        /// </summary>
        public int ID => GetHashCode();

        /// <summary>
        /// The display title of the <see cref="Talent"/>.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Constructs a new <see cref="Talent"/>.
        /// </summary>
        /// <param name="title">The display title of the <see cref="Talent"/>.</param>
        public Talent(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Provides a description of the <see cref="Talent"/> as it should be with the 
        /// specified investment.
        /// </summary>
        /// <param name="investment">The theoretical amount of points invested in 
        /// the <see cref="Talent"/>, to be used in generating a matching description.</param>
        /// <returns>A description, for displaying details of the <see cref="Talent"/> when 
        /// it has the specified investment.</returns>
        public abstract string GetDescription(int investment);

        /// <summary>
        /// Provides a description of the <see cref="Talent"/> as would be appropriate when 
        /// comparing two different theoretical investments.
        /// </summary>
        /// <param name="investmentCurrent">The theoretical current amount of points 
        /// invested in the <see cref="Talent"/>.</param>
        /// <param name="investmentNext">The theoretical next amount of points 
        /// (greater or less than the current) invested in the <see cref="Talent"/>.</param>
        /// <returns>A description, for displaying details of the <see cref="Talent"/> when 
        /// comparing the provided investments.</returns>
        public abstract string GetDescription(int investmentCurrent, int investmentNext);
    }
}
