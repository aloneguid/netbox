using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
   /// <summary>
   /// Extensions methods for <see cref="IList{T}"/> and <see cref="IList"/>
   /// </summary>
   public static class ListExtensions
   {
      /// <summary>
      /// Adds range of elements into the non-generic list
      /// </summary>
      /// <param name="destination">The destination.</param>
      /// <param name="source">The source.</param>
      public static void AddRange(this IList destination, IEnumerable source)
      {
         if (source == null) return;

         foreach(object element in source)
         {
            destination.Add(element);
         }
      }

      /// <summary>
      /// Adds range of elements into the non-generic list
      /// </summary>
      /// <param name="destination">The destination.</param>
      /// <param name="source">The source.</param>
      public static void AddRange(this IList destination, ICollection source)
      {
         if (source == null) return;

#if (NETFULL || NETSTANDARD20)
         if (destination is ArrayList arrayList)
         {
            arrayList.AddRange(source);
            return;
         }
#endif

         AddRange(destination, (IEnumerable)source);
      }
   }
}
