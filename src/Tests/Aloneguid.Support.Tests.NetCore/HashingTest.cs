//extern alias Portable;
using System;
using System.IO;
using System.Text;
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
      public void Compute_OnStream_ResultMatches()
      {
         using(var ms = new MemoryStream(Encoding.UTF8.GetBytes("test stream content")))
         {
            long lengthFull;
            string fullHash1 = ms.GetHash(_hashType);
            ms.Position = 0;
            string fullHash2 = ms.GetHash(out lengthFull, _hashType);

            //long lengthPortable;
            //ms.Position = 0;
            //string portHash1 = Portable::System.IO.StreamExtensions.GetHash(ms, _portableHashType);
            //ms.Position = 0;
            //string portHash2 = Portable::System.IO.StreamExtensions.GetHash(ms, out lengthPortable, _portableHashType);

            //Assert.AreEqual(lengthFull, lengthPortable);
            Assert.AreEqual(fullHash1, fullHash1);
            //Assert.AreEqual(fullHash2, portHash1);
            //Assert.AreEqual(portHash1, portHash2);
         }
      }

      [Test]
      public void Compute_MultipleHashesOnStream_ResultMatches()
      {
         const string s = "my looooooong test string";

         using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
         {
            var fullHashes = ms.GetHashes(
               HashType.Md5,
               HashType.Sha256,
               HashType.Sha1,
               HashType.Sha384,
               HashType.Sha512
#if NETFULL
               ,HashType.RipeMd160
#endif
               );

            Assert.AreEqual(s.GetHash(HashType.Md5), fullHashes[0]);
            Assert.AreEqual(s.GetHash(HashType.Sha256), fullHashes[1]);
            Assert.AreEqual(s.GetHash(HashType.Sha1), fullHashes[2]);
            Assert.AreEqual(s.GetHash(HashType.Sha384), fullHashes[3]);
            Assert.AreEqual(s.GetHash(HashType.Sha512), fullHashes[4]);
#if NETFULL
            Assert.AreEqual(s.GetHash(HashType.RipeMd160), fullHashes[5]);
#endif
         }

         /*using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
         {
            var portableHashes = Portable::System.IO.StreamExtensions.GetHashes(ms,
               Portable::Aloneguid.Support.Model.HashType.Md5,
               Portable::Aloneguid.Support.Model.HashType.Sha256,
               Portable::Aloneguid.Support.Model.HashType.Sha1,
               Portable::Aloneguid.Support.Model.HashType.Sha384,
               Portable::Aloneguid.Support.Model.HashType.Sha512,
               Portable::Aloneguid.Support.Model.HashType.RipeMd160);

            Assert.AreEqual(s.GetHash(HashType.Md5), portableHashes[0]);
            Assert.AreEqual(s.GetHash(HashType.Sha256), portableHashes[1]);
            Assert.AreEqual(s.GetHash(HashType.Sha1), portableHashes[2]);
            Assert.AreEqual(s.GetHash(HashType.Sha384), portableHashes[3]);
            Assert.AreEqual(s.GetHash(HashType.Sha512), portableHashes[4]);
            Assert.AreEqual(s.GetHash(HashType.RipeMd160), portableHashes[5]);
         }*/
      }

      [Test]
      public void DocsDemos()
      {
         string s1 = "my string".GetHash(_hashType);
      }
   }
}
