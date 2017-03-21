using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace NetBox.Application
{
   class JsonSerialiser
   {
      private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
      {
         Formatting = Formatting.Indented,
         NullValueHandling = NullValueHandling.Ignore,
         DefaultValueHandling = DefaultValueHandling.Ignore
      };

      private static readonly JsonSerializerSettings SettingsEnums = new JsonSerializerSettings
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

      private static readonly JsonSerializerSettings CompressedSettingsEnums = new JsonSerializerSettings
      {
         Formatting = Formatting.None,
         NullValueHandling = NullValueHandling.Ignore,
         DefaultValueHandling = DefaultValueHandling.Ignore
      };

      private static readonly Dictionary<Tuple<bool, bool>, JsonSerializerSettings> OptionsToSettings =
         new Dictionary<Tuple<bool, bool>, JsonSerializerSettings>
         {
            {  Tuple.Create(false, false), Settings },
            { Tuple.Create(false, true), SettingsEnums },
            { Tuple.Create(true, false), CompressedSettings},
            { Tuple.Create(true, true), CompressedSettingsEnums }
         };

      static JsonSerialiser()
      {
         SettingsEnums.Converters.Add(new StringEnumConverter());
         CompressedSettingsEnums.Converters.Add(new StringEnumConverter());
      }

      public string Serialise(object obj, bool compressed, bool enumsAsStrings)
      {
         if(obj == null) return null;

         JsonSerializerSettings settings = OptionsToSettings[Tuple.Create(compressed, enumsAsStrings)];

         return JsonConvert.SerializeObject(obj, settings);
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