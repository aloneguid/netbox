using System;
using NetBox.Extensions;

namespace NetBox.Data
{
   /// <summary>
   /// Represents a primitive value which can be constructed from anything and converted to anything.
   /// "Anything" is to the best effort and will be extended constantly.
   /// </summary>
   public partial class DynamicValue
   {
      /// <summary>
      /// Constructs an instance of DynamicValue
      /// </summary>
      /// <param name="value"></param>
      public DynamicValue(object value)
      {
         OriginalValue = value;
      }

      /// <summary>
      /// Original value
      /// </summary>
      public object OriginalValue { get; private set; }

      /// <summary>
      /// Original type
      /// </summary>
      public Type OriginalType => OriginalValue?.GetType();

      /// <summary>
      /// Gets value of specified type
      /// </summary>
      /// <param name="t"></param>
      /// <returns></returns>
      public object GetValue(Type t)
      {
         if (OriginalValue == null) return null;
         if (OriginalType == t) return OriginalValue;

         object result = null;

         try
         {
            //result = Convert.ChangeType(OriginalValue, t, CultureInfo.InvariantCulture);
            result = Convert.ChangeType(OriginalValue, t);
         }
         catch(InvalidCastException)
         {

         }
         catch(FormatException)
         {
            return null;
         }

         if (result != null) return result;

         if(TryConvert(t, out object cr))
         {
            return cr;
         }

         throw new NotSupportedException($"conversion from {OriginalType} to  {t} is not supported");
      }

      /// <summary>
      /// Gets value of specified type
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public T GetValue<T>()
      {
         object r = GetValue(typeof(T));
         if (r == null) return default(T);
         return (T)r;
      }

      private bool TryConvert(Type t, out object result)
      {
         try
         {
            if (OriginalType == typeof(byte[]))
            {
               if (t == typeof(string))
               {
                  result = ((byte[])OriginalValue).ToHexString();
                  return true;
               }
            }
            else if (OriginalType == typeof(DateTimeOffset))
            {
               if (t == typeof(string))
               {
                  result = ((DateTimeOffset)OriginalValue).ToString();
                  return true;
               }
            }
            else if (OriginalType == typeof(string))
            {
               if (t == typeof(byte[]))
               {
                  result = ((string)OriginalValue).FromHexToBytes();
                  return true;
               }
               if (t == typeof(DateTimeOffset))
               {
                  result = DateTimeOffset.Parse((string)OriginalValue);
                  return true;
               }
            }
         }
         catch(FormatException)
         {
            result = null;
            return true;
         }
        
         result = null;
         return false;
      }
   }
}