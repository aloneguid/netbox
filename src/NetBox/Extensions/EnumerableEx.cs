using System;
using System.Collections;
using System.Collections.Generic;

namespace NetBox.Extensions
{
   /// <summary>
   /// Enumerable extras
   /// </summary>
   public static class EnumerableEx
   {
      /// <summary>
      /// Iterates over two <see cref="IEnumerable"/> until one of them reaches the end of elements
      /// </summary>
      /// <typeparam name="TFirst">Types of elements in the first sequence</typeparam>
      /// <typeparam name="TSecond">Types of elements in the second sequence</typeparam>
      /// <param name="first">First sequence</param>
      /// <param name="second">Second sequence</param>
      /// <returns>Sequence of tuples from the first and second sequences</returns>
      public static IEnumerable<Tuple<TFirst, TSecond>> MultiIterate<TFirst, TSecond>(
         IEnumerable<TFirst> first, IEnumerable<TSecond> second)
      {
         if (first == null || second == null) yield break;

         IEnumerator<TFirst> firstEnumerator = first.GetEnumerator();
         IEnumerator<TSecond> secondEnumerator = second.GetEnumerator();

         while(firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
         {
            yield return Tuple.Create(firstEnumerator.Current, secondEnumerator.Current);
         }

         yield break;
      }
   }
}
