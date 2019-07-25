using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBox.Async
{
   /// <summary>
   /// Limit the amount of async tasks that can run at once
   /// </summary>
   public class AsyncLimiter : IDisposable
   {
      private readonly SemaphoreSlim _throttler;

      /// <summary>
      /// Creates a guard block that limits number of tasks to run at once.
      /// </summary>
      /// <param name="maxTasks"></param>
      public AsyncLimiter(int maxTasks)
      {
         if (maxTasks < 1)
            throw new ArgumentException($"there should be at least one task allowed to run, {maxTasks} is invalid", nameof(maxTasks));


         _throttler = new SemaphoreSlim(maxTasks);
      }

      /// <summary>
      /// Call this method to get a lock on the limiter. The call waits asynchronously until a slot is available.
      /// You are responsible for disposing the result in order to release the lock.
      /// </summary>
      /// <returns></returns>
      public async Task<IDisposable> AcquireOneAsync()
      {
         await _throttler.WaitAsync();

         return new LockRelease(this);
      }

      /// <summary>
      /// Disposes internal resources
      /// </summary>
      public void Dispose()
      {
         _throttler.Dispose();
      }

      private class LockRelease : IDisposable
      {
         private readonly AsyncLimiter _parent;

         public LockRelease(AsyncLimiter parent)
         {
            _parent = parent;
         }

         public void Dispose()
         {
            _parent._throttler.Release();
         }
      }
   }
}
