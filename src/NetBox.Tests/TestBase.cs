using System.IO;
using System.Reflection;

namespace NetBox.Tests
{
   public class TestBase
   {
#if NETFULL
      protected Stream GetTestData(string fileName)
      {
         return Assembly.GetExecutingAssembly().GetManifestResourceStream(
            $"{typeof(TestBase).Namespace}.TestData.{fileName}");
      }
#endif
   }
}
