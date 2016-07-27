using System.IO;
using System.Reflection;
using Aloneguid.Support.Extensions;
using NUnit.Framework;

namespace Aloneguid.Support.Tests.Integration
{
   [TestFixture]
   public class NetFileTests : AbstractTestFixture
   {
      [Test]
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
         Assert.IsTrue(File.Exists(compressedFileName));
      }

      protected Stream GetTestData(string fileName)
      {
         return Assembly.GetExecutingAssembly().GetManifestResourceStream(
            $"{typeof(NetFileTests).Namespace}.TestData.{fileName}");
      }
   }
}
