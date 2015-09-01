using System;
using System.Text;
using Newtonsoft.Json;

namespace Aloneguid.Support.Application
{
   class JsonSerialiser
   {
      private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
      {
         Formatting = Formatting.Indented,
         NullValueHandling = NullValueHandling.Ignore,
         DefaultValueHandling = DefaultValueHandling.Ignore
      };

      public string Serialise(object obj)
      {
         if(obj == null) return null;

         return JsonConvert.SerializeObject(obj, Settings);
      }

      public T Deserialise<T>(string s)
      {
         if(string.IsNullOrEmpty(s)) return default(T);

         return JsonConvert.DeserializeObject<T>(s, Settings);
      }

      public object Deserialise(string s, Type t)
      {
         if(string.IsNullOrEmpty(s)) return null;

         return JsonConvert.DeserializeObject(s, t, Settings);
      }
   }
}
