﻿using System.Threading.Tasks;

namespace System
{
   using System.Diagnostics.CodeAnalysis;

   /// <summary>
   /// Task utility methods
   /// </summary>
   static class TaskExtensions
   {
      /// <summary>
      /// Fire-and-forget without compiler warnings
      /// </summary>
      /// <param name="task"></param>
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "task")]
      public static void Forget(this Task task)
      {
      }
   }
}
