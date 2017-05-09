using Xunit;
using NetBox.Serialization;
using System.Collections.Generic;

namespace NetBox.Tests.Serialization
{
   public class TopLevelDictionarySerializerTest
   {
      public class TopLevel
      {
         public string M1 { get; set; }

         public int M2 { get; set; }

         public string M3 { get; set; }

         public LowLevel L { get; set; }

         public LowLevel LNull { get; set; }
      }

      public class LowLevel
      {
         public string M2 { get; set; }
      }

      [Fact]
      public void Serialize_TopLevelOnly_Deserializes()
      {
         var t1 = new TopLevel
         {
            M1 = "m1",
            M2 = 4,
            L = new LowLevel
            {
               M2 = "m2"
            }
         };

         var s = new TopLevelDictionarySerializer();

         //serialize
         Dictionary<string, object> d1 = s.Serialize(t1);
         Assert.NotNull(d1);

         //deserialize
      }
   }


}
