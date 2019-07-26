using System;
using System.Collections.Generic;

namespace NetBox.Extensions
{
   /// <summary>
   /// IDictionary extensions
   /// </summary>
   public static class DictionaryExtensions
   {
      /// <summary>
      /// Adds all elements from source to target
      /// </summary>
      /// <typeparam name="TKey">Key type</typeparam>
      /// <typeparam name="TValue">Value type</typeparam>
      /// <param name="source">Source dictionary to get the values from</param>
      /// <param name="target">Target dictionary to add values to</param>
      public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
      {
         if (target == null || source == null) return;

         foreach(KeyValuePair<TKey, TValue> pair in source)
         {
            target.Add(pair);
         }
      }

      /// <summary>
      /// Merges all the keys into target dictionary, overwriting existing values
      /// </summary>
      /// <typeparam name="TKey">Key type</typeparam>
      /// <typeparam name="TValue">Value type</typeparam>
      /// <param name="source">Source dictionary to get the values from</param>
      /// <param name="target">Target dictionary to merge values to</param>
      public static void MergeRange<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
      {
         if (target == null || source == null) return;

         foreach(KeyValuePair<TKey, TValue> pair in source)
         {
            target[pair.Key] = pair.Value;
         }
      }

      /// <summary>
      /// Gets element by key if it exists in the dictionary, otherwise calls specifed method to
      /// create a new element and adds it back to the dictionary
      /// </summary>
      /// <typeparam name="TKey">Key type</typeparam>
      /// <typeparam name="TValue">Value type</typeparam>
      /// <param name="target">Target dictionary</param>
      /// <param name="key">Key to search on</param>
      /// <param name="createValue">Method used to create a new value</param>
      /// <returns></returns>
      public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> target,
         TKey key,
         Func<TValue> createValue)
      {
         if (target == null) throw new ArgumentNullException(nameof(target));
         if (createValue == null) throw new ArgumentNullException(nameof(createValue));

         TValue value;
         if(!target.TryGetValue(key, out value))
         {
            value = createValue();
            target[key] = value;
         }

         return value;
      }

      /// <summary>
      /// Serialises key-value dictionary using embedded JSON serialiser with zero dependencies.
      /// This is using source code from https://github.com/facebook-csharp-sdk/simple-json.
      /// </summary>
      /// <param name="dictionary">Dictionary to serialise</param>
      /// <returns>JSON string</returns>
      public static string JsonSerialise(this IDictionary<string, object> dictionary)
      {
         var sjo = new SimpleJson.JsonObject();
         foreach(KeyValuePair<string, object> item in dictionary)
         {
            sjo[item.Key] = item.Value;
         }

         return SimpleJson.SimpleJson.SerializeObject(sjo);
      }
   }
}
