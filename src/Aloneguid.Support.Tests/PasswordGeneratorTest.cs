using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Aloneguid.Support.Tests
{
   [TestFixture]
   public class PasswordGeneratorTest
   {
      [Test]
      public void GeneratePassword_Default_Differ()
      {
         string password1 = PasswordGenerator.Generate();
         string password2 = PasswordGenerator.Generate();

         Assert.AreNotEqual(password1, password2);
      }

      [Test]
      public void GeneratePassword_MaxLength_OutOfMemory()
      {
         Assert.Throws<OutOfMemoryException>(() => PasswordGenerator.Generate(int.MaxValue));
      }

      [Test]
      public void GeneratePassword_EdgeCases_Passes()
      {
         Assert.Throws<ArgumentException>(() => PasswordGenerator.Generate(0, 0));
         Assert.Throws<ArgumentException>(() => PasswordGenerator.Generate(-1, 2));
         Assert.Throws<ArgumentException>(() => PasswordGenerator.Generate(3, 2));
      }
   }
}
