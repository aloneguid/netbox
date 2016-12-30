using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetBox.Serialization.Core
{
   public class Node
   {
      private readonly Func<object, object> _valueGetter;

      private readonly Action<object, object> _valueSetter;

      /// <summary>
      /// Name of the node, null for root node
      /// </summary>
      public string Name { get; }

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

      public object GetValue(object instance)
      {
         return _valueGetter == null ? null : _valueGetter(instance);
      }

      public void SetValue(object instance, object value)
      {
         if (_valueSetter == null) return;

         _valueSetter(instance, value);
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
               //todo: check that rank is 1, otherwise we don't support this

               //ElementType = ti.GetElementType();
               //if (ti.GetArrayRank() != 1)

               return NodeType.Collection;
            }

            //generic IEnumerable<T>
            Type enumType = ti.GetInterface(typeof(IEnumerable<>).FullName);
            if (enumType != null)
            {
               Type[] args = ti.GetGenericArguments();

               if(args.Length == 1)
               {
                  return NodeType.Collection;
               }
               else
               {
                  return NodeType.NotSupported;
               }
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

               Action<object, object> valueSetter = null;

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
