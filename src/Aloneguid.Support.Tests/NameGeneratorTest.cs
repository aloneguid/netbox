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

         Assert.AreNotEqual(n0, n1);
      }
   }
}
