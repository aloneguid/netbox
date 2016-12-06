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

      public void Walk(object obj, Func<string, TypeNode, object, bool> callback)
      {
         //get or create type cache
         Type t = obj.GetType();
         CachedType ct = _typeToInfo.GetOrAdd(t, () => new CachedType(t));

         foreach(var tpl in EnumerableEx.MultiIterate(ct.PropNames, ct.TypeNodes))
         {
            callback(tpl.Item1, tpl.Item2, obj);
         }
      }
   }
}
