using Aloneguid.Support.Application;
// ReSharper disable once CheckNamespace


namespace System
{
   public static class DateTimeExtensions
   {
      public static DateTime RoundToDay(this DateTime time)
      {
         return new DateTime(time.Year, time.Month, time.Day);
      }

      public static DateTime EndOfDay(this DateTime time)
      {
         return new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
      }

      public static DateTime RoundToMinute(this DateTime time, int round, bool roundLeft)
      {
         int minute = time.Minute;
         int leftover = minute%round;
         if(leftover == 0) return time;
         int addHours = 0;
         minute -= leftover;

         if(!roundLeft) minute += round;
         if(minute > 59)
         {
            minute = minute%60;
            addHours = 1;
         }

         return new DateTime(time.Year, time.Month, time.Day, time.Hour + addHours, minute, 0);
      }

      public static bool IsToday(this DateTime time)
      {
         return (DateTime.UtcNow - time).TotalDays < 1.0;
      }

      public static bool IsTomorrow(this DateTime time)
      {
         TimeSpan diff = DateTime.UtcNow - time;

         return diff.TotalDays >= 1 && diff.TotalDays < 2;
      }

      public static bool IsThisWeek(this DateTime time)
      {
         DateTime now = DateTime.UtcNow;
         int diff = now.DayOfWeek - DayOfWeek.Monday;
         DateTime weekStart = now.AddDays(-1*diff).RoundToDay();
         DateTime weekEnd = now.AddDays(diff).RoundToDay();

         return time > weekStart && time < weekEnd;
      }

      public static bool IsThisMonth(this DateTime time)
      {
         DateTime now = DateTime.UtcNow;
         DateTime monthStart = new DateTime(now.Year, now.Month, 0);
         DateTime monthEnd = monthStart.AddMonths(1);

         return time > monthStart && time < monthEnd;
      }

      public static string PrettyDayName(this DateTime time)
      {
         if(time.IsToday()) return Strings.Day_Today;
         if(time.IsTomorrow()) return Strings.Day_Tomorrow;
         if(time.IsThisWeek()) return time.ToString("dddd");
         if(time.IsThisMonth()) return time.ToString("");

         return time.ToString();
      }

      public static string ToHourMinuteString(this DateTime time)
      {
         return time.ToString("HH:mm");
      }
   }
}
