namespace System {
    using System.Diagnostics;
    using global::System.Collections.Generic;
    using global::System.Linq;

    /// <summary>
    /// <see cref="System.IEquatable{T}"/> extension methods
    /// </summary>
    static class EnumerableExtensions {

#if NET6_0_OR_GREATER
#else
        // .Chunk<> polyfill available from .net 6 and higher

        /// <summary>
        /// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
        /// </summary>
        /// <remarks>
        /// Every chunk except the last will be of size <paramref name="size"/>.
        /// The last chunk will contain the remaining elements and may be of a smaller size.
        /// </remarks>
        /// <param name="source">
        /// An <see cref="IEnumerable{T}"/> whose elements to chunk.
        /// </param>
        /// <param name="size">
        /// Maximum size of each chunk.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of source.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size <paramref name="size"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size"/> is below 1.
        /// </exception>
        public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> source, int size) {
            if(source is null)
                throw new ArgumentNullException(nameof(source));

            if(size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), "must be >= 1");

            return ChunkIterator(source, size);
        }

        private static IEnumerable<TSource[]> ChunkIterator<TSource>(IEnumerable<TSource> source, int size) {
            using IEnumerator<TSource> e = source.GetEnumerator();

            // Before allocating anything, make sure there's at least one element.
            if(e.MoveNext()) {
                // Now that we know we have at least one item, allocate an initial storage array. This is not
                // the array we'll yield.  It starts out small in order to avoid significantly overallocating
                // when the source has many fewer elements than the chunk size.
                int arraySize = Math.Min(size, 4);
                int i;
                do {
                    var array = new TSource[arraySize];

                    // Store the first item.
                    array[0] = e.Current;
                    i = 1;

                    if(size != array.Length) {
                        // This is the first chunk. As we fill the array, grow it as needed.
                        for(; i < size && e.MoveNext(); i++) {
                            if(i >= array.Length) {
                                arraySize = (int)Math.Min((uint)size, 2 * (uint)array.Length);
                                Array.Resize(ref array, arraySize);
                            }

                            array[i] = e.Current;
                        }
                    } else {
                        // For all but the first chunk, the array will already be correctly sized.
                        // We can just store into it until either it's full or MoveNext returns false.
                        TSource[] local = array; // avoid bounds checks by using cached local (`array` is lifted to iterator object as a field)
                        Debug.Assert(local.Length == size);
                        for(; (uint)i < (uint)local.Length && e.MoveNext(); i++) {
                            local[i] = e.Current;
                        }
                    }

                    if(i != array.Length) {
                        Array.Resize(ref array, i);
                    }

                    yield return array;
                }
                while(i >= size && e.MoveNext());
            }
        }
#endif

        /// <summary>
        /// Performs a specific action on each element of the sequence
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            if(source == null)
                throw new ArgumentNullException(nameof(source));
            if(action == null)
                throw new ArgumentNullException(nameof(action));

            foreach(T element in source) {
                action(element);

                yield return element;
            }
        }
    }
}