using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generics
{
   public static class EnumerableExtensions
   {
      /// <summary>
      /// Split sequence in batches of specified size
      /// </summary>
      /// <typeparam name="T">Element type</typeparam>
      /// <param name="source">Enumeration source</param>
      /// <param name="chunkSize">Size of the batch chunk</param>
      /// <returns></returns>
      public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
      {
         if(source == null) throw new ArgumentNullException(nameof(source));

         while(source.Any())
         {
            yield return source.Take(chunkSize);
            source = source.Skip(chunkSize);
         }
      }

      /// <summary>
      /// Performs a specific action on each element of the sequence
      /// </summary>
      public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
      {
         if(source == null) throw new ArgumentNullException(nameof(source));
         if(action == null) throw new ArgumentNullException(nameof(action));

         foreach(T element in source)
         {
            action(element);

            yield return element;
         }
      }

      /// <summary>
      /// A hashset extension which adds back the .AddAll
      /// </summary>
      public static void AddAll<T>(this HashSet<T> hashSet, IEnumerable<T> elements)
      {
         if(elements == null) return;

         foreach(T el in elements)
         {
            hashSet.Add(el);
         }
      }
   }
}
