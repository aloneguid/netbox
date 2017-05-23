using System;
using System.Collections.Generic;
using System.Text;
using NetBox.Serialization.Core;

namespace NetBox.Serialization
{
   /// <summary>
   /// Serializes only the first level of class properties i.e. the simple ones.
   /// Complex properties are serialized as JSON strings.
   /// </summary>
   public class TopLevelDictionarySerializer : WalkingSerializer
   {
      /// <summary>
      /// Serializes top-level properties
      /// </summary>
      /// <param name="instance">Object instance</param>
      /// <returns>Map of property name to property value, where complex properties are represented as json strings</returns>
      public Dictionary<string, object> Serialize(object instance)
      {
         var result = new Dictionary<string, object>();

         base.Serialize(instance, result);

         return result;
      }

      /// <summary>
      /// Deserializes object map
      /// </summary>
      public object Deserialize(Type t, Dictionary<string, object> data)
      {
         return base.Deserialize(t, data);
      }

      /// <summary>
      /// Puts property into the dictionary
      /// </summary>
      protected override void SerializeValue(Node node, object value, object state)
      {
         var result = state as Dictionary<string, object>;

         if(node.NodeType == NodeType.Simple)
         {
            result[node.Name] = value;
         }
      }

      /// <summary>
      /// Puts the whole complex object into the dictionary as json string
      /// </summary>
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

      /// <summary>
      /// Puts dictionary property back to the object
      /// </summary>
      protected override object DeserializeValue(Node node, object state)
      {
         var d = state as Dictionary<string, object>;

         object value;
         d.TryGetValue(node.Name, out value);

         return value;
      }

      /// <summary>
      /// Deserializes JSON into the complex object
      /// </summary>
      protected override bool BeforeContainerDeserialize(Node node, object state, object instance, out object newState)
      {
         if (node.Name == null)
         {
            newState = state;
            return true;
         }

         var master = state as Dictionary<string, object>;
         master.TryGetValue(node.Name, out object jsonObject);
         string json = jsonObject as string;
         if(json != null)
         {
            object containerValue = json.AsJsonObject(node.RawType);
            node.SetValue(instance, containerValue);
         }

         newState = state;
         return false;
      }
   }
}
