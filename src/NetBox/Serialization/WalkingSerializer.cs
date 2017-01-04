using NetBox.Serialization.Core;
using System;
using System.Collections.Concurrent;

namespace NetBox.Serialization
{
   /// <summary>
   /// A base for building class serializers. Performs a function of caching type metadata and calling back to a derived class
   /// with a high-level event data. Can't be used on it's own.
   /// 
   /// This class is still in development!
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

         SerializeWalk(node, instance, state);

      }

      protected object Deserialize(Type t, object state)
      {
         Node node = typeToNode.GetOrAdd(t, _ => new Node(_));

         object result = Activator.CreateInstance(t);

         DeserializeWalk(node, result, state);

         return result;
      }

      private void SerializeWalk(Node node, object instance, object state)
      {
         if(node.NodeType == NodeType.Simple)
         {
            SerializeValue(node, node.GetValue(instance), state);
         }
         else if(node.NodeType == NodeType.Container)
         {
            object containerState = StartContainer(node, state);

            foreach(Node child in node.Children)
            {
               object containerInstance = node.Level == 0
                  ? instance
                  : node.GetValue(instance);

               SerializeWalk(child, containerInstance, containerState);
            }

            state = StopContainer(node, containerState, state);
         }
      }

      private void DeserializeWalk(Node node, object instance, object state)
      {
         if(node.NodeType == NodeType.Simple)
         {
            object value = DeserializeValue(node, state);

            if(value != null)
            {
               node.SetValue(instance, value);
            }
         }
         else if(node.NodeType == NodeType.Container)
         {
            foreach(Node child in node.Children)
            {
               object containerInstance = node.Level == 0
                  ? instance
                  : node.GetValue(instance);

               DeserializeWalk(child, containerInstance, state);
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

      protected virtual object DeserializeValue(Node node, object state)
      {
         return null;
      }

      /// <summary>
      /// Called to start container
      /// </summary>
      /// <param name="node"></param>
      /// <param name="state"></param>
      protected virtual object StartContainer(Node node, object state)
      {
         return state;
      }

      /// <summary>
      /// Called to stop container
      /// </summary>
      /// <param name="node"></param>
      protected virtual object StopContainer(Node node, object containerState, object previousState)
      {
         return previousState;
      }
   }
}
