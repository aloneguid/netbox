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
