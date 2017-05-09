using System;
using System.Collections.Generic;
using System.Text;
using NetBox.Serialization.Core;

namespace NetBox.Serialization
{
   /// <summary>
   /// Serializes only the first level of class properties i.e. the simple ones.
   /// Comples properties are serialized as JSON
   /// </summary>
   public class TopLevelDictionarySerializer : WalkingSerializer
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

         if(node.NodeType == NodeType.Simple)
         {
            result[node.Name] = value;
         }
      }

      protected override bool BeforeContainerSerialize(Node node, object state, object instance, out object newState)
      {
         newState = state;

         if (node.Name == null)
         {
            return true;
         }

         var dstate = state as Dictionary<string, object>;
         dstate[node.Name] = instance.ToJsonString(true, true);

         return false;
      }
   }
}
