using NetBox.Application;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
   /// <summary>
   /// <see cref="System.IEquatable{T}"/> extension methods
   /// </summary>
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
      /// ICollection extension brining the useful AddRange from List
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="collection"></param>
      /// <param name="source"></param>
      public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> source)
      {
         if(collection == null) return;
         if(source == null) return;

         foreach(T element in source)
         {
            collection.Add(element);
         }
      }

      /// <summary>
      /// Tries to guess the type of data contained in the strings
      /// </summary>
      /// <param name="data">The data.</param>
      /// <returns></returns>
      public static Type InferType(this string[] data)
      {
         return TypeInferring.InferBestType(data);
      }
   }
}
