namespace System
{
    /// <summary>
    /// Provides extensions for Arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Casts the elements of this array to <typeparamref name="TCastElement"/> 
        /// using the provided Func.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of this array.</typeparam>
        /// <typeparam name="TCastElement">The type the elements will be cast to.</typeparam>
        /// <param name="array">This array, whose elements will be cast in a new array.</param>
        /// <param name="cast">The func to perform the cast.</param>
        /// <returns>A new array with <typeparamref name="TCastElement"/> elements.</returns>
        public static TCastElement[] CastTo<TElement, TCastElement>(this TElement[] array,
            Func<TElement, TCastElement> cast)
        {
            TCastElement[] newArray = new TCastElement[array.Length];
            for (int c = 0, count = array.Length; c < count; c++)
                newArray[c] = cast(array[c]);

            return newArray;
        }

        /// <summary>
        /// Creates a new array that is the copy of the provided array, with the addition of 
        /// the provided element at the end of the new array.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of this array.</typeparam>
        /// <param name="array">The array to be copied with the new element.</param>
        /// <param name="element">The element to be added to the new array.</param>
        /// <returns>The new array.</returns>
        public static TElement[] With<TElement>(this TElement[] array, TElement element)
        {
            TElement[] newArray = new TElement[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[array.Length] = element;

            return newArray;
        }
    }
}
