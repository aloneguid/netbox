using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generics
{
   public static class EnumerableExtensions
   {
      public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
      {
         if(source == null) throw new ArgumentNullException("source");

         while(source.Any())
         {
            yield return source.Take(chunksize);
            source = source.Skip(chunksize);
         }
      }

      public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
      {
         if(source == null) throw new ArgumentNullException("source");
         if(action == null) throw new ArgumentNullException("action");

         foreach(T element in source)
         {
            action(element);

            yield return element;
         }
      }

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
