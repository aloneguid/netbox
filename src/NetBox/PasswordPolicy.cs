using System;
using System.Collections.Generic;

namespace NetBox
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

      /// <summary>
      /// Minimum length of the password
      /// </summary>
      public int MinLength { get; }

      /// <summary>
      /// Maximum length of the password
      /// </summary>
      public int MaxLength { get; }

      /// <summary>
      /// List of allowed lowercase characters
      /// </summary>
      public string LowercaseCharacters { get; set; }

      /// <summary>
      /// List of allowed uppercase characters
      /// </summary>
      public string UppercaseCharacters { get; set; }

      /// <summary>
      /// List of allowed numeric characters
      /// </summary>
      public string NumericCharacters { get; set; }

      /// <summary>
      /// List of allowed special characters
      /// </summary>
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
