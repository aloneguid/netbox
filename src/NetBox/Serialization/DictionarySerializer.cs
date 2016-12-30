using NetBox.Serialization.Core;
using System.Collections.Generic;

namespace NetBox.Serialization
{
   /// <summary>
   /// Serializes objects into dictionary
   /// </summary>
   public class DictionarySerializer : WalkingSerializer
   {
      /// <summary>
      /// Serializes instance into a dictionary
      /// </summary>
      /// <param name="instance"></param>
      /// <returns></returns>
      public Dictionary<string, object> Serialize(object instance)
      {
         var result = new Dictionary<string, object>();

         base.Serialize(instance, result);

         return result;
      }

      /// <summary>
      /// Puts value into dictionary
      /// </summary>
      protected override void SerializeValue(Node node, object value, object state)
      {
         var result = state as Dictionary<string, object>;

         result[node.Name] = value;
      }
   }
}
