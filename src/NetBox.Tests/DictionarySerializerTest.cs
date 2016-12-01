using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetBox.Application;
using Xunit;

namespace NetBox.Tests
{
   public class DictionarySerializerTest
   {
      [Fact]
      public void OneLevel_Serialize_Deserializes()
      {
         var obj = new SimpleOneLevelStringOnly("1", "2", "3", "4");

         Dictionary<string, object> result = new DictionarySerializer().Serialize(obj);

         Assert.Equal("1", result["StringPublicProperty"]);
         Assert.Equal("2", result["StringPrivateProperty"]);
         Assert.Equal("3", result["_publicStringMember"]);
         Assert.Equal("4", result["_privateStringMember"]);
      }
   }

   #region [ Test Data ]

   public class SimpleOneLevelStringOnly
   {
      public SimpleOneLevelStringOnly(string publicString, string privateString,
         string publicMember, string privateMember)
      {
         StringPublicProperty = publicString;
         StringPrivateProperty = privateString;
         _publicStringMember = publicMember;
         _privateStringMember = privateMember;
      }

      public string StringPublicProperty { get; set; }

      private string StringPrivateProperty { get; set; }

      public string _publicStringMember;

      private string _privateStringMember;
   }

   #endregion
}
