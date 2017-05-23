using NetBox.Serialization.Core;
using System;
using System.Collections.Concurrent;

namespace NetBox.Serialization
{
   /// <summary>
   /// A base for building class serializers. Performs a function of caching type metadata and calling back to a derived class
   /// with a high-level event data. Can't be used on it's own.
   /// </summary>
   public abstract class WalkingSerializer
   {
      private static ConcurrentDictionary<Type, Node> typeToNode = new ConcurrentDictionary<Type, Node>();

      /// <summary>
      /// Call from derived class to kick off the serialization process
      /// </summary>
      /// <param name="instance">Object instance to serialize</param>
      /// <param name="state">Optional state which will be passed through the chain of protected methods</param>
      protected void Serialize(object instance, object state)
      {
         Type t = instance.GetType();

         Node node = typeToNode.GetOrAdd(t, _ => new Node(_));

         SerializeWalk(node, instance, state);

      }

      /// <summary>
      /// Deserializes the specified type 
      /// </summary>
      /// <param name="t">The type to deserialize</param>
      /// <param name="state">The state to be used to deserialize the object. Usually it's an object
      /// instance but it can be anything your custom serializer needs to do the job.</param>
      /// <returns></returns>
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
            object containerState;
            object containerInstance = node.Level == 0
               ? instance
               : node.GetValue(instance);
            bool proceed = BeforeContainerSerialize(node, state, containerInstance, out containerState);

            if (proceed)
            {

               foreach (Node child in node.Children)
               {
                  SerializeWalk(child, containerInstance, containerState);
               }
            }

            state = AfterContainerSerialize(node, containerState, state);
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
            bool proceed = BeforeContainerDeserialize(node, state, instance, out object containerState);

            if (proceed)
            {
               object containerInstance;
               if (node.Level == 0)
               {
                  containerInstance = instance;
               }
               else
               {
                  containerInstance = Activator.CreateInstance(node.RawType);
                  node.SetValue(instance, containerInstance);
               }

               foreach (Node child in node.Children)
               {
                  DeserializeWalk(child, containerInstance, containerState);
               }
            }

            state = AfterContainerDeserialize(node, containerState, state);
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
      /// Deserializes the value from specified node.
      /// </summary>
      /// <param name="node">The node to use.</param>
      /// <param name="state">The state of the serializer.</param>
      /// <returns>Must return node value and in correct type.</returns>
      protected virtual object DeserializeValue(Node node, object state)
      {
         return null;
      }

      /// <summary>
      /// Called to start container during serialization
      /// </summary>
      /// <param name="node"></param>
      /// <param name="state"></param>
      /// <param name="instance"></param>
      /// <param name="newState"></param>
      /// <returns>When false, won't proceed deep inside the container</returns>
      protected virtual bool BeforeContainerSerialize(Node node, object state, object instance, out object newState)
      {
         newState = state;
         return true;
      }

      /// <summary>
      /// Happends before container deserialization
      /// </summary>
      /// <param name="node">Current node</param>
      /// <param name="state">Current state</param>
      /// <param name="instance">Container instance</param>
      /// <param name="newState">New state to use in next calls</param>
      /// <returns>True to proceeed walking down, otherwise false</returns>
      protected virtual bool BeforeContainerDeserialize(Node node, object state, object instance, out object newState)
      {
         newState = state;
         return true;
      }

      /// <summary>
      /// Called to stop container during serialization
      /// </summary>
      /// <param name="node"></param>
      protected virtual object AfterContainerSerialize(Node node, object containerState, object previousState)
      {
         return previousState;
      }

      protected virtual object AfterContainerDeserialize(Node node, object containerState, object previousState)
      {
         return previousState;
      }
   }
}
