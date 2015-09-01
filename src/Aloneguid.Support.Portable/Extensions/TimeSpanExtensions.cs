using Aloneguid.Support.Application;

// ReSharper disable once CheckNamespace
namespace System
{
   public static class TimeSpanExtensions
   {
      public static string ToLongPrettyString(this TimeSpan duration)
      {
         return PrettyTime.ToPrettyTime(duration, false);
      }

      public static string ToShortPrettyString(this TimeSpan duration)
      {
         return PrettyTime.ToPrettyTime(duration, true);
      }
   }
}
