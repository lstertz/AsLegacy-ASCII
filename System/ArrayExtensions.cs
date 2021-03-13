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
    }
}
