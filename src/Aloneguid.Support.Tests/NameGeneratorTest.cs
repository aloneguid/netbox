using NUnit.Framework;

namespace Aloneguid.Support.Tests
{
   [TestFixture]
   public class NameGeneratorTest
   {
      [Test]
      public void DockerStyle_CallTwo_Different()
      {
         string n0 = NameGenerator.GenerateDockerStyleName();
         string n1 = NameGenerator.GenerateDockerStyleName();

         Assert.IsNotNull(n0);
         Assert.IsNotNull(n1);
         Assert.AreNotEqual(n0, n1);
      }

      [Test]
      public void PersonNames_Random_GeneratesSomething()
      {
         string randomName1 = NameGenerator.GeneratePersonFullName();
         string randomName2 = NameGenerator.GeneratePersonFullName();

         Assert.IsNotNull(randomName1);
         Assert.IsNotNull(randomName2);
         Assert.AreNotEqual(randomName1, randomName2);
      }
   }
}
