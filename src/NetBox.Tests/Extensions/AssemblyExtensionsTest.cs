#if NETFULL
using Xunit;
using System;
using System.Diagnostics;
using System.Reflection;

namespace NetBox.Tests.Extensions
{
   /// <summary>
   /// These don't test much but at least verify calls don't crash
   /// </summary>
   
   public class AssemblyExtensionsTest
   {
      [Fact]
      public void GetFileVersion_ThisAssembly_ReturnsSomething()
      {
         Version v = Assembly.GetExecutingAssembly().FileVersion();

         Assert.Equal(v, new Version("2.0.0.0"));
      }

      [Fact]
      public void GetProductVersion_ThisAssembly_ReturnsSomething()
      {
         Version v = Assembly.GetExecutingAssembly().ProductVersion();

         Assert.Equal(v, new Version("1.0.0.0"));
      }

      [Fact]
      public void GetSameFolderEmbeddedResourceFileAsText_EmbeddedTextFile_TextMatches()
      {
         string content = Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsText<AssemblyExtensionsTest>("EmbeddedResource.txt");

         Assert.Equal("text file content\r\nwith two lines", content);
      }

      [Fact]
      public void GetSameFolderEmbeddedResourceFileAsLines_EmbeddedTextFile_TextMatches()
      {
         string[] content = Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsLines<AssemblyExtensionsTest>("EmbeddedResource.txt");

         Assert.Equal(2, content.Length);
         Assert.Equal("text file content", content[0]);
         Assert.Equal("with two lines", content[1]);
      }
   }
}
#endif
