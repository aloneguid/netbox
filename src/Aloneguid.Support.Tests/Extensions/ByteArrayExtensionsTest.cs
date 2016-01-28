using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
{
   [TestFixture]
   public class ByteArrayExtensionsTest : TestBase
   {
      [TestCase("0", null, null)]
      [TestCase("1", new byte[] { }, "")]
      [TestCase("2", new byte[] { 0, 1, 2, 3, 4, 5 }, "000102030405")]
      public void ToHexString_Variable_Variable(string name, byte[] input, string expected)
      {
         string actual = input.ToHexString();

         Assert.AreEqual(expected, actual);
      }

      [Test]
      public void Compress_Array_Decompresses()
      {
         byte[] input = GetTestData("book.txt").ToByteArray();
         byte[] output = input.Gzip();
         Assert.Less(output.Length, input.Length);

         byte[] input2 = output.Ungzip();
         Assert.AreEqual(input, input2);
      }
   }
}
