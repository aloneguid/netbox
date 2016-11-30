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
         var obj = new SimpleOneLevel() { StringProperty = "one" };

         var result = new DictionarySerializer().Serialize(obj);
         result = new DictionarySerializer().Serialize(obj);
      }
   }

   #region [ Test Data ]

   public class SimpleOneLevel
   {
      public string StringProperty { get; set; }
   }

   #endregion
}
