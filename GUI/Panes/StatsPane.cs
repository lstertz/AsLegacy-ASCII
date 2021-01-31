using AsLegacy.Global;
using Microsoft.Xna.Framework;
using System;

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
            base("Stats", "", width, height)
        {
            ranking = new Ranking(8, true)
            {
                Position = new Point(0, 15)
            };
            Children.Add(ranking);
        }

        /// <inheritdoc/>
        public override void Draw(TimeSpan timeElapsed)
        {
            Fill(0, 2, Width, Colors.White, Colors.Black, 0);

            string health = MathF.Ceiling(AsLegacy.Focus.CurrentHealth).ToString();
            string maxHealth = MathF.Ceiling(AsLegacy.Focus.MaxHealth).ToString();
            Print(0, 2, "Health: " + health + "/" + maxHealth, Color.White);

            base.Draw(timeElapsed);
        }
    }
}
