using System;
using System.Text;

namespace Aloneguid.Support.Application
{
   class PrettyTime
   {
      public static string ToPrettyTime(TimeSpan duration, bool shortMods)
      {
         var b = new StringBuilder();

         //hours
         if(duration.Hours > 0)
         {
            b.Append(duration.Hours);
            b.Append(" ");
            if(duration.Hours == 1)
            {
               b.Append(shortMods ? Strings.HourShort : Strings.HourLong);
            }
            else
            {
               b.Append(shortMods ? Strings.HoursShort : Strings.HoursLong);
            }
         }

         //minutes
         if(duration.Minutes > 0)
         {
            if(b.Length > 0) b.Append(", ");
            b.Append(duration.Minutes);
            b.Append(" ");
            if(duration.Minutes == 1)
            {
               b.Append(shortMods ? Strings.MinuteShort : Strings.MinutesShort);
            }
            else
            {
               b.Append(shortMods ? Strings.MinutesShort : Strings.MinutesLong);
            }
         }

         //seconds
         if(duration.Seconds > 0)
         {
            if(b.Length > 0) b.Append(", ");
            b.Append(duration.Seconds);
            b.Append(" ");
            if(duration.Seconds == 1)
            {
               b.Append(shortMods ? Strings.SecondShort : Strings.SecondLong);
            }
            else
            {
               b.Append(shortMods ? Strings.SecondsShort : Strings.SecondShort);
            }
         }

         return b.ToString();
      }
   }
}
