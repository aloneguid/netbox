using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace NetBox.Serialization.Core
{
   /// <summary>
   /// Node metainformation including sibling types
   /// </summary>
   public class Node
   {
      private readonly Func<object, object> _valueGetter;

      private readonly Action<object, object> _valueSetter;

      /// <summary>
      /// Name of the node, null for root node
      /// </summary>
      public string Name { get; }

      /// <summary>
      /// The raw type of this node
      /// </summary>
      public Type RawType { get; }

      /// <summary>
      /// Type of the node
      /// </summary>
      public NodeType NodeType { get; }

      /// <summary>
      /// Tree level, 0 for root node
      /// </summary>
      public int Level { get; private set; }

      /// <summary>
      /// Parent node, null for the root node
      /// </summary>
      public Node Parent { get; private set; }

      /// <summary>
      /// Child nodes
      /// </summary>
      public List<Node> Children { get; }

      /// <summary>
      /// Creates an instance of the node and discovers all the metadata
      /// </summary>
      /// <param name="t">Node type</param>
      public Node(Type t) : this(null, t, null, null, null, 0)
      {

      }

      private Node(string name, Type t,
         Func<object, object> valueGetter,
         Action<object, object> valueSetter,
         Node parent,
         int level)
      {
         Name = name;
         RawType = t;
         Level = level;
         Parent = parent;
         _valueGetter = valueGetter;
         _valueSetter = valueSetter;
         NodeType = DiscoverType(t);

         if (NodeType == NodeType.Container)
         {
            Children = new List<Node>();
            Discover(t);
         }
      }

      /// <summary>
      /// Gets the value of this node on a particular instance
      /// </summary>
      /// <param name="instance"></param>
      /// <returns></returns>
      public object GetValue(object instance)
      {
         return _valueGetter == null ? null : _valueGetter(instance);
      }

      /// <summary>
      /// Sets the value of this node for a particular instance
      /// </summary>
      /// <param name="instance"></param>
      /// <param name="value"></param>
      public void SetValue(object instance, object value)
      {
         if (_valueSetter == null) return;

         if (value != null)
         {
            if(value.GetType() != RawType)
            {
               var dv = new DynamicValue(value);
               value = dv.GetValue(RawType);
            }

            _valueSetter(instance, value);
         }
      }

      private static NodeType DiscoverType(Type t)
      {
         if(t.IsSimple())
         {
            return NodeType.Simple;
         }
         else
         {
            TypeInfo ti = t.GetTypeInfo();

            //case for classic arrays
            if (ti.IsArray)
            {
               //Type et = ti.GetElementType();
               if (ti.GetArrayRank() != 1) return NodeType.NotSupported;

               return NodeType.Collection;
            }

            Type[] interfaces = ti.GetInterfaces();
            bool isDirectGenericIEnumerable = ti.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>);
            bool isDirectIEnumerable = t == typeof(IEnumerable);
            bool hasGenericIEnumerableInterface = interfaces.Any(ifc => ifc.GetTypeInfo().IsGenericType && ifc.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            bool hasIEnumerableInterface = interfaces.Any(ifc => ifc == typeof(IEnumerable));

            //generic IEnumerable<T>
            if(isDirectGenericIEnumerable || hasGenericIEnumerableInterface)
            {
               return NodeType.Collection;
            }

            //classic IEnumerable
            if(isDirectIEnumerable || hasIEnumerableInterface)
            {
               return NodeType.NotSupported;
            }

            return NodeType.Container;
         }
      }

      private void Discover(Type t)
      {
         //discover properties
         IEnumerable<PropertyInfo> properties = t.GetRuntimeProperties();
         foreach (PropertyInfo pi in properties)
         {
            if (!(pi.CanRead || pi.CanWrite)) continue;

            string name = pi.Name;

            if (pi.GetMethod != null)
            {
               Func<object, object> valueGetter = pi.CanRead
                     ? (Func<object, object>)((instance) => pi.GetMethod.Invoke(instance, null))
                     : (_ => null);

               Action<object, object> valueSetter = pi.CanWrite
                  ? (Action<object, object>)((instance, value) => pi.SetMethod.Invoke(instance, new object[] { value }))
                  : null;

               var node = new Node(name, pi.PropertyType, valueGetter, valueSetter, this, Level + 1);
               Children.Add(node);
            }
         }

         //discover fields
         IEnumerable<FieldInfo> fields = t.GetRuntimeFields();
         foreach (FieldInfo fi in fields)
         {
            if (!fi.IsPublic) continue;

            string name = fi.Name;

            Func<object, object> valueGetter = (instance) => fi.GetValue(instance);
            Action<object, object> valueSetter = (instance, value) => fi.SetValue(instance, value);

            var node = new Node(name, fi.FieldType, valueGetter, valueSetter, this, Level + 1);
            Children.Add(node);
         }

      }

   }
}
