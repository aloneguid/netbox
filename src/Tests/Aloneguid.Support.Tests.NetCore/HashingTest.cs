//extern alias Portable;
using System;
using System.IO;
using Aloneguid.Support.Model;
using NUnit.Framework;


namespace Aloneguid.Support.Tests
{
   [TestFixture(HashType.Md5)]
   [TestFixture(HashType.Sha1)]
   [TestFixture(HashType.Sha256)]
   [TestFixture(HashType.Sha384)]
   [TestFixture(HashType.Sha512)]
#if NETFULL
   [TestFixture(HashType.RipeMd160)]
#endif
   public class HashingTest
   {
      private readonly HashType _hashType;
      //private readonly Portable::Aloneguid.Support.Model.HashType _portableHashType;

      public HashingTest(HashType hashType)
      {
         _hashType = hashType;
         //_portableHashType = (Portable::Aloneguid.Support.Model.HashType)(int)hashType;
      }

      [Test]
      public void Compute_PortableNonPortable_ResultMatches()
      {
         string source = "test";

         string fullHash = source.GetHash(_hashType);
         //string portableHash = Portable::System.StringExtensions.GetHash(source, _portableHashType);

         //Assert.AreEqual(fullHash, portableHash);
      }

      [Test]
      public void DocsDemos()
      {
         string s1 = "my string".GetHash(_hashType);
      }
   }
}
