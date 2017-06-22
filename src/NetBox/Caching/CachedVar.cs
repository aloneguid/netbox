using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetBox.Caching
{
   /// <summary>
   /// Implements a lazy value i.e. that can expire in future
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class LazyVar<T>
   {
      private readonly Func<Task<T>> _renewFunc;
      private DateTime _lastRenewed = DateTime.MinValue;
      private readonly TimeSpan _timeToLive;
      private T _value;

      /// <summary>
      /// Creates an instance of a lazy variable with time-to-live value
      /// </summary>
      /// <param name="timeToLive">Time to live</param>
      /// <param name="renewFunc"></param>
      public LazyVar(TimeSpan timeToLive, Func<Task<T>> renewFunc)
      {
         _timeToLive = timeToLive;
         _renewFunc = renewFunc;
      }

      /// <summary>
      /// Gets the values, renewing it if necessary
      /// </summary>
      /// <returns>Value</returns>
      public async Task<T> GetValueAsync()
      {
         bool expired = (DateTime.UtcNow - _lastRenewed) > _timeToLive;

         if(expired)
         {
            _value = await _renewFunc();
            _lastRenewed = DateTime.UtcNow;
         }

         return _value;
      }
   }
}
