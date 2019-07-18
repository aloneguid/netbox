using System;
using System.Collections.Generic;
using static NetBox.Terminal.PoshConsole;

namespace NetBox.Terminal.App
{
   public abstract class LinePrimitive
   {
      private readonly List<ILinePrimitiveValidator> _validators = new List<ILinePrimitiveValidator>();

      public LinePrimitive(string name, string description)
      {
         Name = name ?? throw new ArgumentNullException(nameof(name));
         Description = description;
      }

      /// <summary>
      /// Name
      /// </summary>
      public string Name { get; }

      /// <summary>
      /// Description (help)
      /// </summary>
      public string Description { get; }

      internal Command Command { get; set; }

      protected void Validate(string value)
      {
         foreach (ILinePrimitiveValidator validator in _validators)
         {
            if (!validator.IsValid(Name, value, out string message))
            {
               throw new ArgValidationException(message, Name);
            }
         }
      }

      /// <summary>
      /// Adds a validator that is executed before the command
      /// </summary>
      /// <param name="validator"></param>
      protected LinePrimitive AddValidator(ILinePrimitiveValidator validator)
      {
         _validators.Add(validator);

         return this;
      }

   }

   public class LinePrimitive<T> : LinePrimitive
   {
      private readonly bool _isOption;
      private readonly T _defaultValue;

      internal LinePrimitive(bool isOption, string name, string description, T defaultValue) : base(name, description)
      {
         if (!IsSupported(typeof(T)))
            throw new ArgumentException($"{typeof(T)} is not supported");

         _isOption = isOption;
         _defaultValue = defaultValue;
      }

      /// <summary>
      /// Adds a validator that is executed before the command
      /// </summary>
      /// <param name="validator"></param>
      public new LinePrimitive<T> AddValidator(ILinePrimitiveValidator validator)
      {
         base.AddValidator(validator);

         return this;
      }

      private static bool IsSupported(Type t)
      {
         return t == typeof(string) || t == typeof(int) || t == typeof(bool);
      }

      public T Value
      {
         get
         {
            string value = GetRawValue();

            Validate(value);

            if (typeof(T) == typeof(string))
            {
               if (value == null)
                  return _defaultValue;

               return (T)(object)value;
            }
            else if (typeof(T) == typeof(int))
            {
               if(string.IsNullOrEmpty(value))
               {
                  return _defaultValue;
               }

               if (!int.TryParse(value, out int iarg))
               {
                  throw new ArgValidationException("value is not an integer", Name);
               }
               else
               {
                  return (T)(object)iarg;
               }
            }
            else if(typeof(T) == typeof(bool))
            {
               return (T)(object)(value != null);
            }
            else
            {
               return _defaultValue;
            }
         }
      }

      public static implicit operator T(LinePrimitive<T> lp)
      {
         return lp.Value;
      }

      private string GetRawValue()
      {
         if(_isOption)
         {
            return Command.GetOption(this, typeof(T) == typeof(bool));
         }

         return Command.GetArgument(this);
      }
   }
   
}
