using System;
using System.IO;
using System.Text;
#if !PORTABLE
using System.Collections.Generic;
using System.Reflection;
#endif

namespace Aloneguid.Support
{
   /// <summary>
   /// Generates random data
   /// </summary>
   public static class Generator
   {
      private static readonly Random Rnd = new Random(DateTime.UtcNow.Millisecond);

      /// <summary>
      /// Generates a random boolean
      /// </summary>
      public static bool RandomBool
      {
         get { return Rnd.Next(2) == 1; }
      }

      /// <summary>
      /// Generates a random long number between 0 and max
      /// </summary>
      public static long RandomLong
      {
         get { return GetRandomLong(0, long.MaxValue); }
      }

      /// <summary>
      /// Generates a random integer between 0 and max
      /// </summary>
      public static int RandomInt
      {
         get { return Rnd.Next(); }
      }

      /// <summary>
      /// Generates a random integer until max parameter
      /// </summary>
      /// <param name="max">Maximum integer value, excluding</param>
      /// <returns></returns>
      public static int GetRandomInt(int max)
      {
         return Rnd.Next(max);
      }

      /// <summary>
      /// Generates a random integer number in range
      /// </summary>
      /// <param name="min">Minimum value, including</param>
      /// <param name="max">Maximum value, excluding</param>
      public static int GetRandomInt(int min, int max)
      {
         return Rnd.Next(min, max);
      }

      /// <summary>
      /// Generates a random long number in range
      /// </summary>
      /// <param name="min">Minimum value, including</param>
      /// <param name="max">Maximum value, excluding</param>
      public static long GetRandomLong(long min, long max)
      {
         long randomLong = min + (long)(Rnd.NextDouble() * (max - min));
         return randomLong;
      }

      /// <summary>
      /// Generates a random enum value by type
      /// </summary>
      public static Enum RandomEnum(Type t)
      {
         Array values = Enum.GetValues(t);

         object value = values.GetValue(Rnd.Next(values.Length));

         return (Enum)value;
      }

#if !PORTABLE

      /// <summary>
      /// Generates a random enum value
      /// </summary>
      public static T RandomEnum<T>() where T : struct
      {
         //can't limit generics to enum http://connect.microsoft.com/VisualStudio/feedback/details/386194/allow-enum-as-generic-constraint-in-c

         if(!typeof(T).IsEnum) throw new ArgumentException("T must be an enum");

         return (T)(object)RandomEnum(typeof(T));
      }
#endif

      /// <summary>
      /// Generates a random date in range
      /// </summary>
      /// <param name="minValue">Minimum date, including</param>
      /// <param name="maxValue">Maximum date, excluding</param>
      public static DateTime GetRandomDate(DateTime minValue, DateTime maxValue)
      {
         long randomTicks = GetRandomLong(minValue.Ticks, maxValue.Ticks);

         return new DateTime(randomTicks);
      }

      /// <summary>
      /// Generates a random date value
      /// </summary>
      public static DateTime RandomDate
      {
         get { return GetRandomDate(DateTime.MinValue, DateTime.MaxValue); }
      }

      /// <summary>
      /// Generates a random string. Never returns null.
      /// </summary>
      public static string RandomString
      {
         get
         {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
         }
      }

      /// <summary>
      /// Generates a random string
      /// </summary>
      /// <param name="maxLength">Maximum string length</param>
      /// <param name="allowNulls">Whether to allow to return null values</param>
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

      /// <summary>
      /// Generates a random URL in format "http://random.com/random.random
      /// </summary>
      /// <param name="allowNulls">Whether to allow to return nulls</param>
      public static Uri GetRandomUri(bool allowNulls)
      {
         if(allowNulls && RandomLong % 2 == 0) return null;

         return new Uri(string.Format("http://{0}.com/{1}.{2}", RandomString, RandomString, GetRandomString(3, false)));
      }

      /// <summary>
      /// Generates a random URL in format "http://random.com/random.random. Never returns null values.
      /// </summary>
      public static Uri RandomUri
      {
         get { return GetRandomUri(false); }
      }

#if !PORTABLE

      /// <summary>
      /// Generates a class filling it in with random field values. Only settable properties will be populated, 
      /// and only for data types supported in this class with existing methods.
      /// </summary>
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
