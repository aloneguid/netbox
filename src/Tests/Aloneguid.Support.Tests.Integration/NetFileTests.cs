using System.IO;
using System.Reflection;
using Aloneguid.Support.Extensions;
using Xunit;

namespace Aloneguid.Support.Tests.Integration
{
   
   public class NetFileTests : AbstractTestFixture
   {
      [Fact]
      public void GzipFile_Random_Unzips()
      {
         string rawFileName = Path.Combine(TestDir.FullName, "default.txt");
         using(Stream s = GetTestData("book.txt"))
         {
            using(Stream t = File.Create(rawFileName))
            {
               s.CopyTo(t);
            }
         }

         NetFile.Gzip(rawFileName, null);

         string compressedFileName = Path.Combine(TestDir.FullName, "default.txt.gz");
         Assert.True(File.Exists(compressedFileName));
      }

      protected Stream GetTestData(string fileName)
      {
         return Assembly.GetExecutingAssembly().GetManifestResourceStream(
            $"{typeof(NetFileTests).Namespace}.TestData.{fileName}");
      }
   }
}
