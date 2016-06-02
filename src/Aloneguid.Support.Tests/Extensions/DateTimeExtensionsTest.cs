using System;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class DateTimeExtensionsTest
   {
      [Test]
      public void RoundToDay_TestDateWithTime_TimeTrimmed()
      {
         Assert.AreEqual(new DateTime(2015, 09, 10), new DateTime(2015, 09, 10, 14, 17, 35).RoundToDay());  
      }

      [Test]
      public void PrettyDayName_Today_Correct()
      {
         Assert.AreEqual("today", DateTime.UtcNow.PrettyDayName());         
      }

      [Test]
      public void IsThisWeek_Today_True()
      {
         Assert.IsTrue(DateTime.UtcNow.IsThisWeek());
      }

      [Test]
      public void IsThisWeek_NextWeek_False()
      {
         Assert.IsFalse(DateTime.UtcNow.AddDays(10).IsThisWeek());
      }

      [Test]
      public void ToHourMinuteString_Trivial_Trivial()
      {
         Assert.AreEqual("13:04", new DateTime(2014, 12, 3, 13, 4, 0).ToHourMinuteString());
      }

      [TestCase("14:15", "14:18", 15, true)]
      [TestCase("14:30", "14:18", 15, false)]
      [TestCase("14:15", "14:15", 15, false)]
      [TestCase("14:15", "14:15", 15, true)]
      [TestCase("15:00", "14:59", 15, false)]
      [TestCase("14:00", "14:01", 15, true)]
      public void RoundToMinute_Variable_Variable(string expected, string actual, int round, bool roundToLeft)
      {
         DateTime actualDate = DateTime.UtcNow.RoundToDay().Add(TimeSpan.Parse(actual));
         DateTime expectedDate = DateTime.UtcNow.RoundToDay().Add(TimeSpan.Parse(expected));
         DateTime convertedDate = actualDate.RoundToMinute(round, roundToLeft);

         Assert.AreEqual(convertedDate, expectedDate);
      }
   }
}
