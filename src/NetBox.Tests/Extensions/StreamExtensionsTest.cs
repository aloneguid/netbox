using System;
using System.IO;
using System.Text;
using NetBox.Extensions;
using NetBox.Model;
using Xunit;

namespace NetBox.Tests.Extensions
{
   
   public class StreamExtensionsTest
   {
      [Fact]
      public void Hashing_GetOne_Calculates()
      {
         const string s = "my looooooong test string";

         using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
         {
            string hash = ms.GetHash(HashType.Sha256);

            Assert.Equal(s.GetHash(HashType.Sha256), hash);  //sha256
         }
         
      }

      public class NodeConfig
      {
         public TableConfig[] Tables { get; set; }

         public static NodeConfig LoadFromResourceByNodeName(string nodeName)
         {
            return new NodeConfig();
         }
      }

      public class TableConfig
      {
         public TableConfig()
         {

         }

         public TableConfig(string name)
         {
            TableName = name;
         }

         public string TableName { get; set; }

         public bool Push { get; set; }

         public bool Pull { get; set; }
      }
   }
}
