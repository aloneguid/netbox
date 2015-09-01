using System;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class ByteArrayExtensionsTest
   {
      [TestCase("0", null, null)]
      [TestCase("1", new byte[] { }, "")]
      [TestCase("2", new byte[] { 0, 1, 2, 3, 4, 5 }, "000102030405")]
      public void ToHexString_Variable_Variable(string name, byte[] input, string expected)
      {
         string actual = input.ToHexString();

         Assert.AreEqual(expected, actual);
      }
   }
}
