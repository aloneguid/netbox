using System;
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

      private static readonly JsonSerializerSettings CompressedSettings = new JsonSerializerSettings
      {
         Formatting = Formatting.None,
         NullValueHandling = NullValueHandling.Ignore,
         DefaultValueHandling = DefaultValueHandling.Ignore
      };

      public string Serialise(object obj, bool compressed)
      {
         if(obj == null) return null;

         return JsonConvert.SerializeObject(obj, compressed ? CompressedSettings : Settings);
      }

      public T Deserialise<T>(string s)
      {
         if(string.IsNullOrEmpty(s)) return default(T);

         try
         {
            return JsonConvert.DeserializeObject<T>(s, Settings);
         }
         catch(JsonSerializationException)
         {
            return default(T);
         }
         catch(JsonReaderException)
         {
            return default(T);
         }
      }

      public object Deserialise(string s, Type t)
      {
         if(string.IsNullOrEmpty(s)) return null;

         try
         {
            return JsonConvert.DeserializeObject(s, t, Settings);
         }
         catch(JsonReaderException)
         {
            return null;
         }
      }
   }
}