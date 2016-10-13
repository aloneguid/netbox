using Xunit;

namespace NetBox.Tests
{
   
   public class NameGeneratorTest
   {
      [Fact]
      public void DockerStyle_CallTwo_Different()
      {
         string n0 = NameGenerator.GenerateDockerStyleName();
         string n1 = NameGenerator.GenerateDockerStyleName();

         Assert.NotNull(n0);
         Assert.NotNull(n1);
         Assert.NotEqual(n0, n1);
      }

      [Fact]
      public void PersonNames_Random_GeneratesSomething()
      {
         string randomName1 = NameGenerator.GeneratePersonFullName();
         string randomName2 = NameGenerator.GeneratePersonFullName();

         Assert.NotNull(randomName1);
         Assert.NotNull(randomName2);
         Assert.NotEqual(randomName1, randomName2);
      }
   }
}
