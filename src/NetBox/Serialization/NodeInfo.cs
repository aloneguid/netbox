using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace NetBox.Serialization
{
   public class NodeInfo
   {
      public NodeInfo(Type t,
         Func<object, object> valueGettter,
         Action<object, object> valueSetter)
      {
         RawType = t;
         _valueGetter = valueGettter;
         _valueSetter = valueSetter;
         HasChildren = !t.IsSimple();
         IsSupported = true;

         DetectArray();
      }

      private void DetectArray()
      {
         TypeInfo ti = RawType.GetTypeInfo();

         //case for classic arrays
         if(ti.IsArray)
         {
            IsEnumerable = true;
            ElementType = ti.GetElementType();
            HasChildren = false;

            if(ti.GetArrayRank() != 1)
            {
               IsSupported = false;
            }

            return;
         }
         
         //generic IEnumerable<T>
         Type enumType = ti.GetInterface(typeof(IEnumerable).FullName);
         IsEnumerable = enumType != null;
         if (IsEnumerable)
         {
            Type[] args = ti.GetGenericArguments();

            throw new NotImplementedException();
         }
      }

      /// <summary>
      /// Raw type of this node
      /// </summary>
      public Type RawType { get; }

      public Type ElementType { get; private set; }

      /// <summary>
      /// Indicates whether the type should be walked into
      /// </summary>
      public bool HasChildren { get; private set; }

      /// <summary>
      /// Whether this type must be enumerated
      /// </summary>
      public bool IsEnumerable { get; private set; }

      public bool IsSupported { get; private set; }

      private Func<object, object> _valueGetter;

      private Action<object, object> _valueSetter;

      /// <summary>
      /// Gets the raw value of the node
      /// </summary>
      /// <param name="parent">The object this node belongs to</param>
      /// <returns></returns>
      public object GetValue(object parent)
      {
         return _valueGetter(parent);
      }
   }
}