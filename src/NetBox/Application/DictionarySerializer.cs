﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetBox.Application
{
   /// <summary>
   /// Performant (or to be in future) class serializer/deserializer to be used as a base
   /// for other serializers.
   /// </summary>
   public class DictionarySerializer
   {
      private static readonly Dictionary<Type, TypeInfo> _typeToInfo =
         new Dictionary<Type, TypeInfo>();

      public Dictionary<string, object> Serialize(object obj)
      {
         var result = new Dictionary<string, object>();

         TypeInfo ti = GetTypeIno(obj.GetType());

         return ti.ToDictionary(obj);
      }

      private static TypeInfo GetTypeIno(Type t)
      {
         TypeInfo ti;
         if(!_typeToInfo.TryGetValue(t, out ti))
         {
            ti = new TypeInfo(t);
            _typeToInfo[t] = ti;
         }

         return ti;
      }

      private class TypeInfo
      {
         private Dictionary<string, Func<object, object>> _propNameToGetter = 
            new Dictionary<string, Func<object, object>>();

         public TypeInfo(Type t)
         {
            //discover properties
            IEnumerable<PropertyInfo> properties = t.GetRuntimeProperties();
            foreach (PropertyInfo pi in properties)
            {
               string name = pi.Name;

               if(pi.GetMethod != null)
               {
                  _propNameToGetter[name] = _ => pi.GetMethod.Invoke(_, null);
               }
            }

            //discover fields
            IEnumerable<FieldInfo> fields = t.GetRuntimeFields();
            foreach(FieldInfo fi in fields)
            {
               string name = fi.Name;

               _propNameToGetter[name] = _ => fi.GetValue(_);
            }
         }

         public Dictionary<string, object> ToDictionary(object obj)
         {
            var result = new Dictionary<string, object>();

            foreach(var m in _propNameToGetter)
            {
               object value = m.Value(obj);
               result[m.Key] = value;
            }


            return result;
         }
      }
   }
}