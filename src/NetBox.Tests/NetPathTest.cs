using System.IO;
using Xunit;

namespace NetBox.Tests
{
   
   public class NetPathTest
   {
      [Fact]
      public void ExecDir_SmokeTest_DoesntCrash()
      {
         string path = NetPath.ExecDir;
         DirectoryInfo info = NetPath.ExecDirInfo;
      }
   }
}