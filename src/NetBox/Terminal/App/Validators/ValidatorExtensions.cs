using NetBox.Terminal.App.Validators;

namespace NetBox.Terminal.App
{
   public static class ValidatorExtensions
   {
      public static LinePrimitive<T> Required<T>(this LinePrimitive<T> arg)
      {
         return arg.AddValidator(new ValueRequiredValidator());
      }

      public static LinePrimitive<T> FileExists<T>(this LinePrimitive<T> arg)
      {
         return arg.AddValidator(new FileExistsValidator());
      }
   }
}
