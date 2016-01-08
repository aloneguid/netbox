using Aloneguid.Support.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   /// <summary>
   /// <see cref="TimeSpan"/> extensions
   /// </summary>
   public static class TimeSpanExtensions
   {
      /// <summary>
      /// Formats time span in a readable format, for example "5 hours 2 minutes"
      /// </summary>
      public static string ToLongPrettyString(this TimeSpan duration)
      {
         return PrettyTime.ToPrettyTime(duration, false);
      }

      /// <summary>
      /// Formats time span in a readable format, for example "5h 2m"
      /// </summary>
      public static string ToShortPrettyString(this TimeSpan duration)
      {
         return PrettyTime.ToPrettyTime(duration, true);
      }
   }
}
