namespace NetBox.Terminal.App.Validators
{
   class ValueRequiredValidator : ILinePrimitiveValidator
   {
      public bool IsValid(string primitiveName, string value, out string message)
      {
         if(string.IsNullOrEmpty(value))
         {
            message = "value is required";
            return false;
         }

         message = null;
         return true;
      }
   }
}
