using System;
using System.IO;
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

      [Test]
      public void IsGzipped_GzippedArray_ReturnsTrue()
      {
         byte[] gzipped = Generator.GetRandomBytes(10000, 100000).Gzip();

         Assert.IsTrue(gzipped.IsGzipped());
      }

      [Test]
      public void IsGzipped_RandomArray_ReturnsFalse()
      {
         byte[] randomBytes = Generator.GetRandomBytes(10, 100);

         Assert.IsFalse(randomBytes.IsGzipped());
      }

   }
}
