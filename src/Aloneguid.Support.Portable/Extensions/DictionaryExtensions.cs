namespace System.Collections.Generic
{
   /// <summary>
   /// IDictionary extensions
   /// </summary>
   public static class DictionaryExtensions
   {
      /// <summary>
      /// Adds all elements from source to target
      /// </summary>
      public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
      {
         if (target == null || source == null) return;

         foreach(KeyValuePair<TKey, TValue> pair in source)
         {
            target.Add(pair);
         }
      }
   }
}
