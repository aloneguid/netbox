using System.Collections.Generic;
using NetBox;
using NetBox.Application;

// ReSharper disable once CheckNamespace
namespace NetBox.Extensions
{
   /// <summary>
   /// Object extensions.
   /// </summary>
   public static class ObjectExtensions
   {

#if (NETFULL || NETSTANDARD20)
      /// <summary>
      /// Serialises any object to XML string if possible
      /// </summary>
      /// <param name="obj"></param>
      /// <returns>XML string</returns>
      public static string XmlSerialise(this object obj)
      {
         return new XmlSerialiser().Serialise(obj, true, true, G.Enc);
      }
#endif

      /// <summary>
      /// Creates an enumerable from one element
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="element"></param>
      /// <returns></returns>
      public static IEnumerable<T> AsEnumerable<T>(this T element)
      {
         return new List<T> { element };
      }
   }
}
