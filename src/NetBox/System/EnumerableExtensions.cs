/*
 * b.             8 8 8888888888 8888888 8888888888 8 888888888o       ,o888888o.  `8.`8888.      ,8' 
 * 888o.          8 8 8888             8 8888       8 8888    `88.  . 8888     `88. `8.`8888.    ,8'  
 * Y88888o.       8 8 8888             8 8888       8 8888     `88 ,8 8888       `8b `8.`8888.  ,8'   
 * .`Y888888o.    8 8 8888             8 8888       8 8888     ,88 88 8888        `8b `8.`8888.,8'    
 * 8o. `Y888888o. 8 8 888888888888     8 8888       8 8888.   ,88' 88 8888         88  `8.`88888'     
 * 8`Y8o. `Y88888o8 8 8888             8 8888       8 8888888888   88 8888         88  .88.`8888.     
 * 8   `Y8o. `Y8888 8 8888             8 8888       8 8888    `88. 88 8888        ,8P .8'`8.`8888.    
 * 8      `Y8o. `Y8 8 8888             8 8888       8 8888      88 `8 8888       ,8P .8'  `8.`8888.   
 * 8         `Y8o.` 8 8888             8 8888       8 8888    ,88'  ` 8888     ,88' .8'    `8.`8888.  
 * 8            `Yo 8 888888888888     8 8888       8 888888888P       `8888888P'  .8'      `8.`8888. 
 * 
 * by aloneguid. GitHub: https://github.com/aloneguid/netbox
 */


using System.Collections.Generic;
using System.Linq;

namespace System
{
   /// <summary>
   /// <see cref="System.IEquatable{T}"/> extension methods
   /// </summary>
   static class EnumerableExtensions
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
         if (source == null) throw new ArgumentNullException(nameof(source));

         while (source.Any())
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
         if (source == null) throw new ArgumentNullException(nameof(source));
         if (action == null) throw new ArgumentNullException(nameof(action));

         foreach (T element in source)
         {
            action(element);

            yield return element;
         }
      }
   }
}
