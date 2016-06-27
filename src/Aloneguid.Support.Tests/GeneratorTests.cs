using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aloneguid.Support.Tests
{
   [TestFixture]
   public class GeneratorTest
   {
      [Test]
      public void RandomString_TwoStrings_NotEqual()
      {
         string s1 = Generator.RandomString;
         string s2 = Generator.RandomString;

         Assert.AreNotEqual(s1, s2);
      }

      [TestCase(10)]
      [TestCase(100)]
      public void RandomString_SpecificLengthNoNulls_Matches(int length)
      {
         Assert.AreEqual(length, Generator.GetRandomString(length, false).Length);
      }

      [Test]
      public void RandomBool_Anything_DoesntCrash()
      {
         bool b = Generator.RandomBool;
      }

      [Test]
      public void RandomEnum_Random_Random()
      {
         EnumExample random = Generator.RandomEnum<EnumExample>();

         //not sure how to validate
      }

      [Test]
      public void RandomEnumNonGeneric_Random_Random()
      {
         EnumExample random = (EnumExample)Generator.RandomEnum(typeof(EnumExample));

         //not sure how to validate
      }

      [Test]
      public void RandomInt_Random_Random()
      {
         int i = Generator.RandomInt;
      }

      // ReSharper disable NUnit.MethodWithParametersAndTestAttribute
      [Test]
      public void RandomInt_MaxValue_LessThan(
         [Random(0, int.MaxValue, 100)]
         int maxValue)
      {
         int i = Generator.GetRandomInt(maxValue);

         Assert.Less(i, maxValue);
      }
      // ReSharper restore NUnit.MethodWithParametersAndTestAttribute

      [Test]
      public void RandomDate_Interval_Matches()
      {
         DateTime randomDate = Generator.GetRandomDate(DateTime.UtcNow, DateTime.UtcNow.AddDays(10));
      }

      [Test]
      public void RandomDate_Random_Random()
      {
         DateTime randomDate = Generator.RandomDate;
      }


      [TestCase(-10L, 100L)]
      [TestCase(5L, 10L)]
      [TestCase(-100L, -1L)]
      public void RandomLong_VaryingRange_InRange(long min, long max)
      {
         long random = Generator.GetRandomLong(min, max);

         Assert.GreaterOrEqual(random, min);
         Assert.LessOrEqual(random, max);
      }

      [Test]
      public void RandomLong_TwoGenerations_NotEqual()
      {
         long l1 = Generator.RandomLong;
         long l2 = Generator.RandomLong;

         Assert.AreNotEqual(l1, l2);
         Assert.Pass("{0} != {1}", l1, l2);
      }

      [Test]
      public void RandomUri_TwoGenerations_NotEqual()
      {
         Uri u1 = Generator.GetRandomUri(false);
         Uri u2 = Generator.RandomUri;

         Assert.AreNotEqual(u1, u2);
      }

      private enum EnumExample
      {
         One,
         Two,
         Three
      }
   }
}
