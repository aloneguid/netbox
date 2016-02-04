﻿using System.IO;
using System.Reflection;

namespace Aloneguid.Support.Tests
{
   public class TestBase
   {
      protected Stream GetTestData(string fileName)
      {
         return Assembly.GetExecutingAssembly().GetManifestResourceStream(
            $"{typeof(TestBase).Namespace}.TestData.{fileName}");
      }
   }
}