using System;
using System.Collections.Generic;
using System.Text;
using NetBox.Collections;
using Xunit;

namespace NetBox.Tests.Collections
{
   public class CallbackListTest
   {
      [Fact]
      public void Adding_an_element_calls_back()
      {
         int i = 0;
         var lst = new CallbackList<int>();
         lst.OnAdd = (el) => { i++; return el; };

         lst.Add(5);

         Assert.Equal(1, i);
      }
   }
}
