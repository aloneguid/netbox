using System.Collections.Generic;
using NetBox;
using NetBox.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   /// <summary>
   /// Object extensions.
   /// </summary>
   public static class ObjectExtensions
   {

      private static readonly JsonSerialiser Json = new JsonSerialiser();

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
      /// Converts any object to a JSON string if possible
      /// </summary>
      /// <param name="obj">Object to serialize</param>
      /// <param name="compress">When true JSON is compressed, i.e. put in one line to be as small as possible</param>
      /// <param name="enumsAsStrings">When true enums are serialized as strings</param>
      public static string ToJsonString(this object obj, bool compress = false, bool enumsAsStrings = false)
      {
         return Json.Serialise(obj, compress, enumsAsStrings);
      }

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
