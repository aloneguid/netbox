using System;
using System.IO;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class ByteArrayExtensionsTest : TestBase
   {
      [InlineData("0", null, null)]
      [InlineData("1", new byte[] { }, "")]
      [InlineData("2", new byte[] { 0, 1, 2, 3, 4, 5 }, "000102030405")]
      public void ToHexString_Variable_Variable(string name, byte[] input, string expected)
      {
         string actual = input.ToHexString();

         Assert.Equal(expected, actual);
      }

#if NETFULL
      [Fact]
      public void Compress_Array_Decompresses()
      {
         byte[] input = GetTestData("book.txt").ToByteArray();
         byte[] output = input.Gzip();
         Assert.True(output.Length < input.Length);

         byte[] input2 = output.Ungzip();
         Assert.Equal(input, input2);
      }
#endif

      [Fact]
      public void IsGzipped_GzippedArray_ReturnsTrue()
      {
         byte[] gzipped = Generator.GetRandomBytes(10000, 100000).Gzip();

         Assert.True(gzipped.IsGzipped());
      }

      [Fact]
      public void IsGzipped_RandomArray_ReturnsFalse()
      {
         byte[] randomBytes = Generator.GetRandomBytes(10, 100);

         Assert.False(randomBytes.IsGzipped());
      }

   }
}
