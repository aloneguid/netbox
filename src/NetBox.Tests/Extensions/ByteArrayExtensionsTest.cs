using System;
using System.IO;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class ByteArrayExtensionsTest : TestBase
   {
      [Theory]
      [InlineData("0", null, null)]
      [InlineData("1", new byte[] { }, "")]
      [InlineData("2", new byte[] { 0, 1, 2, 3, 4, 5 }, "000102030405")]
      public void ToHexString_Variable_Variable(string name, byte[] input, string expected)
      {
         string actual = input.ToHexString();

         Assert.Equal(expected, actual);
      }

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

      [Fact]
      public void Concat_TwoArrays_NewArray()
      {
         char[] left = new[] { 'n' };
         char[] right = new[] { 'e', 'w' };

         char[] na = left.Concat(right);

         Assert.Equal("new", new string(na));
      }

      [Fact]
      public void Concat_WithNull_Left()
      {
         char[] left = new[] { 'l', 'e' };
         char[] r = left.Concat(null);
         Assert.Equal("le", new string(r));
      }

   }
}
