using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetBox.Tests
{
   
   public class GeneratorTest
   {
      [Fact]
      public void RandomString_TwoStrings_NotEqual()
      {
         string s1 = Generator.RandomString;
         string s2 = Generator.RandomString;

         Assert.NotEqual(s1, s2);
      }

      [Theory]
      [InlineData(10)]
      [InlineData(100)]
      public void RandomString_SpecificLengthNoNulls_Matches(int length)
      {
         Assert.Equal(length, Generator.GetRandomString(length, false).Length);
      }

      [Fact]
      public void RandomBool_Anything_DoesntCrash()
      {
         bool b = Generator.RandomBool;
      }

#if NETFULL
      [Fact]
      public void RandomEnum_Random_Random()
      {
         EnumExample random = Generator.RandomEnum<EnumExample>();

         //not sure how to validate
      }
#endif

      [Fact]
      public void RandomEnumNonGeneric_Random_Random()
      {
         EnumExample random = (EnumExample)Generator.RandomEnum(typeof(EnumExample));

         //not sure how to validate
      }

      [Fact]
      public void RandomInt_Random_Random()
      {
         int i = Generator.RandomInt;
      }

      [Fact]
      public void RandomDate_Interval_Matches()
      {
         DateTime randomDate = Generator.GetRandomDate(DateTime.UtcNow, DateTime.UtcNow.AddDays(10));
      }

      [Fact]
      public void RandomDate_Random_Random()
      {
         DateTime randomDate = Generator.RandomDate;
      }


      [Theory]
      [InlineData(-10L, 100L)]
      [InlineData(5L, 10L)]
      [InlineData(-100L, -1L)]
      [InlineData(0, 67)]
      public void RandomLong_VaryingRange_InRange(long min, long max)
      {
         long random = Generator.GetRandomLong(min, max);

         Assert.True(random >= min);
         Assert.True(random <= max);
      }

      [Fact]
      public void RandomLong_TwoGenerations_NotEqual()
      {
         long l1 = Generator.RandomLong;
         long l2 = Generator.RandomLong;

         Assert.NotEqual(l1, l2);
      }

      [Fact]
      public void RandomUri_TwoGenerations_NotEqual()
      {
         Uri u1 = Generator.GetRandomUri(false);
         Uri u2 = Generator.RandomUri;

         Assert.NotEqual(u1, u2);
      }

      private enum EnumExample
      {
         One,
         Two,
         Three
      }
   }
}
