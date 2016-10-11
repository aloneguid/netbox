using Aloneguid.Support;
using Aloneguid.Support.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   /// <summary>
   /// Object extensions.
   /// </summary>
   public static class ObjectExtensions
   {

#if !NETSTANDARD
      private static readonly JsonSerialiser Json = new JsonSerialiser();
#endif

#if NETFULL
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

#if !NETSTANDARD
      /// <summary>
      /// Converts any object to a JSON string if possible
      /// </summary>
      public static string ToJsonString(this object obj)
      {
         return Json.Serialise(obj, false);
      }

      /// <summary>
      /// Converts any object to a compressed JSON string if possible. Compressed strings use as less spacing and separators as possible.
      /// </summary>
      public static string ToCompressedJsonString(this object obj)
      {
         return Json.Serialise(obj, true);
      }
#endif

   }
}
