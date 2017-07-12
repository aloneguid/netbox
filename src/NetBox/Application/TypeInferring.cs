using System;
using System.Collections.Generic;
using System.Text;

namespace NetBox.Application
{
   class TypeInferring
   {
      public static Type InferBestType(IEnumerable<string> data)
      {
         int totalCount = 0;
         int nullOrEmptyCount = 0;

         int intCount = 0;
         int boolCount = 0;
         int byteCount = 0;
         int shortCount = 0;
         int longCount = 0;
         int charCount = 0;
         int floatCount = 0;
         int doubleCount = 0;
         int decimalCount = 0;
         int dateCount = 0;

         foreach(string v in data)
         {
            if (string.IsNullOrEmpty(v))
               nullOrEmptyCount++;

            if (int.TryParse(v, out int i))
               intCount++;

            if (bool.TryParse(v, out bool b))
               boolCount++;

            if (byte.TryParse(v, out byte bt))
               byteCount++;

            if (short.TryParse(v, out short sh))
               shortCount++;

            if (long.TryParse(v, out long l))
               longCount++;

            if (char.TryParse(v, out char ch))
               charCount++;

            if (float.TryParse(v, out float f))
               floatCount++;

            if (double.TryParse(v, out double dob))
               doubleCount++;

            if (decimal.TryParse(v, out decimal dec))
               decimalCount++;

            if (DateTimeOffset.TryParse(v, out DateTimeOffset dto))
               dateCount++;

            totalCount++;
         }

         if (boolCount + nullOrEmptyCount == totalCount)
            return typeof(bool);

         if (byteCount + nullOrEmptyCount == totalCount)
            return typeof(byte);

         if (shortCount + nullOrEmptyCount == totalCount)
            return typeof(short);

         if (intCount + nullOrEmptyCount == totalCount)
            return typeof(int);

         if (longCount + nullOrEmptyCount == totalCount)
            return typeof(long);

         if (charCount + nullOrEmptyCount == totalCount)
            return typeof(char);

         if (floatCount + nullOrEmptyCount == totalCount)
            return typeof(float);

         if (doubleCount + nullOrEmptyCount == totalCount)
            return typeof(double);

         if (decimalCount + nullOrEmptyCount == totalCount)
            return typeof(decimal);

         if (dateCount + nullOrEmptyCount == totalCount)
            return typeof(DateTimeOffset);

         return typeof(string);
      }
   }
}
