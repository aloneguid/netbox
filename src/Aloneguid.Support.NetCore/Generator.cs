using System;
using System.IO;
using System.Text;
#if !PORTABLE
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
#endif

namespace Aloneguid.Support
{
#if PORTABLE
   /// <summary>
   /// Generates random data using System.Random class
   /// </summary>
#else
   /// <summary>
   /// Generates random data using <see cref="RandomNumberGenerator"/> for increased security
   /// </summary>
#endif
   public static class Generator
   {
#if PORTABLE
      private static readonly Random Rnd = new Random(DateTime.UtcNow.Millisecond);
#else
      private static readonly RandomNumberGenerator Rnd = RandomNumberGenerator.Create();

      private static double NextCryptoDouble()
      {
         byte[] b = new byte[4];
         Rnd.GetBytes(b);
         return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
      }

      private static int NextCryptoInt()
      {
         byte[] b = new byte[sizeof(int)];
         Rnd.GetBytes(b);
         return BitConverter.ToInt32(b, 0);
      }

      /*private static long NextCryptoLong()
      {
         byte[] b = new byte[sizeof(long)];
         Rnd.GetBytes(b);
         return BitConverter.ToInt64(b, 0);
      }*/

#endif

      /// <summary>
      /// Generates a random boolean
      /// </summary>
      public static bool RandomBool
      {
         get
         {
#if PORTABLE
            return Rnd.Next(2) == 1;
#else
            return NextCryptoDouble() >= 0.5d;
#endif
         }
      }

      /// <summary>
      /// Generates a random long number between 0 and max
      /// </summary>
      public static long RandomLong => GetRandomLong(0, long.MaxValue);

      /// <summary>
      /// Generates a random integer between 0 and max
      /// </summary>
      public static int RandomInt
      {
         get
         {
#if PORTABLE
            return Rnd.Next();
#else
            return NextCryptoInt();
#endif
         }
      }

      /// <summary>
      /// Returns random double
      /// </summary>
      public static double RandomDouble
      {
         get
         {
#if PORTABLE
            return Rnd.NextDouble();
#else
            return NextCryptoDouble();
#endif
         }
      }

      /// <summary>
      /// Generates a random integer until max parameter
      /// </summary>
      /// <param name="max">Maximum integer value, excluding</param>
      /// <returns></returns>
      public static int GetRandomInt(int max)
      {
         return GetRandomInt(0, max);
      }

      /// <summary>
      /// Generates a random integer number in range
      /// </summary>
      /// <param name="min">Minimum value, including</param>
      /// <param name="max">Maximum value, excluding</param>
      public static int GetRandomInt(int min, int max)
      {
#if PORTABLE
         return Rnd.Next(min, max);
#else
         return (int)Math.Round(NextCryptoDouble() * (max - min - 1)) + min;
#endif
      }

      /// <summary>
      /// Generates a random long number in range
      /// </summary>
      /// <param name="min">Minimum value, including</param>
      /// <param name="max">Maximum value, excluding</param>
      public static long GetRandomLong(long min, long max)
      {
#if PORTABLE
         return min + (long)(Rnd.NextDouble() * (max - min));
#else
         return (long)Math.Round(NextCryptoDouble() * (max - min - 1)) + min;
#endif
      }

      /// <summary>
      /// Generates a random enum value by type
      /// </summary>
      public static Enum RandomEnum(Type t)
      {
         Array values = Enum.GetValues(t);

         object value = values.GetValue(GetRandomInt(values.Length));

         return (Enum)value;
      }

#if NETFULL

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
      /// <param name="length">string length</param>
      /// <param name="allowNulls">Whether to allow to return null values</param>
      public static string GetRandomString(int length, bool allowNulls)
      {
         if(allowNulls && RandomLong % 2 == 0) return null;

         var builder = new StringBuilder();
         char ch;
         for(int i = 0; i < length; i++)
         {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * RandomDouble + 65)));
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

         return new Uri($"http://{RandomString}.com/{RandomString}.{GetRandomString(3, false)}");
      }

      /// <summary>
      /// Generates a random URL in format "http://random.com/random.random. Never returns null values.
      /// </summary>
      public static Uri RandomUri
      {
         get { return GetRandomUri(false); }
      }

      /// <summary>
      /// Generates a random sequence of bytes of a specified size
      /// </summary>
      public static byte[] GetRandomBytes(int minSize, int maxSize)
      {
         int size = minSize == maxSize ? minSize : GetRandomInt(minSize, maxSize);
         byte[] data = new byte[size];
#if PORTABLE
         for(int i = 0; i < data.Length; i++)
         {
            data[i] = (byte)GetRandomInt(byte.MaxValue);
         }
#else
         Rnd.GetBytes(data);
#endif
         return data;
      }

   }
}
