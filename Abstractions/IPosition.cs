using System;

namespace AsLegacy.Abstractions
{
    /// <summary>
    /// Defines that a construct has a column and row defining a position in space.
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// The Column (x-axis) location of the position.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// The Row (y-axis) location of the position.
        /// </summary>
        public int Row { get; }
    }

    public static class PositionExtensions
    {
        /// <summary>
        /// Provides the exact distance from this IPosition to the provided IPosition.
        /// </summary>
        /// <param name="a">This IPosition.</param>
        /// <param name="b">The IPosition being measured to.</param>
        /// <returns>The distance from this IPosition to the provided IPosition.</returns>
        public static float DistanceTo(this IPosition a, IPosition b)
        {
            return MathF.Sqrt((a.Column - b.Column) ^ 2 + (a.Row - b.Row) ^ 2);
        }

        /// <summary>
        /// Provides the squared distance from this IPosition to the provided IPosition.
        /// This function is intended to reduce computation to square root the distance.
        /// </summary>
        /// <param name="a">This IPosition.</param>
        /// <param name="b">The IPosition being measured to.</param>
        /// <returns>The squared distance from this IPosition to the provided IPosition.</returns>
        public static int SquaredDistanceTo(this IPosition a, IPosition b)
        {
            return (a.Column - b.Column) ^ 2 + (a.Row - b.Row) ^ 2;
        }
    }
}
