//extern alias Portable;
using System;
using System.IO;
using NetBox.Extensions;
using NetBox.Model;
using Xunit;


namespace NetBox.Tests
{
   public class HashingTest
   {
      [Theory]
      [InlineData(HashType.Md5)]
      [InlineData(HashType.Sha1)]
      [InlineData(HashType.Sha256)]
      [InlineData(HashType.Sha384)]
      [InlineData(HashType.Sha512)]
      public void Compute_PortableNonPortable_ResultMatches(HashType hashType)
      {
         string source = "test";

         string fullHash = source.GetHash(hashType);
      }

      [Theory]
      [InlineData(HashType.Sha256, "", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
      [InlineData(HashType.Sha256, "123", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3")]
      [InlineData(HashType.HMACSHA256, "123", "e132d6f8461792705d7561c18bb1971f5ce0120776ec199a068ac470c02f0a92", "salt1")]
      [InlineData(HashType.HMACSHA256, "123", "8e547fd1a432dd5c5e34ad0412e2aac0f5f066f7d4b3b45f1ac0f178e5e737c3", "salt2")]
      public void Hashing_theory(HashType hashType, string input, string expected, string salt = null)
      {
         string actual = input.GetHash(hashType, salt);

         Assert.Equal(expected, actual);
      }

   }
}
