namespace System
{
    public static class ArrayExtensions
    {
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
