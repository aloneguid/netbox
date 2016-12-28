using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetBox.Serialization
{
   class ContainerInfo
   {
      //fact: comparing string is 3 times faster than comparing System.Type for equality

      public ContainerInfo(Type t)
      {
         Type = t;
         FullTypeName = t.FullName;

         Discover();
      }

      public readonly Type Type;

      public readonly string FullTypeName;

      public readonly List<string> PropNames = new List<string>();

      public readonly List<NodeInfo> TypeNodes = new List<NodeInfo>();

      private void Discover()
      {
         //discover properties
         IEnumerable<PropertyInfo> properties = Type.GetRuntimeProperties();
         foreach (PropertyInfo pi in properties)
         {
            if (!(pi.CanRead || pi.CanWrite)) continue;

            string name = pi.Name;

            if (pi.GetMethod != null)
            {
               var node = new NodeInfo(pi.PropertyType,
                  pi.CanRead
                     ? (Func<object, object>)(_ => pi.GetMethod.Invoke(_, null))
                     : (_ => null),
                  null);

               PropNames.Add(name);
               TypeNodes.Add(node);
            }
         }

         //discover fields
         IEnumerable<FieldInfo> fields = Type.GetRuntimeFields();
         foreach (FieldInfo fi in fields)
         {
            if (!fi.IsPublic) continue;

            string name = fi.Name;

            var node = new NodeInfo(fi.FieldType,
               _ => fi.GetValue(_),
               null);

            PropNames.Add(name);
            TypeNodes.Add(node);
         }
      }
   }
}
