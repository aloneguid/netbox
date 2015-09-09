using System.IO;
using NUnit.Framework;

namespace Aloneguid.Support.Tests
{
   [TestFixture]
   public class NetPathTest
   {
      [Test]
      public void ExecDir_SmokeTest_DoesntCrash()
      {
         string path = NetPath.ExecDir;
         DirectoryInfo info = NetPath.ExecDirInfo;
      }
   }
}
