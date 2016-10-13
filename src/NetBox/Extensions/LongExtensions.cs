using NetBox.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   /// <summary>
   /// <see cref="long"/> extension methods
   /// </summary>
   public static class LongExtensions
   {
      /// <summary>
      /// Converts number to readable size string in IEC format, i.e. 1024 converts to "1.02 KiB"
      /// </summary>
      public static string ToFileSizeString(this long number)
      {
         return ByteFormat.ToString(number, ByteFormat.Standard.Iec, null);
      }

      /// <summary>
      /// Converts number to readable size string in SI format, i.e. 1024 converts to "1.02 KB"
      /// </summary>
      public static string ToFileSizeUiString(this long number)
      {
         return ByteFormat.ToString(number, ByteFormat.Standard.Si, null);
      }
   }
}
