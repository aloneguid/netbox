using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBox.Application
{
   class TypeInferring
   {
      private delegate bool ParserDelegate<T>(string input, out T result);


      class Tag<T> : Tag
      {
         private readonly ParserDelegate<T> _parser;

         public Tag(ParserDelegate<T> parser) : base(typeof(T))
         {
            _parser = parser;
         }

         public override bool GetValue(string s, out object value)
         {
            if(_parser(s, out T typedValue))
            {
               value = typedValue;
               return true;
            }

            value = null;
            return false;
         }
      }

      abstract class Tag
      {
         public Tag(Type t)
         {
            Type = t;
         }

         public Type Type { get; }

         public abstract bool GetValue(string s, out object value);
      }

      

      private static List<Tag> SupportedTypes =
         new List<Tag>
         {
            new Tag<bool>(bool.TryParse),
            new Tag<byte>(byte.TryParse),
            new Tag<short>(short.TryParse),
            new Tag<int>(int.TryParse),
            new Tag<long>(long.TryParse),
            new Tag<float>(float.TryParse),
            new Tag<double>(double.TryParse),
            new Tag<decimal>(decimal.TryParse),
            new Tag<char>(char.TryParse),
            new Tag<DateTimeOffset>(DateTimeOffset.TryParse)
         };

      public static Type InferBestType(IEnumerable<string> data, bool createValues, out IList values)
      {
         int totalCount = 0;
         int nullOrEmptyCount = 0;
         values = createValues ? new List<object>() : null;

         int[] counts = new int[SupportedTypes.Count];

         foreach(string v in data)
         {
            if (string.IsNullOrEmpty(v))
               nullOrEmptyCount++;

            for (int i = 0; i < SupportedTypes.Count; i++)
            {
               if (SupportedTypes[i].GetValue(v, out object tv))
                  counts[i]++;
            }

            totalCount++;
         }

         int idx = -1;
         for(int i = 0; i < counts.Length; i++)
         {
            if(counts[i] + nullOrEmptyCount == totalCount)
            {
               idx = i;
               break;
            }
         }

         Type resultType = idx == -1 ? typeof(string) : SupportedTypes[idx].Type;

         if(createValues)
         {
            if(idx == -1)
            {
               values.AddRange(data);
            }
            else
            {
               Tag t = SupportedTypes[idx];

               foreach(string v in data)
               {
                  if (string.IsNullOrEmpty(v))
                  {
                     values.Add(null);
                  }
                  else
                  {
                     t.GetValue(v, out object rv);
                     values.Add(rv);
                  }
               }
            }
         }

         return resultType;
      }
   }
}
