//extern alias Portable;
using System;
using System.IO;
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
#if NETFULL
   [InlineData(HashType.RipeMd160)]
#endif
      public void Compute_PortableNonPortable_ResultMatches(HashType hashType)
      {
         string source = "test";

         string fullHash = source.GetHash(hashType);
      }

   }
}
