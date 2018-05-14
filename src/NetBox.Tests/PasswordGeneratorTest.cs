using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using NetBox;
using NetBox.Generator;

namespace NetBox.Tests
{
   
   public class PasswordGeneratorTest
   {
      private PasswordPolicy _policy;

      public PasswordGeneratorTest()
      {
         _policy = new PasswordPolicy(8, 12);
      }

      [Fact]
      public void GeneratePassword_Default_Differ()
      {
         string password1 = PasswordGenerator.Generate(_policy);
         string password2 = PasswordGenerator.Generate(_policy);

         Assert.NotEqual(password1, password2);
      }

      [Fact]
      public void GeneratePassword_MaxLength_OutOfMemory()
      {
         var policy = new PasswordPolicy(int.MaxValue, int.MaxValue);
         Assert.Throws<OutOfMemoryException>(() => PasswordGenerator.Generate(policy));
      }

      [Fact]
      public void GeneratePassword_EdgeCases_Passes()
      {
         Assert.Throws<ArgumentException>(() =>
         {
            _policy = new PasswordPolicy(0, 0);
            PasswordGenerator.Generate(_policy);
         });

         Assert.Throws<ArgumentException>(() =>
         {
            _policy = new PasswordPolicy(-1, 2);
            PasswordGenerator.Generate(_policy);
         });

         Assert.Throws<ArgumentException>(() =>
         {
            _policy = new PasswordPolicy(3, 2);
            PasswordGenerator.Generate(_policy);
         });
      }

      [Fact]
      public void GeneratePassword_NoSpecialChars_Checked()
      {
         _policy = new PasswordPolicy(8, 8) { SpecialCharacters = null };

         string password = PasswordGenerator.Generate(_policy);

         char[] special = new PasswordPolicy(1, 2).SpecialCharacters.ToCharArray();

         foreach(char ch in password)
         {
            Assert.DoesNotContain(ch, special);
         }
      }
   }
}
