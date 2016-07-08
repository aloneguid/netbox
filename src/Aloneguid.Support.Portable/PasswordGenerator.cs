using System;

namespace Aloneguid.Support
{
   /// <summary>
   /// Generates random passwords
   /// </summary>
   public static class PasswordGenerator
   {
      // Define default min and max password lengths.
      private static int DefaultMinPasswordLength = 8;
      private static int DefaultMaxPasswordLength = 10;

      // Define supported password characters divided into groups.
      // You can add (or remove) characters to (from) these groups.
      private static string LowercastChars = "abcdefgijkmnopqrstwxyz";
      private static string UppercaseChars = "ABCDEFGHJKLMNPQRSTWXYZ";
      private static string NumericChars = "23456789";
      private static string SpecialChars = "*$-+?_&=!%{}/";

      /// <summary>
      /// Generates a random password.
      /// </summary>
      /// <returns>
      /// Randomly generated password.
      /// </returns>
      /// <remarks>
      /// The length of the generated password will be determined at
      /// random. It will be no shorter than the minimum default and
      /// no longer than maximum default.
      /// </remarks>
      public static string Generate()
      {
         return Generate(DefaultMinPasswordLength,
                         DefaultMaxPasswordLength);
      }

      /// <summary>
      /// Generates a random password of the exact length.
      /// </summary>
      /// <param name="length">
      /// Exact password length.
      /// </param>
      /// <returns>
      /// Randomly generated password.
      /// </returns>
      public static string Generate(int length)
      {
         return Generate(length, length);
      }

      /// <summary>
      /// Generates a random password.
      /// </summary>
      /// <param name="minLength">
      /// Minimum password length.
      /// </param>
      /// <param name="maxLength">
      /// Maximum password length.
      /// </param>
      /// <returns>
      /// Randomly generated password.
      /// </returns>
      /// <remarks>
      /// The length of the generated password will be determined at
      /// random and it will fall with the range determined by the
      /// function parameters.
      /// </remarks>
      public static string Generate(int minLength,
                                    int maxLength)
      {
         if (minLength <= 0) throw new ArgumentException("must be positive and greater than 0", nameof(minLength));
         if (maxLength <= 0) throw new ArgumentException("must be positive and greater than 0", nameof(maxLength));
         if (maxLength < minLength) throw new ArgumentException("minimum length must be equal or greater than maximum", nameof(minLength));

         // Create a local array containing supported password characters
         // grouped by types. You can remove character groups from this
         // array, but doing so will weaken the password strength.
         char[][] charGroups = new char[][]
         {
            LowercastChars.ToCharArray(),
            UppercaseChars.ToCharArray(),
            NumericChars.ToCharArray(),
            SpecialChars.ToCharArray()
         };

         // Use this array to track the number of unused characters in each
         // character group.
         int[] charsLeftInGroup = new int[charGroups.Length];

         // Initially, all characters in each group are not used.
         for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

         // Use this array to track (iterate through) unused character groups.
         int[] leftGroupsOrder = new int[charGroups.Length];

         // Initially, all character groups are not used.
         for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;

         // This array will hold password characters.
         char[] password = null;

         // Allocate appropriate memory for the password.
         if (minLength < maxLength)
            password = new char[Generator.GetRandomInt(minLength, maxLength + 1)];
         else
            password = new char[minLength];

         // Index of the next character to be added to password.
         int nextCharIdx;

         // Index of the next character group to be processed.
         int nextGroupIdx;

         // Index which will be used to track not processed character groups.
         int nextLeftGroupsOrderIdx;

         // Index of the last non-processed character in a group.
         int lastCharIdx;

         // Index of the last non-processed group.
         int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

         // Generate password characters one at a time.
         for (int i = 0; i < password.Length; i++)
         {
            // If only one character group remained unprocessed, process it;
            // otherwise, pick a random character group from the unprocessed
            // group list. To allow a special character to appear in the
            // first position, increment the second parameter of the Next
            // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
            if (lastLeftGroupsOrderIdx == 0)
               nextLeftGroupsOrderIdx = 0;
            else
               nextLeftGroupsOrderIdx = Generator.GetRandomInt(0,
                                                    lastLeftGroupsOrderIdx);

            // Get the actual index of the character group, from which we will
            // pick the next character.
            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            // Get the index of the last unprocessed characters in this group.
            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

            // If only one unprocessed character is left, pick it; otherwise,
            // get a random character from the unused character list.
            if (lastCharIdx == 0)
               nextCharIdx = 0;
            else
               nextCharIdx = Generator.GetRandomInt(0, lastCharIdx + 1);

            // Add this character to the password.
            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            // If we processed the last character in this group, start over.
            if (lastCharIdx == 0)
               charsLeftInGroup[nextGroupIdx] =
                                         charGroups[nextGroupIdx].Length;
            // There are more unprocessed characters left.
            else
            {
               // Swap processed character with the last unprocessed character
               // so that we don't pick it until we process all characters in
               // this group.
               if (lastCharIdx != nextCharIdx)
               {
                  char temp = charGroups[nextGroupIdx][lastCharIdx];
                  charGroups[nextGroupIdx][lastCharIdx] =
                              charGroups[nextGroupIdx][nextCharIdx];
                  charGroups[nextGroupIdx][nextCharIdx] = temp;
               }
               // Decrement the number of unprocessed characters in
               // this group.
               charsLeftInGroup[nextGroupIdx]--;
            }

            // If we processed the last group, start all over.
            if (lastLeftGroupsOrderIdx == 0)
               lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            // There are more unprocessed groups left.
            else
            {
               // Swap processed group with the last unprocessed group
               // so that we don't pick it until we process all groups.
               if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
               {
                  int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                  leftGroupsOrder[lastLeftGroupsOrderIdx] =
                              leftGroupsOrder[nextLeftGroupsOrderIdx];
                  leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
               }
               // Decrement the number of unprocessed groups.
               lastLeftGroupsOrderIdx--;
            }
         }

         // Convert password characters into a string and return the result.
         return new string(password);
      }
   }
}