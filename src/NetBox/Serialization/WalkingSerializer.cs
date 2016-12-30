using NetBox.Serialization.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetBox.Serialization
{
   public abstract class WalkingSerializer
   {
      private static ConcurrentDictionary<Type, Node> typeToNode = new ConcurrentDictionary<Type, Node>();

      protected void Serialize(object instance, object state)
      {
         Type t = instance.GetType();

         Node node = typeToNode.GetOrAdd(t, _ => new Node(_));

         Walk(node, instance, state);

      }

      private void Walk(Node node, object instance, object state)
      {
         if(node.NodeType == NodeType.Simple)
         {
            SerializeValue(node, node.GetValue(instance), state);
         }
         else if(node.NodeType == NodeType.Container)
         {
            foreach(Node child in node.Children)
            {
               object containerInstance = node.Level == 0
                  ? instance
                  : node.GetValue(instance);

               Walk(child, containerInstance, state);
            }
         }
      }

      protected abstract void SerializeValue(Node node, object value, object state);
   }
}
