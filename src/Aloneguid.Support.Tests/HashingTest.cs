extern alias Portable;
using System.IO;
using System.Text;
using Aloneguid.Support.Application;
using Aloneguid.Support.Model;
using NUnit.Framework;


namespace Aloneguid.Support.Tests
{
   [TestFixture(HashType.Md5)]
   [TestFixture(HashType.Sha1)]
   [TestFixture(HashType.Sha256)]
   [TestFixture(HashType.Sha384)]
   [TestFixture(HashType.Sha512)]
   [TestFixture(HashType.RipeMd160)]
   public class HashingTest
   {
      private readonly HashType _hashType;

      public HashingTest(HashType hashType)
      {
         _hashType = hashType;
      }

      [Test]
      public void Compute_PortableNonPortable_ResultMatches()
      {
         byte[] source = Encoding.UTF8.GetBytes("test");

         byte[] fullHash = Hashing.GetHash(source, _hashType);
         byte[] portableHash = Portable::Aloneguid.Support.Application.Hashing.GetHash(
            source,
            (Portable::Aloneguid.Support.Model.HashType) (int) _hashType);

         string fullHashString = Encoding.UTF8.GetString(fullHash);
         string portableHashString = Encoding.UTF8.GetString(portableHash);

         Assert.AreEqual(fullHashString, portableHashString);
      }

      [Test]
      public void Compute_MultipleHashesOnOneStream_ResultMatches()
      {
         using(var ms = new MemoryStream(Encoding.UTF8.GetBytes("test stream content")))
         {
            long lengthFull;
            byte[][] resultFull = Hashing.CalculateHashes(ms, out lengthFull, HashType.Md5, _hashType);

            ms.Position = 0;
            long lengthPortable;
            byte[][] resultPortable = Portable::Aloneguid.Support.Application.Hashing.CalculateHashes(
               ms, out lengthPortable,
               Portable::Aloneguid.Support.Model.HashType.Md5,
               (Portable::Aloneguid.Support.Model.HashType)(int)_hashType);

            Assert.AreEqual(lengthFull, lengthPortable);
         }
      }
   }
}
