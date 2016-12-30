using NetBox.Serialization.Core;
using System;
using System.Collections.Concurrent;

namespace NetBox.Serialization
{
   /// <summary>
   /// A base for building class serializers
   /// </summary>
   public abstract class WalkingSerializer
   {
      private static ConcurrentDictionary<Type, Node> typeToNode = new ConcurrentDictionary<Type, Node>();

      /// <summary>
      /// Call from derived class to kick off the serialization process
      /// </summary>
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

      /// <summary>
      /// Called to serialize value
      /// </summary>
      /// <param name="node"></param>
      /// <param name="value"></param>
      /// <param name="state"></param>
      protected virtual void SerializeValue(Node node, object value, object state)
      {

      }

      /// <summary>
      /// Called to start container
      /// </summary>
      /// <param name="node"></param>
      protected virtual void StartContainer(Node node)
      {

      }

      /// <summary>
      /// Called to stop container
      /// </summary>
      /// <param name="node"></param>
      protected virtual void StopContainer(Node node)
      {

      }
   }
}
