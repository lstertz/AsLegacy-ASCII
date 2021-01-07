using Microsoft.Xna.Framework;
using SadConsole;

namespace AsLegacy.GUI.Panes
{
    /// <summary>
    /// Defines a StatsPane, a Pane dedicated to showing Character-relevant stats.
    /// </summary>
    public class StatsPane : Pane
    {
        private readonly Ranking ranking;

        public StatsPane(int width, int height) : 
            base("Stats", "Character data to be shown here.", width, height)
        {
            ranking = new Ranking(width, 8)
            {
                Position = new Point(0, 15)
            };
            Children.Add(ranking);
        }
    }
}
