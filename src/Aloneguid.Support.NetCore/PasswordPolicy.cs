using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aloneguid.Support
{
   /// <summary>
   /// Password policy used by <see cref="PasswordGenerator"/>
   /// </summary>
   public class PasswordPolicy
   {
      /// <summary>
      /// Constructs an intance
      /// </summary>
      /// <param name="minLength"></param>
      /// <param name="maxLength"></param>
      public PasswordPolicy(int minLength, int maxLength)
      {
         if (minLength <= 0) throw new ArgumentException("must be positive and greater than 0", nameof(minLength));
         if (maxLength <= 0) throw new ArgumentException("must be positive and greater than 0", nameof(maxLength));
         if (maxLength < minLength) throw new ArgumentException("minimum length must be equal or greater than maximum", nameof(minLength));

         MinLength = minLength;
         MaxLength = maxLength;

         LowercaseCharacters = "abcdefgijkmnopqrstwxyz";
         UppercaseCharacters = "ABCDEFGHJKLMNPQRSTWXYZ";
         NumericCharacters = "0123456789";
         SpecialCharacters = "#$%^&*-_!+=[]{}|\\:‘,.?/`~“();";
      }

      public int MinLength { get; }

      public int MaxLength { get; }

      public string LowercaseCharacters { get; set; }

      public string UppercaseCharacters { get; set; }

      public string NumericCharacters { get; set; }

      public string SpecialCharacters { get; set; }

      internal char[][] ToCharGroups()
      {
         var result = new List<char[]>();
         if (LowercaseCharacters != null && LowercaseCharacters.Length != 0) result.Add(LowercaseCharacters.ToCharArray());
         if (UppercaseCharacters != null && UppercaseCharacters.Length != 0) result.Add(UppercaseCharacters.ToCharArray());
         if (NumericCharacters != null && NumericCharacters.Length != 0) result.Add(NumericCharacters.ToCharArray());
         if (SpecialCharacters != null && SpecialCharacters.Length != 0) result.Add(SpecialCharacters.ToCharArray());

         if (result.Count == 0) throw new ArgumentException("no character groups defined");

         return result.ToArray();
      }
   }
}
