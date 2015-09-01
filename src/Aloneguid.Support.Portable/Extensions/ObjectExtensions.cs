using Aloneguid.Support;
using Aloneguid.Support.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   public static class ObjectExtensions
   {
      private static readonly JsonSerialiser Json = new JsonSerialiser();

      /// <summary>
      /// Serialises any object to XML string if possible
      /// </summary>
      /// <param name="obj"></param>
      /// <returns>XML string</returns>
      public static string XmlSerialise(this object obj)
      {
         return new XmlSerialiser().Serialise(obj, true, true, G.Enc);
      }

      /// <summary>
      /// Converts any object to a JSON string if possible
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public static string ToJsonString(this object obj)
      {
         return Json.Serialise(obj);
      }
   }
}
