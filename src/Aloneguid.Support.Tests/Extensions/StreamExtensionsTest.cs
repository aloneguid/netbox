using System;
using System.IO;
using System.Text;
using Aloneguid.Support.Model;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Extensions
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

      [Test]
      public void Json_FromStream_Deserializes()
      {
         var tag = new Tag { Name = "test string" };
         var ms = tag.ToJsonString().ToMemoryStream();
         ms.Position = 0;

         var tag2 = ms.ReadAsJsonObject<Tag>(Encoding.UTF8);
         Assert.AreEqual(tag.Name, tag2.Name);
      }

      private class Tag
      {
         public string Name { get; set; }
      }
   }
}
