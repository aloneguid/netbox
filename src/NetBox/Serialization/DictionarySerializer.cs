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

      public object Deserialize(Type t, Dictionary<string, object> data)
      {
         return base.Deserialize(t, data);
      }

      /// <summary>
      /// Puts value into dictionary
      /// </summary>
      protected override void SerializeValue(Node node, object value, object state)
      {
         var result = state as Dictionary<string, object>;

         result[node.Name] = value;
      }

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
