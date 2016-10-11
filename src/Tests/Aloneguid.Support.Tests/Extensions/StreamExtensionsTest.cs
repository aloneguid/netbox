using System;
using System.IO;
using System.Text;
using Aloneguid.Support.Model;
using Xunit;

namespace Aloneguid.Support.Tests.Extensions
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

      [Fact]
      public void Json_FromStream_Deserializes()
      {
         var tag = new NodeConfig { Tables = new[] { new TableConfig("test string") } };
         var ms = tag.ToJsonString().ToMemoryStream();
         ms.Position = 0;

         var tag2 = ms.ReadAsJsonObject<NodeConfig>(Encoding.UTF8);
         Assert.Equal(tag.Tables[0].TableName, tag2.Tables[0].TableName);
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
