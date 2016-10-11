using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Aloneguid.Support;

namespace Aloneguid.Support.Tests
{
   [TestFixture]
   public class PasswordGeneratorTest
   {
      private PasswordPolicy _policy;

      [SetUp]
      public void SetUp()
      {
         _policy = new PasswordPolicy(8, 12);
      }

      [Test]
      public void GeneratePassword_Default_Differ()
      {
         string password1 = PasswordGenerator.Generate(_policy);
         string password2 = PasswordGenerator.Generate(_policy);

         Assert.AreNotEqual(password1, password2);
      }

      [Test]
      public void GeneratePassword_MaxLength_OutOfMemory()
      {
         var policy = new PasswordPolicy(int.MaxValue, int.MaxValue);
         Assert.Throws<OutOfMemoryException>(() => PasswordGenerator.Generate(policy));
      }

      [Test]
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

      [Test]
      public void GeneratePassword_NoSpecialChars_Checked()
      {
         _policy = new PasswordPolicy(8, 8) { SpecialCharacters = null };

         string password = PasswordGenerator.Generate(_policy);

         char[] special = new PasswordPolicy(1, 2).SpecialCharacters.ToCharArray();

         foreach(char ch in password)
         {
            Assert.IsFalse(special.Contains(ch));
         }
      }
   }
}
