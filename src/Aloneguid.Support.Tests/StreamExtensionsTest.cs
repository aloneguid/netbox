using System;
using System.IO;
using System.Text;
using Aloneguid.Support.Model;
using NUnit.Framework;

namespace Aloneguid.Support.Tests
{
   [TestFixture]
   public class StreamExtensionsTest
   {
      [Test]
      public void Hashing_AllKnownAlgorithms_Calculates()
      {
         const string s = "my looooooong test string";

         using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
         {
            string[] hashes = ms.GetHashes(
               HashType.Md5,
               HashType.Sha256,
               HashType.Sha1,
               HashType.Sha384,
               HashType.Sha512,
               HashType.RipeMd160);

            Assert.AreEqual(s.GetHash(HashType.Md5), hashes[0]);
            Assert.AreEqual(s.GetHash(HashType.Sha256), hashes[1]);
            Assert.AreEqual(s.GetHash(HashType.Sha1), hashes[2]);
            Assert.AreEqual(s.GetHash(HashType.Sha384), hashes[3]);
            Assert.AreEqual(s.GetHash(HashType.Sha512), hashes[4]);
            Assert.AreEqual(s.GetHash(HashType.RipeMd160), hashes[5]);
         }
      }

      [Test]
      public void Hashing_GetOne_Calculates()
      {
         const string s = "my looooooong test string";

         using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
         {
            string hash = ms.GetHash(HashType.Sha256);

            Assert.AreEqual(s.GetHash(HashType.Sha256), hash);  //sha256
         }
         
      }
   }
}
