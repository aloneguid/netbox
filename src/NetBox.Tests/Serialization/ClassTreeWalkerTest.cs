using System;
using System.Collections.Generic;
using NetBox.Serialization;
using Xunit;

namespace NetBox.Tests.Serialization
{
   public class ClassTreeWalkerTest
   {
      [Fact]
      public void Simple_Walk_AllWalked()
      {
         var walker = new ClassTreeWalker();
         var obj = new OneLevelPropertyAndMemberClass("v1", "v2");
         var d = new Dictionary<string, object>();

         walker.Walk(obj, (name, node, parent) =>
         {
            object value = node.GetValue(parent);
            d[name] = value;

            return true;
         });

         Assert.Equal(2, d.Count);
      }

      [Fact]
      public void Temp_Comparison_Yeah()
      {
         Type t = typeof(int);
         string ts = t.FullName;
         const int count = 10000;

         long time1, time2;

         using (var m = new Measure())
         {
            for (int i = 0; i < count; i++)
            {
               bool equal = (t == typeof(long));
            }

            time1 = m.ElapsedTicks;
         }

         using (var m = new Measure())
         {
            for(int i = 0; i < count; i++)
            {
               bool equal = (ts == "System.Int64");
            }

            time2 = m.ElapsedTicks;
         }

         Assert.NotEqual(time1, time2);
      }
   }

   #region [ Sample Data Structure ]

   public class OneLevelPropertyAndMemberClass
   {
      public OneLevelPropertyAndMemberClass(string s1, string s2)
      {
         S1 = s1;
         S2 = s2;
      }

      public string S1 { get; set; }

      public string S2;
   }

   #endregion
}
