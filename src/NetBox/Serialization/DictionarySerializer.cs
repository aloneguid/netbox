using NetBox.Serialization.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBox.Serialization
{
   public class DictionarySerializer : WalkingSerializer
   {
      public Dictionary<string, object> Serialize(object instance)
      {
         var result = new Dictionary<string, object>();

         base.Serialize(instance, result);

         return result;
      }

      protected override void SerializeValue(Node node, object value, object state)
      {
         var result = state as Dictionary<string, object>;

         result[node.Name] = value;
      }
   }
}
