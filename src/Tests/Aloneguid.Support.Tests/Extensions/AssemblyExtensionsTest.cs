using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Aloneguid.Support.Tests.Extensions
{
   /// <summary>
   /// These don't test much but at least verify calls don't crash
   /// </summary>
   [TestFixture]
   public class AssemblyExtensionsTest
   {
      [Test]
      public void GetFileVersion_ThisAssembly_ReturnsSomething()
      {
         Version v = Assembly.GetExecutingAssembly().FileVersion();

         Assert.AreEqual(v, new Version("2.0.0.0"));
      }

      [Test]
      public void GetProductVersion_ThisAssembly_ReturnsSomething()
      {
         Version v = Assembly.GetExecutingAssembly().ProductVersion();

         Assert.AreEqual(v, new Version("1.0.0.0"));
      }

      [Test]
      public void GetSameFolderEmbeddedResourceFileAsText_EmbeddedTextFile_TextMatches()
      {
         string content = Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsText<AssemblyExtensionsTest>("EmbeddedResource.txt");

         Assert.AreEqual("text file content\r\nwith two lines", content);
      }

      [Test]
      public void GetSameFolderEmbeddedResourceFileAsLines_EmbeddedTextFile_TextMatches()
      {
         string[] content = Assembly.GetExecutingAssembly().GetSameFolderEmbeddedResourceFileAsLines<AssemblyExtensionsTest>("EmbeddedResource.txt");

         Assert.AreEqual(2, content.Length);
         Assert.AreEqual("text file content", content[0]);
         Assert.AreEqual("with two lines", content[1]);
      }
   }
}

