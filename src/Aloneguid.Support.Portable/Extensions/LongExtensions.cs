using Aloneguid.Support.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   public static class LongExtensions
   {
      public static string ToFileSizeString(this long number)
      {
         return ByteFormat.ToString(number, ByteFormat.Standard.Iec, null);
      }

      public static string ToFileSizeUiString(this long number)
      {
         return ByteFormat.ToString(number, ByteFormat.Standard.Si, null);
      }
   }
}
