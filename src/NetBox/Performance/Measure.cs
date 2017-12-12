using System;
using System.Diagnostics;

namespace NetBox.Performance
{
   /// <summary>
   /// Measures a time slice as precisely as possible
   /// </summary>
   public class Measure : IDisposable
   {
      private readonly Stopwatch _sw = new Stopwatch();

      /// <summary>
      /// Creates the measure object
      /// </summary>
      public Measure()
      {
         _sw.Start();
      }

      /// <summary>
      /// Returns number of elapsed ticks since the start of measure.
      /// The measuring process will continue running.
      /// </summary>
      public long ElapsedTicks => _sw.ElapsedTicks;

      /// <summary>
      /// Returns number of elapsed milliseconds since the start of measure.
      /// The measuring process will continue running.
      /// </summary>
      public long ElapsedMilliseconds => _sw.ElapsedMilliseconds;

      /// <summary>
      /// Stops measure object if still running
      /// </summary>
      public void Dispose()
      {
         if (_sw.IsRunning)
         {
            _sw.Stop();
         }
      }
   }
}
