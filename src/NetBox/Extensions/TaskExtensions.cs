﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace NetBox.Extensions
{
   /// <summary>
   /// Task utility methods
   /// </summary>
   public static class TaskExtensions
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
