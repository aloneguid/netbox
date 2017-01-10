using NetBox.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetBox.Tests.Serialization
{
   public class JsonSerializerTest
   {
      [Fact]
      public void SerializeDeserialize_JsonClass_JsonClass()
      {
         var j1 = new JsonClass
         {
            s = "one",
            i = 2
         };

         var s = new JsonSerializer();
         var ms = new MemoryStream();
         s.Serialize(j1, ms);
         ms.Position = 0;

         string json = Encoding.UTF8.GetString(ms.ToArray());


         
      }
   }

   class JsonClass
   {
      public string s;

      public int i;
   }
}
