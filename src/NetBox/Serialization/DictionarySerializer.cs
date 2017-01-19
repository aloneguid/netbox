using NetBox.Serialization.Core;
using System;
using System.Collections.Generic;

namespace NetBox.Serialization
{
   /// <summary>
   /// Serializes objects into dictionary. This class is still in development!
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
      /// Deserializes a type from dictionary data
      /// </summary>
      /// <param name="t">The type to deserialize</param>
      /// <param name="data">The dictionary data</param>
      /// <returns></returns>
      public object Deserialize(Type t, Dictionary<string, object> data)
      {
         return base.Deserialize(t, data);
      }

      /// <summary>
      /// Puts value into intermediate dictionary
      /// </summary>
      /// <param name="node">Working node</param>
      /// <param name="value">Value to put</param>
      /// <param name="state">Serializer state which is a current container</param>
      protected override void SerializeValue(Node node, object value, object state)
      {
         var result = state as Dictionary<string, object>;

         result[node.Name] = value;
      }

      /// <summary>
      /// Deserializes the value from current state container by fetching by key
      /// </summary>
      /// <param name="node">Current node</param>
      /// <param name="state">Current state which is a dictionary for current node</param>
      /// <returns></returns>
      protected override object DeserializeValue(Node node, object state)
      {
         var d = state as Dictionary<string, object>;

         object value;
         d.TryGetValue(node.Name, out value);

         return value;
      }

      protected override object BeforeContainerSerialize(Node node, object state)
      {
         if (node.Name == null) return state;

         var master = state as Dictionary<string, object>;
         var container = new Dictionary<string, object>();
         master[node.Name] = container;

         return container;
      }

      protected override object BeforeContainerDeserialize(Node node, object state)
      {
         if (node.Name == null) return state;

         var master = state as Dictionary<string, object>;
         object container;
         master.TryGetValue(node.Name, out container);

         return container;
      }
   }
}
