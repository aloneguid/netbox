using Aloneguid.Support.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   public static class IntExtensions
   {
      public static string ToFileSizeString(this int number)
      {
         return ByteFormat.ToString(number, ByteFormat.Standard.Iec, null);
      }

      public static string ToFileSizeUiString(this int number)
      {
         return ByteFormat.ToString(number, ByteFormat.Standard.Si, null);
      }
   }
}
