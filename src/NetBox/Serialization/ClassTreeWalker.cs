using System;
using System.Collections.Generic;
using System.Reflection;
using NetBox;
using System.Collections;

namespace NetBox.Serialization
{
   /// <summary>
   /// Simplifies and speeds up the process of walking down the class structure, mostly useful
   /// but not limited to writing custom serializers. Designed for extreme performance when used
   /// with common sense in mind.
   /// </summary>
   public class ClassTreeWalker
   {
      private static readonly Dictionary<Type, ContainerInfo> _typeToInfo =
         new Dictionary<Type, ContainerInfo>();

      public delegate bool WalkDelegate(WalkEventArgs args);

      /// <summary>
      /// Walks down the type calling back the on each node
      /// </summary>
      /// <param name="obj">Type instance to walk</param>
      /// <param name="callback"></param>
      public void Walk(object obj, WalkDelegate callback)
      {
         if (obj == null || callback == null) return;

         //get or create type cache
         Type t = obj.GetType();
         ContainerInfo ct = _typeToInfo.GetOrAdd(t, () => new ContainerInfo(t));

         Walk(ct, obj, callback, 0);
      }

      private void Walk(ContainerInfo ct, object parent, WalkDelegate callback, int level)
      {
         foreach (Tuple<string, NodeInfo> tpl in EnumerableEx.MultiIterate(ct.PropNames, ct.TypeNodes))
         {
            if (!callback(new WalkEventArgs(tpl.Item1, tpl.Item2, parent, level))) return;

            if (tpl.Item2.HasChildren)
            {
               Type cht = tpl.Item2.RawType;
               ContainerInfo chct = _typeToInfo.GetOrAdd(tpl.Item2.RawType, () => new ContainerInfo(cht));
               object cv = tpl.Item2.GetValue(parent);

               Walk(chct, cv, callback, level + 1);
            }

            /*if(tpl.Item2.IsEnumerable)
            {
               //todo: for generic enumerables type of elements can be detected just once
               Type elemType = null;

               IEnumerable ie = tpl.Item2.GetValue(parent) as IEnumerable;
               if(ie != null)
               {
                  foreach(object iem in ie)
                  {
                     if (elemType == null) elemType = iem.GetType();

                     ContainerInfo chct = _typeToInfo.GetOrAdd(elemType, () => new ContainerInfo(elemType));
                     

                  }
               }
            }*/
         }

      }
   }
}
