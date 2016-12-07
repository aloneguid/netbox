using System;
using System.Collections.Generic;
using System.Reflection;
using NetBox;

namespace NetBox.Serialization
{
   /// <summary>
   /// Simplifies and speeds up the process of walking down the class structure, mostly useful
   /// but not limited to writing custom serializers. Designed for extreme performance when used
   /// with common sense in mind.
   /// </summary>
   public class ClassTreeWalker
   {
      private static readonly Dictionary<Type, CachedType> _typeToInfo =
         new Dictionary<Type, CachedType>();

      /// <summary>
      /// 
      /// </summary>
      /// <param name="name"></param>
      /// <param name="node"></param>
      /// <param name="parent"></param>
      /// <param name="level"></param>
      /// <returns>When true, walking in current level will stop and proceed to parent level</returns>
      public delegate bool WalkDelegate(string name, TypeNode node, object parent, int level);

      public void Walk(object obj, WalkDelegate callback)
      {
         //get or create type cache
         Type t = obj.GetType();
         CachedType ct = _typeToInfo.GetOrAdd(t, () => new CachedType(t));

         Walk(ct, obj, callback, 0);
      }

      private void Walk(CachedType ct, object parent, WalkDelegate callback, int level)
      {
         foreach (Tuple<string, TypeNode> tpl in EnumerableEx.MultiIterate(ct.PropNames, ct.TypeNodes))
         {
            if (!callback(tpl.Item1, tpl.Item2, parent, level)) return;

            if (tpl.Item2.HasChildren)
            {
               Type cht = tpl.Item2.NodeType;
               CachedType chct = _typeToInfo.GetOrAdd(tpl.Item2.NodeType, () => new CachedType(cht));
               object cv = tpl.Item2.GetValue(parent);

               Walk(chct, cv, callback, level + 1);
            }
         }

      }
   }
}
