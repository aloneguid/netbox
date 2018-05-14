using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBox.Extensions;
using Xunit;

namespace NetBox.Tests.IO
{
   public class DirectoryScannerTest
   {
      [Fact]
      public void List_all_dll_files_in_all_subfolders()
      {
         IReadOnlyCollection<string> files = NetPath.FindFiles(new DirectoryInfo(Environment.CurrentDirectory), "**/*.dll");

         Assert.True(files.Any());
      }

      [Fact]
      public void Pattern_than_never_matches()
      {
         Assert.Equal(0, NetPath.FindFiles(new DirectoryInfo(Environment.CurrentDirectory), Guid.NewGuid().ToString()).Count);
      }
   }
}