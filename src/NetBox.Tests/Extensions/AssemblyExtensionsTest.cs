using Xunit;
using System;
using System.Diagnostics;
using System.Reflection;
using NetBox.Extensions;

namespace NetBox.Tests.Extensions
{
   /// <summary>
   /// These don't test much but at least verify calls don't crash
   /// </summary>
   
   public class AssemblyExtensionsTest
   {
      private Assembly _asm = Assembly.Load(new AssemblyName("NetBox.Tests"));

      [Fact]
      public void GetSameFolderEmbeddedResourceFileAsText_EmbeddedTextFile_TextMatches()
      {
         string content = _asm.GetSameFolderEmbeddedResourceFileAsText<AssemblyExtensionsTest>("EmbeddedResource.txt");

         Assert.Equal("text file content\r\nwith two lines", content, false, true);
      }

      [Fact]
      public void GetSameFolderEmbeddedResourceFileAsLines_EmbeddedTextFile_TextMatches()
      {
         string[] content = _asm.GetSameFolderEmbeddedResourceFileAsLines<AssemblyExtensionsTest>("EmbeddedResource.txt");

         Assert.Equal(2, content.Length);
         Assert.Equal("text file content", content[0]);
         Assert.Equal("with two lines", content[1]);
      }
   }
}