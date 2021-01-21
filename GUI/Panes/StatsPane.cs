using Microsoft.Xna.Framework;

namespace AsLegacy.GUI.Panes
{
    /// <summary>
    /// Defines a StatsPane, a Pane dedicated to showing Character-relevant stats.
    /// </summary>
    public class StatsPane : Pane
    {
        private readonly Ranking ranking;

        /// <summary>
        /// Constructs a new StatsPane.
        /// </summary>
        /// <param name="width">The width of the Pane.</param>
        /// <param name="height">The height of the Pane.</param>
        public StatsPane(int width, int height) : 
            base("Stats", "Character data to be shown here.", width, height)
        {
            ranking = new Ranking(width, 8, true)
            {
                Position = new Point(0, 15)
            };
            Children.Add(ranking);
        }
    }
}
