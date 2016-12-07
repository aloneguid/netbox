﻿using System;
using System.Collections.Generic;
using NetBox.Serialization;
using Xunit;

namespace NetBox.Tests.Serialization
{
   public class ClassTreeWalkerTest
   {
      private ClassTreeWalker _walker = new ClassTreeWalker();

      [Fact]
      public void Simple_Walk_AllWalked()
      {
         var obj = new OneLevelPropertyAndMemberClass("v1", "v2");
         var d = new Dictionary<string, object>();

         _walker.Walk(obj, (name, node, parent, level) =>
         {
            object value = node.GetValue(parent);
            d[name] = value;

            return true;
         });

         Assert.Equal(2, d.Count);
      }

      [Fact]
      public void MultiLevel_Walk_AllWalked()
      {
         var obj = new TwoLevelClass
         {
            S01 = "m1",
            Member = new OneLevelPropertyAndMemberClass("mm1", "mm2")
         };
         var result = new Dictionary<string, object>();

         _walker.Walk(obj, (name, node, parent, level) =>
         {
            if (!node.HasChildren)
            {
               result[name] = node.GetValue(parent);
            }

            return true;
         });

         Assert.Equal(3, result.Count);
         Assert.True(result["S01"].ToString() == "m1");
         Assert.True(result["S1"].ToString() == "mm1");
         Assert.True(result["S2"].ToString() == "mm2");
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

   public class TwoLevelClass
   {
      public string S01 { get; set; }

      public OneLevelPropertyAndMemberClass Member { get; set; }
   }

   #endregion
}