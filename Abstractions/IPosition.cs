using System;

namespace AsLegacy.Abstractions
{
    /// <summary>
    /// Defines that a construct has a position (column and row) in space.
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

    /// <summary>
    /// Extensions for working with implementations of IPosition.
    /// </summary>
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
            return MathF.Sqrt(a.SquaredDistanceTo(b));
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
            int cDiff = a.Column - b.Column;
            int rDiff = a.Row - b.Row;
            return cDiff * cDiff + rDiff * rDiff;
        }
    }
}
