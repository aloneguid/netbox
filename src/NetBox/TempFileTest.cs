using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace NetBox
{
   public class TempFileTest
   {
      [Fact]
      public void NotInUse()
      {
         using(var tf = new TempFile())
         {

         }
      }

      [Fact]
      public void InUse()
      {
         string path;

         using(var tf = new TempFile())
         {
            path = tf;
            File.WriteAllText(tf, "test");
         }

         Assert.False(File.Exists(path));
      }
   }
}
