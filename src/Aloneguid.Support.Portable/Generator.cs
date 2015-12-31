using System;
using System.IO;
using System.Text;
#if !PORTABLE
using System.Collections.Generic;
using System.Reflection;
#endif

namespace Aloneguid.Support
{
   public static class Generator
   {
      private static readonly Random Rnd = new Random(DateTime.UtcNow.Millisecond);

      public static bool RandomBool
      {
         get { return Rnd.Next(2) == 1; }
      }

      public static long RandomLong
      {
         get { return GetRandomLong(0, long.MaxValue); }
      }

      public static int RandomInt
      {
         get { return Rnd.Next(); }
      }

      public static int GetRandomInt(int max)
      {
         return Rnd.Next(max);
      }

      public static int GetRandomInt(int min, int max)
      {
         return Rnd.Next(min, max);
      }

      public static long GetRandomLong(long min, long max)
      {
         long randomLong = min + (long)(Rnd.NextDouble() * (max - min));
         return randomLong;
      }

      public static Enum RandomEnum(Type t)
      {
         Array values = Enum.GetValues(t);

         object value = values.GetValue(Rnd.Next(values.Length));

         return (Enum)value;
      }

#if !PORTABLE
      public static T RandomEnum<T>() where T : struct
      {
         //can't limit generics to enum http://connect.microsoft.com/VisualStudio/feedback/details/386194/allow-enum-as-generic-constraint-in-c

         if(!typeof(T).IsEnum) throw new ArgumentException("T must be an enum");

         return (T)(object)RandomEnum(typeof(T));
      }
#endif

      public static DateTime GetRandomDate(DateTime minValue, DateTime maxValue)
      {
         long randomTicks = GetRandomLong(minValue.Ticks, maxValue.Ticks);

         return new DateTime(randomTicks);
      }

      public static DateTime RandomDate
      {
         get { return GetRandomDate(DateTime.MinValue, DateTime.MaxValue); }
      }

      public static string RandomString
      {
         get
         {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
         }
      }

      public static string GetRandomString(int maxLength, bool allowNulls)
      {
         if(allowNulls && RandomLong % 2 == 0) return null;

         var builder = new StringBuilder();
         char ch;
         for(int i = 0; i < maxLength; i++)
         {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Rnd.NextDouble() + 65)));
            builder.Append(ch);
         }

         return builder.ToString();
      }

      public static Uri GetRandomUri(bool allowNulls)
      {
         if(allowNulls && RandomLong % 2 == 0) return null;

         return new Uri(string.Format("http://{0}.com/{1}.{2}", RandomString, RandomString, GetRandomString(3, false)));
      }

      public static Uri RandomUri
      {
         get { return GetRandomUri(false); }
      }

#if !PORTABLE

      public static T RandomClass<T>() where T : class, new()
      {
         T result = new T();

         foreach(PropertyInfo property in result.GetType().GetProperties())
         {
            if(property.CanWrite)
            {
               Type t = property.PropertyType;
               object setValue;

               if(t == typeof(string)) setValue = RandomString;
               else if(t == typeof(long)) setValue = RandomLong;
               else if(t == typeof(int)) setValue = RandomInt;
               else if(t == typeof(bool)) setValue = RandomBool;
               else if(t == typeof(Guid)) setValue = Guid.NewGuid();
               else if(t == typeof(DateTime)) setValue = RandomDate;
               else if(t.IsEnum) setValue = RandomEnum(t);
               else
               {
                  throw new InvalidOperationException(string.Format("type {0} cannot be generated on property {1}", t,
                     property.Name));
               }

               property.SetValue(result, setValue, null);
            }
         }

         return result;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="count"></param>
      /// <param name="allowNulls">When set to true some of the returned elements may be null</param>
      /// <returns></returns>
      public static IEnumerable<T> RandomClassCollection<T>(int count, bool allowNulls) where T : class, new()
      {
         var result = new List<T>();
         for(int i = 0; i < count; i++)
         {
            bool isNull;
            if(!allowNulls) isNull = false;
            else isNull = RandomLong % 2 == 0;

            if(isNull)
            {
               result.Add(null);
            }
            else
            {
               result.Add(RandomClass<T>());
            }
         }
         return result;
      }

#endif

   }
}
